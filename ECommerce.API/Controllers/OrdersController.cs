using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("GetOrderById/{orderId}")]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);

        return Ok(order);
    }

    [HttpGet("GetAllOrders")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();

        return Ok(orders);
    }

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateDto createDto)
    {
        var validator = new OrderCreateDtoValidator(); //örnek olarak FluentValidation kullanarak DTO'yu doğruluyoruz
        validator.ValidateAndThrow(createDto);

        var result = await _orderService.CreateOrderAsync(createDto);

        return Ok(result);
    }

    [HttpPost("CompleteOrder")]
    public async Task<IActionResult> CompleteOrder([FromBody] CompleteDto completeDto)
    {
        var validator = new OrderCompleteDtoValidator(); // örnek olarak FluentValidation kullanarak DTO'yu doğruluyoruz
        validator.ValidateAndThrow(completeDto);

        var result = await _orderService.CompleteOrderAsync(completeDto);

        return Ok(result);
    }

    [HttpPost("CancelOrder")]
    public async Task<IActionResult> CancelOrder([FromBody] CancelDto cancelDto)
    {
        var validator = new OrderCancelDtoValidator(); // örnek olarak FluentValidation kullanarak DTO'yu doğruluyoruz
        validator.ValidateAndThrow(cancelDto);

        var result = await _orderService.CancelOrderAsync(cancelDto);

        return Ok(result);
    }
}