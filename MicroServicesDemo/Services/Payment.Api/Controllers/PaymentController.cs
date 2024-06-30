using Microsoft.AspNetCore.Mvc;

namespace Payment.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaymentController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Success", "Failed", "InProcess"
        };

        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Payment> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Payment
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Amount = Random.Shared.Next(-20, 55),
                Status = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
