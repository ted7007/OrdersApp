namespace InternalService.Models;

public class Order
{
    public Guid Id { get; set; }

    public string Customer { get; set; }

    public decimal Price { get; set; }

    public Guid EmployeeId { get; set; }

    public DateTime DateOfCreation { get; set; }    // SHOULD BE IN UTC!!!

    public OrderStatus Status { get; set; }

    public OrderType Type { get; set; }
    
    public ICollection<Dish> Dishes { get; set; }
}