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
    public async Task<Models.Order> CreateAsync(Models.Order order)
    {
        order = UpdateToUtc(order);
        var res = await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        
        return res.Entity;
    }

    public async Task<Models.Order> GetAsync(Guid id)
    {
        return await _context.Orders
                                .Include(o => o.Dishes)
                                .FirstAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Models.Order>> GetListAsync(Func<Models.Order, bool> predicate)
    {
        return await _context.Orders
            .Include(o => o.Dishes)
            .Where(predicate)
            .AsQueryable()
            .ToListAsyncSafe();

    }

    private Models.Order UpdateToUtc(Models.Order order)
    {
         order.DateOfCreation = DateTime.SpecifyKind(order.DateOfCreation, DateTimeKind.Utc);
         return order;
    }
}