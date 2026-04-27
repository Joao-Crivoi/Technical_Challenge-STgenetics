using GoodHamburger.Api.Domain.Entities;
using GoodHamburger.Api.Domain.Enums;
using GoodHamburger.Api.Infrastructure.Data;

namespace GoodHamburger.Api.Infrastructure.Data.Helpers;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        if (context.Products.Any()) return;

        var products = new List<Product>
        {
            new Product("X Burger", 5.00m, ProductCategory.Sandwich),
            new Product("X Egg", 4.50m, ProductCategory.Sandwich),  
            new Product("X Bacon", 7.00m, ProductCategory.Sandwich),
            new Product("Batata frita", 2.00m, ProductCategory.Side),
            new Product("Refrigerante", 2.50m, ProductCategory.Drink)
        };

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}