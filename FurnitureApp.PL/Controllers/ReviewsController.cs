using FurnitureApp.Core.Dtos.ReviewDtos;
using FurnitureApp.Core.Services.Contract.Product;
using FurnitureApp.Core.Services.Contract.Review;
using FurnitureApp.PL.Response.Error;
using FurnitureApp.PL.Response.GeneralResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureApp.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IProductsService _productsService;

        public ReviewsController(IReviewService reviewService,IProductsService productsService)
        {
            _reviewService = reviewService;
            _productsService = productsService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<ReviewsDto>>> AddReview([FromBody] AddReviewDto dto)
        {
            if (dto.Rating == null && dto.Comment == null) BadRequest(new ErrorResponse(400,"Please Enter A Valid Review"));
            var result = await _reviewService.AddReviewAsync(dto);
            if (result == null) BadRequest(new ErrorResponse(400));
            return Ok(new ApiResponse<ReviewsDto>(true,200,"Review Added Succssefully",result));
        }

        [HttpPut("{reviewId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<ReviewsDto>>> UpdateReview(int reviewId, [FromBody] UpdateReviewDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            if (userId == null) return Unauthorized(new ErrorResponse(401, "User is not authenticated."));

            var result = await _reviewService.UpdateReviewAsync(reviewId, dto, userId);
            if (result == null) return BadRequest(new ErrorResponse(400, "Failed to update review."));

            return Ok(new ApiResponse<ReviewsDto>(true,200,"Review Updated Succssefully",result));

        }

        [HttpDelete("{reviewId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteReview(int reviewId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            if (userId == null) return Unauthorized(new ErrorResponse(401, "User is not authenticated."));

            var result = await _reviewService.DeleteReviewAsync(reviewId, userId);
            if (result == null) return BadRequest(new ErrorResponse(400, "Failed to Delete review."));

            return Ok(new ApiResponse<string>(true,200,"",result));

        }
        [HttpGet("{productId}/reviews")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ProductReviewsDto>> GetAllReviewsByBookID(int productId)
        {
            var product = await _productsService.GetProductById(productId);
            if (product == null) return NotFound(new ErrorResponse(404));
            var reviews = await _reviewService.GetAllReviewsForProductAsync(productId);
            if (reviews == null) return BadRequest(new ErrorResponse(400, "The Book Has No Reviews"));
            return Ok(new ApiResponse<ProductReviewsDto>(true,200,"Product Reviews Retrived Successfully",reviews));
        }

        [HttpGet("my-reviews")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<UsersReviewsDto>>> GetAllReviewsByBookID()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound("User Not Found");

            var reviews = await _reviewService.GetAllReviewsForUserAsync(userId);
            if (reviews == null) return NotFound(new ErrorResponse(404));
            if (reviews == null) return BadRequest(new ErrorResponse(400, "User Has No Reviews"));
            return Ok(new ApiResponse<IEnumerable<UsersReviewsDto>>(true, 200, "Product Reviews Retrived Successfully", reviews));
        }
    }
}
