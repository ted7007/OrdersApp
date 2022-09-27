using AutoMapper;
using InternalService.Dto.Input.Order;
using InternalService.Dto.Output.Order;
using InternalService.Models;
using InternalService.Repository.Argument;
using InternalService.Repository.Argument.Order;

namespace InternalService.AutoMapperProfiles;

public class OrderMapperConfiguration : Profile
{
    public OrderMapperConfiguration()
    {
        CreateMap<Order, CreateOrderArgument>();
        CreateMap<CreateOrderArgument, Order>();
        
        CreateMap<Order, OrderDto>();
        
        CreateMap<CreateOrderDto, CreateOrderArgument>();
        
        
        
    }
}