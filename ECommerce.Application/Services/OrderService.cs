using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.BalanceManagement.Order;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using System.Net;

namespace ECommerce.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IClientBalanceService _balanceService;
    private readonly IBalanceRepository _balanceRepository;
    private readonly IMapper _mapper;
    private const int MaxRetryCount = 3;
    private const int RetryInterval = 5;//from seconds


    public OrderService(IUnitOfWork unitOfWork,
                        IOrderRepository orderRepository,
                        IClientBalanceService balanceService,
                        IBalanceRepository balanceRepository,
                        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _balanceService = balanceService;
        _balanceRepository = balanceRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<OrderHeaderDto?>> GetOrderByIdAsync(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId)
            ?? throw new CustomException("Order not found.", "", (int)HttpStatusCode.NotFound);

        var orderDto = _mapper.Map<OrderHeaderDto>(order);

        return ServiceResult<OrderHeaderDto?>.Ok(orderDto, "Successfully.");
    }

    public async Task<ServiceResult<IEnumerable<OrderHeaderDto>>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();

        if (!orders.Any())
            throw new CustomException("No orders found.", "", (int)HttpStatusCode.NotFound);

        var orderDtos = _mapper.Map<IEnumerable<OrderHeaderDto>>(orders);

        return ServiceResult<IEnumerable<OrderHeaderDto>>.Ok(orderDtos, "Successfully.");
    }

    public async Task<ServiceResult<CreateResultDto>> CreateOrderAsync(CreateDto createDto)
    {
        var total = createDto.Items.Sum(x => x.Price * x.Quantity);
        var preOrderSuccess = false;
        var orderId = Guid.CreateVersion7();

        try
        {
            #region create pre-order
            var preOrderRootDto = await _balanceService.PreOrderAsync(new CreateOrderReqDto() { Amount = total, OrderId = orderId });

            preOrderSuccess = preOrderRootDto != null;
            #endregion

            //await _unitOfWork.BeginTransactionAsync(); // InMemory'de transaction kullanmaya gerek yok, çünkü bu işlem atomic olarak gerçekleşiyor.

            #region add order to db
            var orderHeader = new OrderHeader
            {
                Id = orderId,
                UserId = createDto.UserId,
                TotalAmount = total,
                Status = preOrderRootDto?.Order?.Status ?? "Pending",
                OrderLines = createDto.Items.Select(item => new OrderLine
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };
            await _orderRepository.AddOrderAsync(orderHeader);
            #endregion

            #region upsert balance to db

            var balance = _mapper.Map<Balance>(preOrderRootDto?.UpdatedBalance);

            await _balanceRepository.UpsertBalance(balance);
            #endregion

            await _unitOfWork.SaveChangesAsync();

            //await _unitOfWork.CommitAsync(); // InMemory'de commit'e gerek yok, çünkü işlemler zaten atomic olarak gerçekleşiyor.

            return ServiceResult<CreateResultDto>.Ok(new CreateResultDto() { OrderId = orderId }, "Order created successfully.");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();

            if (preOrderSuccess)
                await ProcessCancelByOrderIdAsync(orderId);
            throw;
        }
    }

    public async Task<ServiceResult<CompleteResultDto>> CompleteOrderAsync(CompleteDto completeDto)
    {
        try
        {
            #region pre-order complete
            var preOrderRootDto = await _balanceService.CompleteAsync(new CompleteOrderReqDto() { OrderId = completeDto.OrderId });
            #endregion

            // await _unitOfWork.BeginTransactionAsync(); // InMemory'de transaction kullanmaya gerek yok, çünkü bu işlem atomic olarak gerçekleşiyor.

            #region update order status to db
            await _orderRepository.UpdateOrderStatusByIdAsync(completeDto.OrderId, preOrderRootDto.Order.Status);
            #endregion

            #region upsert balance to db
            var balance = _mapper.Map<Balance>(preOrderRootDto.UpdatedBalance);

            await _balanceRepository.UpsertBalance(balance);
            #endregion

            await _unitOfWork.SaveChangesAsync();

            // await _unitOfWork.CommitAsync(); // InMemory'de commit'e gerek yok, çünkü işlemler zaten atomic olarak gerçekleşiyor.

            return ServiceResult<CompleteResultDto>.Ok(new CompleteResultDto() { OrderId = completeDto.OrderId }, "Order completed successfully.");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();

            throw;
        }
    }

    public async Task<ServiceResult<CancelResultDto>> CancelOrderAsync(CancelDto cancelDto)
    {
        try
        {
            await ProcessCancelAsync(cancelDto.OrderId);

            return ServiceResult<CancelResultDto>.Ok(new CancelResultDto() { OrderId = cancelDto.OrderId }, "Order cancelled successfully.");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task ProcessCancelAsync(Guid orderId)
    {
        #region pre-order cancellation
        var preOrderRootDto = await _balanceService.CancelAsync(new CancelOrderReqDto() { OrderId = orderId });
        #endregion

        // await _unitOfWork.BeginTransactionAsync(); // InMemory'de transaction kullanmaya gerek yok, çünkü bu işlem atomic olarak gerçekleşiyor.

        #region update order status to db
        await _orderRepository.UpdateOrderStatusByIdAsync(orderId, preOrderRootDto.Order.Status);
        #endregion

        #region upsert balance to db

        var balance = _mapper.Map<Balance>(preOrderRootDto.UpdatedBalance);

        await _balanceRepository.UpsertBalance(balance);
        #endregion

        await _unitOfWork.SaveChangesAsync();

        // await _unitOfWork.CommitAsync(); // InMemory'de commit'e gerek yok, çünkü işlemler zaten atomic olarak gerçekleşiyor.
    }

    // <summary>
    // Burada Quartz, Hangfire gibi bir cronjob ile yapılması daha sağlıklı olur.
    // MaxRetryCount aşılması durumunda, manuel olarak incelenme senaryosu kullanılabilir.
    // </summary>
    private async Task ProcessCancelByOrderIdAsync(Guid orderId)
    {
        for (int i = 0; i < MaxRetryCount; i++)
        {
            try
            {
                await ProcessCancelAsync(orderId);

                break;
            }
            catch (CustomException ex) when (ex.StatusCode == (int)HttpStatusCode.NotFound)
            {
                break; // Retry yapmaya gerek yok, çünkü zaten preorder yok
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();

                var retryInterval = TimeSpan.FromSeconds(RetryInterval);
                await Task.Delay((int)retryInterval.TotalMilliseconds); // 5000 ms
            }
        }
    }
}