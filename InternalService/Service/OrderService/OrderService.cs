using AutoMapper;
using InternalService.Models;
using InternalService.Repository;
using InternalService.Repository.Argument.Order;
using InternalService.Service.DishService;

namespace InternalService.Service.OrderService;

public class OrderServices : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IDishService _dishService;
    private readonly IMapper _mapper;

    public OrderServices(IOrderRepository repository, IDishService dishService, IMapper mapper)
    {
        _repository = repository;
        _dishService = dishService;
        _mapper = mapper;
    }
    
    public IEnumerable<Order> GetAll()
    {
        return _repository.GetAll();
    }

    public Order Get(Guid id)
    {
        return _repository.Get(id);
    }

    public Order Create(CreateOrderArgument argument)
    {
        argument.DateOfCreation = DateTime.Now;
        argument.Status = OrderStatus.WaitingForPayment;
        for (var i = 0; i < argument.Dishes.Count;i++)
        {
            var oldDish = argument.Dishes.First();
            var newDish = _dishService.Get(oldDish.Id);
            argument.Dishes.Remove(oldDish);
            argument.Dishes.Add(newDish);
        }
        var mappedOrder = _mapper.Map<CreateOrderArgument, Order>(argument);
        return _repository.Create(mappedOrder);
    }
}

