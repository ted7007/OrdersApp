using InternalService.Models;
using InternalService.Service;

namespace InternalService.Service.DishService;

public class DishService : IDishService
{
    private readonly IDishRepository _repository;


    public DishService(IDishRepository repository)
    {
        _repository = repository;
    }
    
    public IEnumerable<Dish> GetAll()
    {
        return _repository.GetAll();
    }
    
    public Dish Get(Guid id)
    {
        return _repository.Get(id);
    }
    
}