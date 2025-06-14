using FurnitureApp.Core.Dtos.CartDtos;
using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Services.Contract.Carts;
using FurnitureApp.PL.Response.Error;
using FurnitureApp.PL.Response.GeneralResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureApp.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly UserManager<AppUser> _userManager;

        public CartController(ICartService cartService , UserManager<AppUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<CartDto>>> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ErrorResponse(404));
            var cart = await _cartService.GetCartWithUserId(userId);
            return Ok(new ApiResponse<CartDto>(true,200, "Cart retrieved successfully.", cart));
        }
        [HttpPost("{productId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<bool>>> AddToCart(int productId , int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound(new ErrorResponse(404));
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ErrorResponse(404));
            var result = await _cartService.AddItemToCartAsync(userId, productId ,quantity);
            return Ok(new ApiResponse<bool>(true,200,"Product Add Successfully To Cart.",result));
        }

        [HttpDelete("{productId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveFromWishlist(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound(new ErrorResponse(404));
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ErrorResponse(404));
            var result = await _cartService.RemoveItemFromCartAsync(userId, productId);
            return Ok(new ApiResponse<bool>(true,200, "Product Removed successfully",result));
        }

        [HttpDelete("clear")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ClearWishlist()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound(new ErrorResponse(404));
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ErrorResponse(404));
            var result = await _cartService.ClearCartAsync(userId);
            return Ok(new ApiResponse<bool>(true, 200, "Cart Cleared successfully", result));
        }
    }
}
