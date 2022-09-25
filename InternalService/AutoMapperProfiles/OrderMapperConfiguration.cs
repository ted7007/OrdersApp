using AutoMapper;
using InternalService.Models;
using InternalService.Repository.Argument;

namespace InternalService.AutoMapperProfiles;

public class OrderMapperConfiguration : Profile
{
    public OrderMapperConfiguration()
    {
        CreateMap<Order, CreateOrderArgument>();
        CreateMap<CreateOrderArgument, Order>();
        CreateMap<Order, UpdateOrderArgument>();
        CreateMap<UpdateOrderArgument, Order>();
    }
}