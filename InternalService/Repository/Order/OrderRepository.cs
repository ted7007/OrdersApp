using AutoMapper;
using InternalService.Service;
using Microsoft.EntityFrameworkCore;

namespace InternalService.Repository.Order;
                                                                                               
public class OrderRepository : IOrderRepository
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public OrderRepository(ApplicationContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }
    public Models.Order Create(Models.Order order)
    {
        //todo async methods
        var res = _context.Orders.Add(order);
        _context.SaveChanges();
        
        return res.Entity;
    }

    public Models.Order Get(Guid id)
    {
        return _context.Orders
                                .Include(o => o.Dishes)
                                .First(o => o.Id == id);
    }

    public IEnumerable<Models.Order> GetList(Func<Models.Order, bool> predicate)
    {
        return _context.Orders
                                .Include(o => o.Dishes)
                                .Where(predicate)
                                .ToList();
        
    }
}