using AutoMapper;
using InternalService.Dto.Input.Order;
using InternalService.Dto.Output.Order;
using InternalService.Models;
using InternalService.Repository.Argument;

namespace InternalService.AutoMapperProfiles;

public class OrderDtoMapperConfiguration : Profile
{
    public OrderDtoMapperConfiguration()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<OrderDto, Order>();
        
        CreateMap<CreateOrderArgument, CreateOrderDto>();
        CreateMap<CreateOrderDto, CreateOrderArgument>();
        CreateMap<UpdateOrderArgument, UpdateOrderDto>();
        CreateMap<UpdateOrderDto, UpdateOrderArgument>();
        
    }
}