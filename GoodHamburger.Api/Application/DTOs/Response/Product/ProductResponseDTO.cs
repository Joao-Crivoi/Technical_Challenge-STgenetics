namespace GoodHamburger.Api.Application.DTOs.Response.Product;

public class ProductResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
}