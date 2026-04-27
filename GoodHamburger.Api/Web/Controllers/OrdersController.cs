using Microsoft.AspNetCore.Mvc;
using GoodHamburger.Api.Web.Helpers;
using GoodHamburger.Api.Web.Constants;
using GoodHamburger.Api.Application.Interfaces;
using GoodHamburger.Api.Application.DTOs.Response.Order;
using GoodHamburger.Api.Application.DTOs.Request.Order;

namespace GoodHamburger.Api.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    #region Get
      [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
            return NotFound(ApiResponse<string>.Error(Messages.OrderNotFound));

        return Ok(ApiResponse<OrderResponseDTO>.Ok(order));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<OrderResponseDTO>>.Ok(orders));
    }

#endregion

    #region POST  

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequestDTO request)
    {
        var result = await _orderService.CreateOrderAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<OrderResponseDTO>.Ok(result));
    }

#endregion

    #region PUT
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateOrderRequestDTO request)
    {
        var result = await _orderService.UpdateOrderAsync(id, request);
        return Ok(ApiResponse<OrderResponseDTO>.Ok(result));
    }
    #endregion

    #region Delete

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _orderService.DeleteOrderAsync(id);
        return NoContent(); // 204 No Content é o padrão para deleção com sucesso
    }

    #endregion
}