using Microsoft.AspNetCore.Mvc;
using GoodHamburger.Api.Application.Interfaces;
using GoodHamburger.Api.Application.DTOs.Response.Product;
using GoodHamburger.Shared.Models;

namespace GoodHamburger.Api.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet] 
    public async Task<IActionResult> GetMenu()
    {
        var menu = await _productService.GetMenuAsync();
        return Ok(ApiResponse<IEnumerable<ProductResponseDTO>>.Ok(menu));
    }
}