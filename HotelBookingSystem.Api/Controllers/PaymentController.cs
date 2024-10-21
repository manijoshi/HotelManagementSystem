using HotelBookingSystem.Application.DTO.PaymentDTO;
using HotelBookingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers
{
    namespace HotelBookingSystem.Api.Controllers
    {
        [ApiController]
        [Authorize(Policy = "AdminOrCustomer")]
        [Route("api/[controller]")]
        public class PaymentController : ControllerBase
        {
            private readonly IPaymentService _paymentService;

            public PaymentController(IPaymentService paymentService)
            {
                _paymentService = paymentService;
            }

            [HttpPost("create-payment")]
            public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest paymentRequest)
            {

                var result = await _paymentService.CreatePaymentAsync(paymentRequest);
                return CreatedAtAction(nameof(GetPaymentDetails), new { paymentId = result.PaymentId }, result);

            }

            [HttpGet("{paymentId}")]
            public async Task<IActionResult> GetPaymentDetails(int paymentId)
            {

                var result = await _paymentService.GetPaymentDetailsAsync(paymentId);
                return Ok(result);

            }

            [HttpPatch("{paymentId}/status")]
            public async Task<IActionResult> UpdatePaymentStatus(int paymentId, [FromBody] string status)
            {

                var result = await _paymentService.UpdatePaymentStatusAsync(paymentId, status);
                return Ok(result);

            }
        }
    }
}