using InternalService.Models;

namespace InternalService.Service;

public interface IDishRepository
{
    
    public Task<IEnumerable<Models.Dish>> GetAll();

    public Task<Models.Dish> Get(Guid id);
}