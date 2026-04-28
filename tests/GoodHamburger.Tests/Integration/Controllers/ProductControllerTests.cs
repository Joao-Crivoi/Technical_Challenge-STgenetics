using Xunit;
using System.Net;
using System.Net.Http.Json;
using GoodHamburger.Shared.Models;
using GoodHamburger.Shared.Constants;
using GoodHamburger.Tests.Infrastructure;
using GoodHamburger.Api.Application.DTOs.Response.Product;

namespace GoodHamburger.Tests.Integration.Controllers;

public class ProductControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnSeededProducts()
    {
        var response = await _client.GetAsync(ApiRoutes.Products);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<ProductResponseDTO>>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result?.Data);
        Assert.NotEmpty(result.Data); 
        
        Assert.Contains(result.Data, p => p.Name.Contains("Sandwich") || p.Id == 1);
    }
}