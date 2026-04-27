using GoodHamburger.Api.Application.Interfaces;
using GoodHamburger.Api.Domain.Entities;

namespace GoodHamburger.Api.Domain.Strategies;

public class FullComboStrategy : IDiscountStrategy
{
    public bool CanApply(IEnumerable<OrderItem> items) =>
        items.Count() == 3; 

    public decimal Calculate(decimal subtotal) => subtotal * 0.20m;
}