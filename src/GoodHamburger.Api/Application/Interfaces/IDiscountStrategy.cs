using GoodHamburger.Api.Domain.Entities;

namespace GoodHamburger.Api.Application.Interfaces;
public interface IDiscountStrategy
{
    bool CanApply(IEnumerable<OrderItem> items);
    decimal Calculate(decimal subtotal);
}