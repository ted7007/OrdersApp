using InternalService.Models;

namespace InternalService.Service.Param;

public class OrderSearchParam
{
    public string? Customer { get; set; }

    public decimal Price { get; set; }

    public Guid EmployeeId { get; set; }

    public DateTimeOffset DateOfCreation { get; set; }

    public OrderStatus? Status { get; set; }

    public OrderType? Type { get; set; }
}