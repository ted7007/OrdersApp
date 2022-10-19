namespace InternalService.Repository.Dish;

public interface IDishRepository
{
    
    public Task<IEnumerable<Models.Dish>> GetAll();

    public Task<Models.Dish> Get(Guid id);             

    public void Update(Models.Dish dish);
}