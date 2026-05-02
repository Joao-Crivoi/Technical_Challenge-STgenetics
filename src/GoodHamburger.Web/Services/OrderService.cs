using System.Net.Http.Json;
using GoodHamburger.Shared.Models;
using GoodHamburger.Shared.Constants;
using GoodHamburger.Shared.DTOs.Request.Order;
using GoodHamburger.Shared.DTOs.Response.Order;
namespace GoodHamburger.Web.Services;

public class OrderApiService
{
    private readonly HttpClient _http;

    public OrderApiService(HttpClient http)
    {
        _http = http;
    }

    #region Create
    public async Task<ApiResponse<OrderResponseDTO>?> CreateAsync(CreateOrderRequestDTO request)
    {
        var response = await _http.PostAsJsonAsync(ApiRoutes.Orders, request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<OrderResponseDTO>>();
    }
    #endregion

    #region Get
    public async Task<IEnumerable<OrderResponseDTO>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<ApiResponse<IEnumerable<OrderResponseDTO>>>(ApiRoutes.Orders);
        return result?.Data ?? Enumerable.Empty<OrderResponseDTO>();
    }
    #endregion


    #region Delete
    public async Task DeleteAsync(Guid id)
    {
        await _http.DeleteAsync($"{ApiRoutes.Orders}/{id}");
    }
    #endregion
}