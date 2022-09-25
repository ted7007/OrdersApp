using AutoMapper;
using InternalService.Dto.Input.Order;
using InternalService.Dto.Output.Order;
using InternalService.Models;
using InternalService.Repository;
using InternalService.Repository.Argument;
using Microsoft.AspNetCore.Mvc;

namespace InternalService.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IRepository<Order, CreateOrderArgument, UpdateOrderArgument> _context;
    private readonly IMapper _mapper;

    public OrderController(IRepository<Order, CreateOrderArgument, UpdateOrderArgument> context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    [HttpPost]
    public ActionResult<OrderDto> Create(CreateOrderDto argument)
    {
        var mappedArgument = _mapper.Map<CreateOrderDto, CreateOrderArgument>(argument);
        var result = _context.Create(mappedArgument);
        var mappedResult = _mapper.Map<Order, OrderDto>(result);
        return new OkObjectResult(mappedResult);
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<OrderDto>> GetAll()
    {
        var Orders = _context.GetAll();
        var mappedOrders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(Orders);
        return new OkObjectResult(mappedOrders);
    }

    [HttpGet("{id}")]
    public ActionResult<OrderDto> Get(Guid id)
    {
        var Order = _context.Get(id);
        var mappedOrder = _mapper.Map<Order, OrderDto>(Order); 
        return new OkObjectResult(mappedOrder);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
        _context.Delete(id);
        return new OkResult();
    }

    [HttpPut]
    public ActionResult Update(UpdateOrderDto argument)
    {
        var mappedArgument = _mapper.Map<UpdateOrderDto, UpdateOrderArgument>(argument);
        _context.Update(mappedArgument);
        return new OkResult();
    }
}