using InternalService.Models;

namespace InternalService.Service.DishService;

public interface IDishService
{
    public IEnumerable<Dish> GetAll();

    public Dish Get(Guid id);
}