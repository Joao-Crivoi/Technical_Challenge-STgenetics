namespace GoodHamburger.Api.Application.DTOs.Response.Order;

public class OrderResponseDTO
{
    public Guid Id { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
    public List<OrderItemDTO> Items { get; set; } = new();
}