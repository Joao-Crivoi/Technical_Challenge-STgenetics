using GoodHamburger.Api.Domain.Enums;
using GoodHamburger.Api.Application.Interfaces;
using GoodHamburger.Api.Domain.Entities;

namespace GoodHamburger.Api.Domain.Strategies;

public class SandwichDrinkStrategy : IDiscountStrategy
{
    public bool CanApply(IEnumerable<OrderItem> items) =>
        items.Count() == 2 && 
        items.Any(i => i.Product.Category == ProductCategory.Sandwich) &&
        items.Any(i => i.Product.Category == ProductCategory.Drink);

    public decimal Calculate(decimal subtotal) => subtotal * 0.15m;
}