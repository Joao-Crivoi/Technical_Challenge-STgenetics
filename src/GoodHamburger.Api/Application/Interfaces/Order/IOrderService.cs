using GoodHamburger.Shared.DTOs.Request.Order;
using GoodHamburger.Shared.DTOs.Response.Order;

namespace GoodHamburger.Api.Application.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDTO> CreateOrderAsync(CreateOrderRequestDTO request);
    Task<OrderResponseDTO?> GetByIdAsync(Guid id);
    Task<IEnumerable<OrderResponseDTO>> GetAllAsync();
    Task<OrderResponseDTO> UpdateOrderAsync(Guid id, CreateOrderRequestDTO request);
    Task DeleteOrderAsync(Guid id);
}