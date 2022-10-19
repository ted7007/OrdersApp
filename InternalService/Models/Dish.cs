namespace InternalService.Models;

public class Dish
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public float Calory { get; set; }

    public decimal Price { get; set; }

    public int CountOrders { get; set; }
}