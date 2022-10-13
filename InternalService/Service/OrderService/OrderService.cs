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
    
    /// <summary>
    /// Method for getting Order by id
    /// </summary>
    /// <param name="id">Order's id</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException">throws if Order wasnt found</exception>
    public Order Get(Guid id)
    {
        return _repository.Get(id) ?? throw new KeyNotFoundException($"order is not found with id {id}");
    }

    /// <summary>
    /// Method calls repository for create Order and returns created order
    /// </summary>
    /// <param name="argument">argument for creating order</param>
    /// <returns>created order</returns>
    public Order Create(CreateOrderArgument argument)
    {
        var orderForCreate = ProcessCreateArgumentAndGetOrder(argument);
        return _repository.Create(orderForCreate);
    }

    /// <summary>
    /// Calls repository for getting list of orders, that satisfies param
    /// </summary>
    /// <param name="param">param for getting List of orders</param>
    /// <returns>list of orders that satisfies param</returns>
    public IEnumerable<Order> GetList(OrderSearchParam param)
    {
        Func<Order, bool> predicate = GetPredicateFromOrderSearchParam(param);
        return  _repository.GetList(predicate);
    }

    /// <summary>
    /// Method process param for gettinf predicate
    /// </summary>
    /// <param name="param"></param>
    /// <returns>predicate from param</returns>
    private Func<Order, bool> GetPredicateFromOrderSearchParam(OrderSearchParam param)
    {
       return  order => 
                    (param.Customer == default || param.Customer.ToLower() == order.Customer.ToLower())
                    && (param.Price == default || param.Price == order.Price)
                    && (param.Status == default || param.Status == order.Status)
                    && (param.Type == default || param.Type == order.Type)
                    && (param.EmployeeId == default || param.EmployeeId == order.EmployeeId)
                    && (param.DateOfCreation == default || param.DateOfCreation == order.DateOfCreation);
    }

    /// <summary>
    /// method process argument for getting order
    /// </summary>
    /// <param name="argument"></param>
    /// <returns>order from argument</returns>
    private Order ProcessCreateArgumentAndGetOrder(CreateOrderArgument argument)
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

        return mappedOrder;
    }
}

