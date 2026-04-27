using GoodHamburger.Api.Domain.Enums; 

namespace GoodHamburger.Api.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ProductCategory Category { get; set; }

    public Product() { }
    public Product(string name, decimal price, ProductCategory category)
    {
        Name = name;
        Price = price;
        Category = category;
    }
}