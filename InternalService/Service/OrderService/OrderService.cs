using AutoMapper;
using InternalService.Models;
using InternalService.Service;
using InternalService.Service.Argument.Order;
using InternalService.Service.DishService;

namespace InternalService.Service.OrderService;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IDishService _dishService;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository repository, IDishService dishService, IMapper mapper)
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
        var mappedOrder = _mapper.Map<CreateOrderArgument, Order>(argument);
        mappedOrder.DateOfCreation = DateTime.Now;
        mappedOrder.Status = OrderStatus.WaitingForPayment;
        for (var i = 0; i < mappedOrder.Dishes.Count;i++)
        {
            var oldDish = mappedOrder.Dishes.First();
            var newDish = _dishService.Get(oldDish.Id);
            mappedOrder.Dishes.Remove(oldDish);
            mappedOrder.Dishes.Add(newDish);
            mappedOrder.Price += newDish.Price;
        }
        
        return _repository.Create(mappedOrder);
    }
}

