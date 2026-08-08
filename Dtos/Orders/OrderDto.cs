using GameStoreApi.Dtos.Users;

namespace GameStoreApi.Dtos.Orders;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
}