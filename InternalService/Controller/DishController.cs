using AutoMapper;
using InternalService.Dto.Input.Order;
using InternalService.Dto.Output.Dish;
using InternalService.Dto.Output.Order;
using InternalService.Models;
using InternalService.Service;
using InternalService.Service.Argument;
using InternalService.Service.DishService;
using Microsoft.AspNetCore.Mvc;

namespace InternalService.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class DishController
{
    private readonly IMapper _mapper;
    private readonly IDishService _service;

    public DishController(IDishService service, IMapper mapper)
    {
        _mapper = mapper;
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAllAsync()
    {
        var dishes = await _service.GetAllAsync();
        var mappedOrders = _mapper.Map<IEnumerable<Dish>, IEnumerable<DishDto>>(dishes);
        return new OkObjectResult(mappedOrders);
    }
    
}