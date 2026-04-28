using GoodHamburger.Api.Domain.Entities;
using GoodHamburger.Api.Domain.Enums;
using GoodHamburger.Api.Domain.Strategies;
using Xunit;

namespace GoodHamburger.Api.Tests.Unit.Domain.Strategies;

public class DiscountStrategyTests
{
    [Theory]
    [InlineData(10, 1.0)] // 10% of 10 = 1.0
    [InlineData(100, 10.0)] // 10% of 100 = 10.0
    public void SandwichSideStrategy_ShouldCalculate_10_Percent(decimal subtotal, decimal expectedDiscount)
    {
        var strategy = new SandwichSideStrategy();
        var result = strategy.Calculate(subtotal);
        Assert.Equal(expectedDiscount, result);
    }

    [Fact]
    public void FullComboStrategy_ShouldCalculate_20_Percent()
    {
        var strategy = new FullComboStrategy();
        var subtotal = 10.0m;
        var result = strategy.Calculate(subtotal);
        Assert.Equal(2.0m, result); // 20% of 10
    }

    [Fact]
    public void NoDiscount_ShouldReturnZero_WhenOnlyOneItemIsPresent()
    {
        var items = new List<OrderItem> { 
            new OrderItem(Guid.NewGuid(), new Product { Category = ProductCategory.Side, Price = 2.0m }) 
        };
        
        var combo20 = new FullComboStrategy();
        
        Assert.False(combo20.CanApply(items));
    }
}