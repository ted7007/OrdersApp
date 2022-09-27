using InternalService.Models;

namespace InternalService.Repository;

public interface IDishRepository
{
    
    public IEnumerable<Dish> GetAll();

    public Dish Get(Guid id);
}