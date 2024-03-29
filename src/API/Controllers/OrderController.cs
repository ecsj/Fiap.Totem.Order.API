using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace API.Controllers;

[ExcludeFromCodeCoverage]
[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderUseCase _orderUseCase;

    public OrderController(IOrderUseCase orderUseCase)
    {
        _orderUseCase = orderUseCase;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        var orders = _orderUseCase.Get();

        return Ok(orders);
    }
    [HttpGet("GetOrdersByStatus/{status}")]
    public ActionResult<IEnumerable<Order>> GetOrdersByStatus(OrderStatus status)
    {
        var orders = _orderUseCase.GetOrdersByStatus(status);

        return Ok(orders);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _orderUseCase.GetById(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpGet("{orderCode:int}")]
    public async Task<IActionResult> GetByOrderCode(int orderCode)
    {
        var order = await _orderUseCase.GetByOrderCode(orderCode);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpGet("GetOrdersByCustomer/{id:Guid}")]
    public async Task<IActionResult> GetOrdersByCustomer(Guid id)
    {
        var order = await _orderUseCase.GetOrdersByCustomer(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpGet("GetByCustomerCpf/{cpf}")]
    public async Task<IActionResult> GetByCustomerCpf(string cpf)
    {
        var orders = await _orderUseCase.GetOrdersByCpf(cpf);

        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest request)
    {
        var order = await _orderUseCase.PlaceOrder(request);

        return CreatedAtAction(nameof(GetByOrderCode), new { orderCode = order.OrderCode }, order);
    }
    
    [HttpPut("UpdateOrderStatus/{id:Guid}")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderStatus newStatus)
    {
        await _orderUseCase.UpdateOrderStatus(id, newStatus);

        return NoContent();
    }
    
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        await _orderUseCase.CancelOrder(id);

        return NoContent();
    }
}