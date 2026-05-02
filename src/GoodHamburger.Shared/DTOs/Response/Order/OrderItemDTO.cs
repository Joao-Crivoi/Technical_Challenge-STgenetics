namespace GoodHamburger.Shared.DTOs.Response.Order;

public class OrderItemDTO
{
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
}