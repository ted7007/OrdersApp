namespace InternalService.Repository.Order;

public interface IOrderRepository
{
    public Models.Order Create(Models.Order order);

    public Models.Order Get(Guid id);
    
    IEnumerable<Models.Order> GetList(Func<Models.Order, bool> predicate);
}