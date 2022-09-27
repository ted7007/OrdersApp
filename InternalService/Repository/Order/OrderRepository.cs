using AutoMapper;
using InternalService.Repository;
using InternalService.Models;
using InternalService.Repository.Argument;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace InternalService.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public OrderRepository(ApplicationContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }
    public Order Create(CreateOrderArgument argument)
    {
        //todo async methods
        var mappedArgument = _mapper.Map<CreateOrderArgument, Order>(argument);
        mappedArgument.DateOfCreation=DateTime.Now;
        mappedArgument.Status = OrderStatus.WaitingForPayment;
        var res = _context.Orders.Add(mappedArgument);
        _context.SaveChanges();
        
        return res.Entity;
    }

    public IEnumerable<Order> GetAll()
    {
        return _context.Orders.ToList();
    }

    public Order Get(Guid id)
    {
        return _context.Orders.Find(id) ?? throw new InvalidOperationException();
    }
    
}