using GoodHamburger.Api.Application.Interfaces;
using GoodHamburger.Api.Domain.Entities;
using GoodHamburger.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Api.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    #region Get
    public async Task<Order?> GetByIdAsync(Guid id) => 
        await _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product).FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Order>> GetAllAsync() => 
        await _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product).ToListAsync();
    
    #endregion

    #region Update
    
    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    #endregion

    #region Delete
    
    public async Task DeleteAsync(Order order)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    #endregion
}