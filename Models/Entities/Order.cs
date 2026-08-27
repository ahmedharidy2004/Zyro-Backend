namespace GameStoreApi.Models.Entities;

public class Order
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public DateTime CreatedAt { get; set; }

    public decimal TotalPrice { get; set; }

    public User User { get; set; } = null!;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}