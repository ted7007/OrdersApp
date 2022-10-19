using InternalService.Dto.Output.Dish;
using InternalService.Models;

namespace InternalService.Dto.Output.Order;

public class OrderDto
{
    public Guid Id { get; set; }

    public string Customer { get; set; }

    public decimal Price { get; set; }

    public Guid EmployeeId { get; set; }

    public DateTimeOffset DateOfCreation { get; set; }

    public string Status { get; set; }

    public OrderType Type { get; set; }
    
    
    public ICollection<DishDto> Dishes { get; set; }
}
