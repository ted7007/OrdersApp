using InternalService.Models;
using InternalService.Service.Argument.Order;
using InternalService.Service.Param;

namespace InternalService.Service.OrderService;

public interface IOrderService
{
    public Task<IEnumerable<Models.Order>> GetListAsync(OrderSearchParam param);

    public Task<Models.Order> GetAsync(Guid id);

    public Task<Models.Order> CreateAsync(CreateOrderArgument argument);
}