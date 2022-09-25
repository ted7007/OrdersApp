using InternalService.Models;

namespace InternalService.Dto.Input.Order;

public class CreateOrderDto
{

    public string Customer { get; set; }

    public decimal Price { get; set; }

    public Guid EmployeeId { get; set; }
}
