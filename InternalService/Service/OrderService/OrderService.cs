using InternalService.Repository;
using InternalService.Repository.Argument.Order;

namespace InternalService.Services.Order;

public class OrderServices : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderServices(IOrderRepository repository)
    {
        _repository = repository;
    }
    
    public IEnumerable<Order> GetAll()
    {
        throw new NotImplementedException();
    }

    public Order Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Order Create(CreateOrderArgument argument)
    {
        throw new NotImplementedException();
    }
}