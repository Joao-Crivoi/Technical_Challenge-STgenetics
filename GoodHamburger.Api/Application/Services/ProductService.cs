using AutoMapper;
using GoodHamburger.Api.Application.Interfaces;
using GoodHamburger.Api.Application.DTOs.Response.Product;

namespace GoodHamburger.Api.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    #region Get
    public async Task<IEnumerable<ProductResponseDTO>> GetMenuAsync()
    {
        var products = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductResponseDTO>>(products);
    }
    
    #endregion
}