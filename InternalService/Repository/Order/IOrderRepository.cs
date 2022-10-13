namespace InternalService.Service.Order;

public interface IOrderRepository
{
    public Task<Models.Order> CreateAsync(Models.Order order);

    public Task<Models.Order> GetAsync(Guid id);
    
    public Task<IEnumerable<Models.Order>> GetListAsync(Func<Models.Order, bool> predicate);
}