using GoodHamburger.Api.Domain.Entities;

namespace GoodHamburger.Api.Application.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
}