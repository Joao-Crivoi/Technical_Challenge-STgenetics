using Xunit;
using System.Net;
using System.Net.Http.Json;
using GoodHamburger.Shared.Models;
using GoodHamburger.Shared.Constants; 
using GoodHamburger.Tests.Infrastructure;
using GoodHamburger.Shared.DTOs.Request.Order;
using GoodHamburger.Shared.DTOs.Response.Order;

namespace GoodHamburger.Tests.Integration.Controllers;

public class OrderControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrderControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    #region Create

    [Fact]
    public async Task CreateOrder_WithFullCombo_ShouldApply20PercentDiscount()
    {
        var request = new CreateOrderRequestDTO { ProductIds = new List<int> { 1, 4, 5 } };

        var response = await _client.PostAsJsonAsync(ApiRoutes.Orders, request);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderResponseDTO>>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(result?.Data);
        Assert.Equal(7.6m, result.Data.Total);
    }

    [Fact]
    public async Task CreateOrder_WithSandwichAndDrink_ShouldApply15PercentDiscount()
    {
        var request = new CreateOrderRequestDTO { ProductIds = new List<int> { 1, 5 } };
        
        var response = await _client.PostAsJsonAsync(ApiRoutes.Orders, request);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderResponseDTO>>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(6.375m, result?.Data?.Total);
    }

    [Fact]
    public async Task CreateOrder_WithInvalidProductId_ShouldReturnNotFound()
    {
        var request = new CreateOrderRequestDTO { ProductIds = new List<int> { 999 } };

        var response = await _client.PostAsJsonAsync(ApiRoutes.Orders, request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    #endregion

    #region Read

    [Fact]
    public async Task GetOrder_WithInvalidId_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync($"{ApiRoutes.Orders}/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion
}