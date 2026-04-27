using GoodHamburger.Api.Domain.Entities;

namespace GoodHamburger.Api.Application.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task UpdateAsync(Order order);
    Task DeleteAsync(Order order);
}