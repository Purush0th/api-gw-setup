using Microsoft.AspNetCore.Mvc;

namespace Order.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "iPhone", "Nikon D30","Google Pixel", "Amazon Alexa", "Dell Inspiron"
        };

        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<OrderDto> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new OrderDto
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Id = index,
                Items = Enumerable.Range(1, 5).Select(i => new OrderItem
                {
                    Name = Summaries[Random.Shared.Next(1, 5)],
                    Id = i,
                    Quantity = 1,
                    Price = Random.Shared.Next(250, 5000)
                }),
                ShippingId = Guid.NewGuid(),
                TransactionId = Guid.NewGuid()
            })
            .ToArray();
        }
    }
}
