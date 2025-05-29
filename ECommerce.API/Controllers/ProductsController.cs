using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class ProductsController : BaseController
{
    private readonly IClientBalanceService _balanceService;

    public ProductsController(IClientBalanceService balanceService)
    {
        _balanceService = balanceService;
    }

    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _balanceService.GetProductsAsync();
        return Ok(products);
    }
}
