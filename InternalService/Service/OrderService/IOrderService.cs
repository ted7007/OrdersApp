using InternalService.Models;
using InternalService.Service.Argument.Order;
using InternalService.Service.Param;

namespace InternalService.Service.OrderService;

public interface IOrderService
{
    public IEnumerable<Order> GetAll();

    public IEnumerable<Order> GetList(OrderSearchParam param);

    public Order Get(Guid id);

    public Order Create(CreateOrderArgument argument);
}