using AutoMapper;
using InternalService.Dto.Input.Order;
using InternalService.Dto.Output.Order;
using InternalService.Models;
using InternalService.Service;
using InternalService.Service.Argument;
using InternalService.Service.Argument.Order;
using InternalService.Service.OrderService;
using InternalService.Service.Param;
using Microsoft.AspNetCore.Mvc;

namespace InternalService.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly IMapper _mapper;

    public OrderController(IOrderService service, IMapper mapper)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderDto argument)
    {
        var mappedArgument = _mapper.Map<CreateOrderDto, CreateOrderArgument>(argument);
        var result = await _service.CreateAsync(mappedArgument);
        var mappedResult = _mapper.Map<Order, OrderDto>(result);
        return new OkObjectResult(mappedResult);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetList([FromHeader]OrderSearchParam param)
    {
        var orders = await _service.GetListAsync(param);
        var mappedOrders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(orders);
        return new OkObjectResult(mappedOrders);

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> Get(Guid id)
    {
        var order = await _service.GetAsync(id);
        var mappedOrder = _mapper.Map<Order, OrderDto>(order);          
        return new OkObjectResult(mappedOrder);
    }
}