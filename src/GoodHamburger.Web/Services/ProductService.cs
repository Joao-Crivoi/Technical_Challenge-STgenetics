using System.Net.Http.Json;
using GoodHamburger.Shared.Models;
using GoodHamburger.Shared.Constants;
using GoodHamburger.Shared.DTOs.Response.Product;

namespace GoodHamburger.Web.Services;

public class ProductApiService
{
    private readonly HttpClient _http;

    public ProductApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<IEnumerable<ProductResponseDTO>> GetMenuAsync()
    {
        var result = await _http.GetFromJsonAsync<ApiResponse<IEnumerable<ProductResponseDTO>>>(ApiRoutes.Products);
        return result?.Data ?? Enumerable.Empty<ProductResponseDTO>();
    }
}