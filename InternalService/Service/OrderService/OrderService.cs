using AutoMapper;
using InternalService.Models;
using InternalService.Repository.Order;
using InternalService.Service;
using InternalService.Service.Argument.Order;
using InternalService.Service.DishService;
using InternalService.Service.Param;

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

    public Order Get(Guid id)
    {
        return _repository.Get(id) ?? throw new KeyNotFoundException($"order is not found with id {id}");
    }

    public Order Create(CreateOrderArgument argument)
    {
        var mappedOrder = _mapper.Map<CreateOrderArgument, Order>(argument);
        mappedOrder.DateOfCreation = DateTimeOffset.UtcNow;
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

    public IEnumerable<Order> GetList(OrderSearchParam param)
    {
        Func<Order, bool> predicate = order =>
            (param.Customer == default || param.Customer.ToLower() == order.Customer.ToLower())
            && (param.Price == default || param.Price == order.Price)
            && (param.Status == default || param.Status == order.Status)
            && (param.Type == default || param.Type == order.Type)
            && (param.EmployeeId == default || param.EmployeeId == order.EmployeeId)
            && (param.DateOfCreation == default || param.DateOfCreation == order.DateOfCreation);

        return  _repository.GetList(predicate);
    }
}

