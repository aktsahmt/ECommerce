using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class BalancesController : BaseController
{

    private readonly IBalanceService _balanceService;

    public BalancesController(IBalanceService balanceService)
    {
        _balanceService = balanceService;
    }

    [HttpGet("GetBalanceByUserId/{userId}")]
    public async Task<IActionResult> GetBalanceByUserId(string userId)
    {
        var balance = await _balanceService.GetBalanceByUserIdAsync(userId);

        return Ok(balance);
    }

    [HttpGet("GetAllBalance")]
    public async Task<IActionResult> GetAllBalance()
    {
        var balances = await _balanceService.GetAllBalanceAsync();
        return Ok(balances);
    }
}

