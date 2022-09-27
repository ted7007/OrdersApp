using InternalService.Models;
using InternalService.Repository.Argument.Order;

namespace InternalService.Repository;

public interface IOrderRepository
{
    public Order Create(CreateOrderArgument argument);
    
    public IEnumerable<Order> GetAll();

    public Order Get(Guid id);
}