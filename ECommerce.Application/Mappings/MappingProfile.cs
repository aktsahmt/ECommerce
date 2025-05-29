using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.BalanceManagement.Order;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Balance, BalanceDto>().ReverseMap();
        CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
    }
}
