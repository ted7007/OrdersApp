using AutoMapper;
using InternalService.Repository;
using InternalService.Models;
using InternalService.Repository.Argument;
using InternalService.Repository.Argument.Order;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;

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
    public Order Create(Order order)
    {
        //todo async methods
        var res = _context.Orders.Add(order);
        _context.SaveChanges();
        
        return res.Entity;
    }

    public IEnumerable<Order> GetAll()
    {
        return _context.Orders
                                .Include(o => o.Dishes).ToList();
    }

    public Order Get(Guid id)
    {
        return _context.Orders
                                .Include(o => o.Dishes)
                                .First(o => o.Id == id);
    }
    
}