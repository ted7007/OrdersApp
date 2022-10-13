using InternalService.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalService.Service;

public class DishRepository : IDishRepository
{
    private readonly ApplicationContext _context;

    public DishRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Models.Dish>> GetAll()
    {
        return await _context.Dishes
                                .Include(d => d.Orders)
                                .ToListAsync();
    }
    
    public async Task<Models.Dish> Get(Guid id)
    {
        return await _context.Dishes
                                .Include(d => d.Orders)
                                .FirstAsync(d => d.Id==id);
    }
}