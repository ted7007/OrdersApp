using InternalService.Models;
using InternalService.Repository.Dish;
using InternalService.Service;

namespace InternalService.Service.DishService;

public class DishService : IDishService
{
    private readonly IDishRepository _repository;


    public DishService(IDishRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<Dish>> GetAllAsync()
    {
        return await _repository.GetAll();
    }
    
    public async Task<Dish> GetAsync(Guid id)
    {
        return await (_repository.Get(id) ?? throw new KeyNotFoundException($"dish is not found with id {id}"));
    }
    
}