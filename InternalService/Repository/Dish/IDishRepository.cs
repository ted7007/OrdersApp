using InternalService.Models;

namespace InternalService.Service;

public interface IDishRepository
{
    
    public IEnumerable<Dish> GetAll();

    public Dish Get(Guid id);
}