using InternalService.Service;
using Microsoft.EntityFrameworkCore;

namespace InternalService.Repository.Dish;

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
                                .ToListAsync();
    }
    
    public async Task<Models.Dish> Get(Guid id)
    {
        return await _context.Dishes
                                .FirstAsync(d => d.Id==id);
    }

    public void Update(Models.Dish dish)
    { 
        _context.Dishes.Update(dish);
        _context.SaveChanges();
    }
}