using APIs.Exceptions;
using APIs.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private static readonly List<OrderResponseModels> _inMemoryStore = new();
        private readonly IValidator<CreateOrderRequest> _validator;

        // Dependency injection resolves the validator registered by FluentValidation.DependencyInjectionExtensions
        public OrdersController(IValidator<CreateOrderRequest> validator)
        {
            _validator = validator;
        }

        // 1. Tests [FromBody], [FromHeader], 400 BadRequest, 409 Conflict, and 201 Created
        [HttpPost]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderRequest request,
            [FromHeader(Name = "X-Client-Trace-Id")] string? traceId)
        {
            // Explicit async validation execution
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                // Returns standard RFC 7807 validation problem response with 400 Bad Request
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            // Simulating a 409 Conflict scenario
            var exists = _inMemoryStore.Any(o => o.CustomerEmail == request.CustomerEmail && o.ItemName == request.ItemName);
            if (exists)
            {
                throw new DuplicateOrderException($"Order for item '{request.ItemName}' by '{request.CustomerEmail}' already exists.");
            }

            var order = new OrderResponseModels(
                Id: Guid.NewGuid(),
                CustomerEmail: request.CustomerEmail,
                ItemName: request.ItemName,
                Quantity: request.Quantity,
                TotalPrice: request.Quantity * request.UnitPrice,
                CreatedAt: DateTime.UtcNow
            );

            _inMemoryStore.Add(order);

            // 201 Created with Location header referencing the GetById route
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        // 2. Tests [FromRoute], 200 OK, and 404 NotFound
        [HttpGet("{id:guid}")]
        public IActionResult GetOrderById([FromRoute] Guid id)
        {
            var order = _inMemoryStore.FirstOrDefault(o => o.Id == id);
            if (order is null)
            {
                throw new OrderNotFoundException($"No order found matching ID '{id}'.");
            }

            return Ok(order);
        }

        // 3. Tests [FromQuery] pagination and filtering
        [HttpGet]
        public IActionResult GetOrders(
            [FromQuery] string? searchEmail,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            var query = _inMemoryStore.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchEmail))
            {
                query = query.Where(o => o.CustomerEmail.Contains(searchEmail, StringComparison.OrdinalIgnoreCase));
            }

            var results = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(results);
        }

        // 4. Tests 204 NoContent
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteOrder([FromRoute] Guid id)
        {
            var order = _inMemoryStore.FirstOrDefault(o => o.Id == id);
            if (order is null)
            {
                throw new OrderNotFoundException($"Cannot delete: order '{id}' does not exist.");
            }

            _inMemoryStore.Remove(order);

            // 204 No Content indicates success with an empty body
            return NoContent();
        }
    }
}
