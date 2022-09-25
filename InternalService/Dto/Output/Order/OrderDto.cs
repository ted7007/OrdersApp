using InternalService.Models;

namespace InternalService.Dto.Output.Order;

public class OrderDto
{
    public Guid Id { get; set; }

    public string Customer { get; set; }

    public decimal Price { get; set; }

    public Guid EmployeeId { get; set; }

    public DateTime DateOfCreation { get; set; }

    public OrderStatus Status { get; set; }
}
