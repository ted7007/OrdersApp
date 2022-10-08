using InternalService.Models;

namespace InternalService.Service.Argument.Order;

public class CreateOrderArgument
{
    public string Customer { get; set; }

    public Guid EmployeeId { get; set; }
    
    
    public OrderType Type { get; set; }
    
    public ICollection<Dish> Dishes { get; set; }
}
