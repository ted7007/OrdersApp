using AutoMapper;
using InternalService.Repository;
using InternalService.Models;
using InternalService.Repository.Argument;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace InternalService.Repository;

public class OrderService : IRepository<Order, CreateOrderArgument, UpdateOrderArgument>
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public OrderService(ApplicationContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }
    public Order Create(CreateOrderArgument argument)
    {
        //todo async methods
        var res = _context.Orders.Add(_mapper.Map<CreateOrderArgument, Order>(argument));
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

    public void Delete(Guid id)
    {
        _context.Orders.Remove(Get(id));
        _context.SaveChanges();
    }

    public void Update(UpdateOrderArgument argument)
    {
        _context.Orders.Update(_mapper.Map<UpdateOrderArgument, Order>(argument));
        _context.SaveChanges();
    }
}