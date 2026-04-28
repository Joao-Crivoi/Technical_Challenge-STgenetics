namespace GoodHamburger.Api.Application.DTOs.Request.Order;

public class CreateOrderRequestDTO
{
    public List<int> ProductIds { get; set; } = new();
}