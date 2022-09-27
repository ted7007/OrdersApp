using InternalService.Models;
using InternalService.Repository.Argument.Order;

namespace InternalService.Service.OrderService;

public interface IOrderService
{
    public IEnumerable<Order> GetAll();

    public Order Get(Guid id);

    public Order Create(CreateOrderArgument argument);
}