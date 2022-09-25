using InternalService.Models;
using InternalService.Repository.Argument;

namespace InternalService.Repository;

public interface IOrderRepository
{
    public Order Create(CreateOrderArgument argument);
    
    public IEnumerable<Order> GetAll();

    public Order Get(Guid id);

    public void Delete(Guid id);

    public void Update(UpdateOrderArgument argument);
}