using GoodHamburger.Api.Domain.Entities;
using GoodHamburger.Api.Domain.Enums;
using GoodHamburger.Api.Domain.Exceptions;
using Xunit;

namespace GoodHamburger.Api.Tests.Unit.Domain.Entities;

public class OrderTests
{
    [Fact]
    public void AddItem_ShouldThrowException_WhenAddingDuplicateCategory()
    {
        var order = new Order();
        var product1 = new Product { Id = 1, Name = "X Burger", Category = ProductCategory.Sandwich };
        var product2 = new Product { Id = 2, Name = "X Bacon", Category = ProductCategory.Sandwich };
        
        order.AddItem(product1);

        var exception = Assert.Throws<DomainException>(() => order.AddItem(product2));
        Assert.Equal("O pedido já contém um item da categoria Sandwich.", exception.Message);
    }


    [Fact]
    public void Order_ShouldCalculateCorrectSubtotal_WithoutDiscounts()
    {
        var order = new Order();
        var burger = new Product { Id = 1, Name = "X Burger", Price = 5.0m, Category = ProductCategory.Sandwich };
        
        order.AddItem(burger);

        Assert.Equal(5.0m, order.Subtotal);
        Assert.Equal(0, order.DiscountAmount);
        Assert.Equal(5.0m, order.Total);
    }

    [Fact]
    public void Order_ShouldMaintainOriginalPrice_EvenIfProductPriceChangesLater()
    {
        var burger = new Product { Id = 1, Name = "X Burger", Price = 5.0m, Category = ProductCategory.Sandwich };
        var orderItem = new OrderItem(Guid.NewGuid(), burger);

        burger.Price = 10.0m;

        Assert.Equal(5.0m, orderItem.UnitPrice);
    }


}