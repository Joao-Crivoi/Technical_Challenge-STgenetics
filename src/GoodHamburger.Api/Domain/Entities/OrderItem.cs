namespace GoodHamburger.Api.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public int ProductId { get; set; }
    
    public decimal UnitPrice { get; set; } 
    
    public virtual Product Product { get; set; } = null!;

    private OrderItem() { }

    public OrderItem(Guid orderId, Product product)
    {
        OrderId = orderId;
        ProductId = product.Id;
        Product = product;
        UnitPrice = product.Price; 
    }
}