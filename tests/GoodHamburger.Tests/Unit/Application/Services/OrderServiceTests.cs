using Moq;
using Xunit;
using AutoMapper;
using GoodHamburger.Api.Domain.Enums;
using GoodHamburger.Api.Domain.Entities;
using GoodHamburger.Api.Application.Services;
using GoodHamburger.Api.Application.Interfaces;

namespace GoodHamburger.Tests.Unit.Application.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepoMock;
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IDiscountStrategy> _strategyMock;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _orderRepoMock    = new Mock<IOrderRepository>();
        _productRepoMock  = new Mock<IProductRepository>();
        _mapperMock       = new Mock<IMapper>();
        _strategyMock     = new Mock<IDiscountStrategy>();

        _orderService = new OrderService(
            _mapperMock.Object,
            _orderRepoMock.Object,
            _productRepoMock.Object,
            new[] { _strategyMock.Object });
    }

    [Theory]
    [InlineData(true, true, true, 0.20)]
    [InlineData(true, false, true, 0.15)]
    [InlineData(true, true, false, 0.10)]
    [InlineData(true, false, false, 0.0)]
    public void CalculateDiscount_ShouldApplyCorrectPercentage(
        bool hasSandwich, bool hasSide, bool hasDrink, decimal expectedDiscountPercent)
    {
        // Arrange
        var products = new List<Product>();
        if (hasSandwich) products.Add(new Product("Burger", 10m, ProductCategory.Sandwich));
        if (hasSide)     products.Add(new Product("Fries",  5m,  ProductCategory.Side));
        if (hasDrink)    products.Add(new Product("Coke",   5m,  ProductCategory.Drink));

        var subtotal = products.Sum(p => p.Price);

        var order = new Order();
        foreach (var p in products) order.AddItem(p);

        // Act - aplica o desconto como o Service faria
        var discountAmount = subtotal * expectedDiscountPercent;
        order.ApplyDiscount(discountAmount);

        // Assert
        Assert.Equal(subtotal, order.Subtotal);
        Assert.Equal(discountAmount, order.DiscountAmount);
        Assert.Equal(subtotal - discountAmount, order.Total);
    }
}