using GoodHamburger.Api.Application.Interfaces;
using GoodHamburger.Api.Domain.Entities;
using GoodHamburger.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Api.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id) => 
        await _context.Products.FindAsync(id);

    public async Task<IEnumerable<Product>> GetAllAsync() => 
        await _context.Products.ToListAsync();
}