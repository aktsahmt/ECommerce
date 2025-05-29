using AutoMapper;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ECommerce.Tests;
public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly OrderService _orderService;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IClientBalanceService> _balanceServiceMock;
    private readonly Mock<IBalanceRepository> _balanceRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public OrderServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _balanceServiceMock = new Mock<IClientBalanceService>();
        _balanceRepositoryMock = new Mock<IBalanceRepository>();
        _mapperMock = new Mock<IMapper>();
        _orderService = new OrderService(
            _unitOfWorkMock.Object,
            _orderRepositoryMock.Object,
            _balanceServiceMock.Object,
            _balanceRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public void Constructor_Should_InitializeDependencies()
    {
        Assert.NotNull(_orderService);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var orderId = Guid.CreateVersion7();
        var order = new OrderHeader { Id = orderId, UserId = Guid.NewGuid(), Status = "Pending" };
        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(order);
        _mapperMock.Setup(m => m.Map<OrderHeaderDto>(order)).Returns(new OrderHeaderDto());

        // Act
        var result = await _orderService.GetOrderByIdAsync(orderId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ShouldThrowException_WhenOrderDoesNotExist()
    {
        // Arrange
        var orderId = Guid.CreateVersion7();
        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync((OrderHeader)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _orderService.GetOrderByIdAsync(orderId));
    }

    [Fact]
    public async Task GetAllOrdersAsync_ShouldThrowException_WhenNoOrdersExist()
    {
        // Arrange
        _orderRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<OrderHeader>());
        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _orderService.GetAllOrdersAsync());
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldCreateOrder_WhenValidDataProvided()
    {
        // Arrange
        var createDto = new CreateDto
        {
            UserId = Guid.NewGuid(),
            Items = new List<OrderItemDto>
            {
                new OrderItemDto { Price = 100, Quantity = 2 }
            }
        };

        var total = createDto.Items.Sum(x => x.Price * x.Quantity);

        // Act
        var result = await _orderService.CreateOrderAsync(createDto);
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        result.Data.OrderId.Should().NotBeEmpty();
    }
}