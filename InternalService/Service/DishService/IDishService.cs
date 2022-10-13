using InternalService.Models;

namespace InternalService.Service.DishService;

public interface IDishService
{
    public Task<IEnumerable<Dish>> GetAllAsync();

    public Task<Dish> GetAsync(Guid id);
}