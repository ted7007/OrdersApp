using InternalService.Dto.Output.Dish;
using InternalService.Models;

namespace InternalService.Dto.Input.Order;

public class CreateOrderDto
{

    public string Customer { get; set; }

    public Guid EmployeeId { get; set; }

    public OrderType Type { get; set; }
    
    public ICollection<InputDishDto> Dishes { get; set; }
}
