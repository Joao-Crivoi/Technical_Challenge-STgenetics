using AutoMapper;
using GoodHamburger.Api.Web.Constants;
using GoodHamburger.Api.Domain.Entities;
using GoodHamburger.Api.Application.Interfaces;
using GoodHamburger.Api.Application.DTOs.Request.Order;
using GoodHamburger.Api.Application.DTOs.Response.Order;


namespace GoodHamburger.Api.Application.Services;

public class OrderService : IOrderService 
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IEnumerable<IDiscountStrategy> _strategies;


    public OrderService(
        IMapper mapper,
        IOrderRepository orderRepository, 
        IProductRepository productRepository,
        IEnumerable<IDiscountStrategy> strategies)
    {
         _mapper = mapper;
         _strategies = strategies;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        
    }

    #region Create
    public async Task<OrderResponseDTO> CreateOrderAsync(CreateOrderRequestDTO request)
    {
        var order = new Order();

        foreach (var id in request.ProductIds)
        {
            var product = await _productRepository.GetByIdAsync(id) 
                ?? throw new KeyNotFoundException($"Product {id} not found.");
            
            order.AddItem(product);
        }

        ApplyDiscountStrategies(order);

        await _orderRepository.AddAsync(order);
        
        return _mapper.Map<OrderResponseDTO>(order);
    }

    #endregion

    #region Get
    public async Task<OrderResponseDTO?> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order == null ? null : _mapper.Map<OrderResponseDTO>(order);
    }

    public async Task<IEnumerable<OrderResponseDTO>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
    }

    #endregion

    #region Update  
    public async Task<OrderResponseDTO> UpdateOrderAsync(Guid id, CreateOrderRequestDTO request)
    {
        var order = await _orderRepository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException(Messages.OrderNotFound);

        
        order.ClearItems();

    
        foreach (var productId in request.ProductIds)
        {
            var product = await _productRepository.GetByIdAsync(productId) 
                ?? throw new KeyNotFoundException(string.Format(Messages.ProductNotFound, productId));
            
            order.AddItem(product);
        }

        
        ApplyDiscountStrategies(order);

        await _orderRepository.UpdateAsync(order);
        return _mapper.Map<OrderResponseDTO>(order);
    }

    #endregion

    #region Delete
    public async Task DeleteOrderAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException(Messages.OrderNotFound);

        await _orderRepository.DeleteAsync(order);
    }
    #endregion

    #region Private Methods
    private void ApplyDiscountStrategies(Order order)
    {
        var strategy = _strategies
            .OrderByDescending(s => s.Calculate(100))
            .FirstOrDefault(s => s.CanApply(order.Items));

        if (strategy != null)
        {
            var discount = strategy.Calculate(order.Subtotal);
            order.ApplyDiscount(discount);
        }
    }
    
    #endregion
}
        