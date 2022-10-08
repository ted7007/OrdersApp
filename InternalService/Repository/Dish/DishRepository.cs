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

    public IEnumerable<Dish> GetAll()
    {
        return _context.Dishes
                                .Include(d => d.Orders)
                                .ToList();
    }
    
    public Dish Get(Guid id)
    {
        return _context.Dishes
                                .Include(d => d.Orders)
                                .First(d => d.Id==id);
    }
}