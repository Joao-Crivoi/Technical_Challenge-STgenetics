using GoodHamburger.Shared.DTOs.Response.Product;

namespace GoodHamburger.Api.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDTO>> GetMenuAsync();
}