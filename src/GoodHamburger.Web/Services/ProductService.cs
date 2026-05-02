using System.Net.Http.Json;
using GoodHamburger.Shared.Models;
using GoodHamburger.Shared.Constants;
using GoodHamburger.Shared.DTOs.Response.Product;

namespace GoodHamburger.Web.Services;

public interface IProductService 
{
    Task<IEnumerable<ProductResponseDTO>> GetProductsAsync();
}

public class ProductService : IProductService
{
    private readonly HttpClient _http;

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public async Task<IEnumerable<ProductResponseDTO>> GetProductsAsync()
    {
        // Usando a constante centralizada do Shared!
        var response = await _http.GetFromJsonAsync<ApiResponse<IEnumerable<ProductResponseDTO>>>(ApiRoutes.Products);
        return response?.Data ?? Enumerable.Empty<ProductResponseDTO>();
    }
}