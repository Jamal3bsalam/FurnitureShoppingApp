using FurnitureApp.Core.Dtos.CartDtos;
using FurnitureApp.Core.Entities.Products;
using FurnitureApp.Core.Services.Contract.Payment;
using FurnitureApp.PL.Response.Error;
using FurnitureApp.PL.Response.GeneralResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureApp.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("cart/{Id}")]
        public async Task<ActionResult<ApiResponse<CartDto>>> CreatePaymentIntent(int Id)
        {
            if (Id == null) return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest));
            var paymentCart = await _paymentService.CreateOrUpdatePaymentIntentId(Id);
            if (paymentCart == null) return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(new ApiResponse<CartDto>(true,200, "Payment created successfully.", paymentCart));
        }
    }
}
