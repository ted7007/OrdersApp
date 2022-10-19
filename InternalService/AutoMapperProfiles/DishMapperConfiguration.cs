using AutoMapper;
using InternalService.Dto.Input.Order;
using InternalService.Dto.Output.Dish;
using InternalService.Models;

namespace InternalService.AutoMapperProfiles;

public class DishMapperConfiguration : Profile
{
    public DishMapperConfiguration()
    {
        CreateMap<Dish, DishDto>();
        CreateMap<InputDishDto, Dish>();
    }
}