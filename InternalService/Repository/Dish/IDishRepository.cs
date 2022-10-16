namespace InternalService.Repository.Dish;

public interface IDishRepository
{
    
    public Task<IEnumerable<Models.Dish>> GetAll();

    public Task<Models.Dish> Get(Guid id);
}