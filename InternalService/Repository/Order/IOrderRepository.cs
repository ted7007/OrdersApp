using InternalService.Models;
using InternalService.Service.Argument.Order;

namespace InternalService.Service;

public interface IOrderRepository
{
    public Order Create(Order order);
    
    public IEnumerable<Order> GetAll();

    public Order Get(Guid id);
}