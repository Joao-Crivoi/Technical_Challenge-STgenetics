using GoodHamburger.Api.Domain.Exceptions;

namespace GoodHamburger.Api.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public decimal Total { get; private set; }
    public decimal Subtotal { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public Order()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(Product product)
    {
        if (_items.Any(i => i.Product.Category == product.Category))
        {
            throw new DomainException($"O pedido já contém um item da categoria {product.Category}.");
        }

        _items.Add(new OrderItem(this.Id, product));
        
        Subtotal = _items.Sum(i => i.Product.Price);
        
        Total = Subtotal; 
    }
    public void ApplyDiscount(decimal amount)
    {
        if (amount < 0) throw new DomainException("O desconto não pode ser negativo.");
        
        DiscountAmount = amount;
        Total = Subtotal - DiscountAmount;
    }

    public void ClearItems()
    {
        _items.Clear();
        Subtotal = 0;
        DiscountAmount = 0;
        Total = 0;
    }
}