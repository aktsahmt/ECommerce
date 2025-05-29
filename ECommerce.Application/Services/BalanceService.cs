using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.DTOs.BalanceManagement.Order;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using System.Net;

namespace ECommerce.Application.Services;
public class BalanceService : IBalanceService
{
    private readonly IBalanceRepository _balanceRepository;
    private readonly IMapper _mapper;

    public BalanceService(IBalanceRepository balanceRepository, IMapper mapper)
    {
        _balanceRepository = balanceRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<BalanceDto>> GetBalanceByUserIdAsync(string userId)
    {
        var balance = await _balanceRepository.GetByUserIdAsync(userId)
            ?? throw new CustomException("Balance not found for the user.", "", (int)HttpStatusCode.NotFound);

        var balanceDto = _mapper.Map<BalanceDto>(balance);

        return ServiceResult<BalanceDto>.Ok(balanceDto, "Successfully.");
    }

    public async Task<ServiceResult<IEnumerable<BalanceDto>>> GetAllBalanceAsync()
    {
        var balances = await _balanceRepository.GetAllAsync();

        if (!balances.Any())
            throw new CustomException("No balances found.", "", (int)HttpStatusCode.NotFound);

        var balanceDtos = _mapper.Map<IEnumerable<BalanceDto>>(balances);
        return ServiceResult<IEnumerable<BalanceDto>>.Ok(balanceDtos, "Successfully retrieved all balances.");
    }
}
