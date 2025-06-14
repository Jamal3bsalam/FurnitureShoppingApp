using FurnitureApp.Core;
using FurnitureApp.Core.Dtos.ReviewDtos;
using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Entities.Products;
using FurnitureApp.Core.Services.Contract.Product;
using FurnitureApp.Core.Services.Contract.Review;
using FurnitureApp.Core.Specification.Products;
using FurnitureApp.Core.Specification.Review;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Service.Services.ReviewsService
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductsService _productsService;

        public ReviewService(IUnitOfWork unitOfWork ,IConfiguration configuration , IHttpContextAccessor httpContext,UserManager<AppUser> userManager,IProductsService productsService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContext = httpContext;
            _userManager = userManager;
            _productsService = productsService;
        }
        public async Task<ReviewsDto> AddReviewAsync(AddReviewDto addreviewDto)
        {
            var userId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return null;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return null;

            if (addreviewDto.ProductId == 0 || addreviewDto.ProductId is null) return null;
            var product = await _productsService.GetProductById(addreviewDto.ProductId.Value);
            if (product == null) return null;

            var review = new Reviews()
            {
                Rating = addreviewDto.Rating,
                Comment = addreviewDto.Comment,
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                ProductId = addreviewDto.ProductId,
            };

            await _unitOfWork.Repository<Reviews,int>().AddAsync(review);
            await _unitOfWork.CompleteAsync();

            var reviewDto = new ReviewsDto()
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UserName = user.UserName,
                UserImageUrl = _configuration["BASEURL"] + user.ProfileImage,
            };

            return reviewDto;
        }

        public async Task<ReviewsDto> UpdateReviewAsync(int reviewId, UpdateReviewDto updateReview, string userId)
        {

            if (userId == null) return null;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return null;

            var reviewSpec = new ReviewSpecification(reviewId,true);
            var review = await _unitOfWork.Repository<Reviews, int>().GetWithSpecAsync(reviewSpec);
            if (review == null) { return null; }

            if (review.UserId != userId) { return null; }

            review.Comment = updateReview.Comment;
            review.Rating = updateReview.Rating;
            review.CreatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Reviews, int>().Update(review);
            var result = await _unitOfWork.CompleteAsync();
            if (result == 0) { return null; }

            var reviewDto = new ReviewsDto()
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UserName = user.UserName,
                UserImageUrl = _configuration["BASEURL"] + user.ProfileImage,
            };
            return reviewDto ;

        }

        public async Task<string> DeleteReviewAsync(int reviewId , string userId)
        {
            var reviewSpec = new ReviewSpecification(reviewId , true);
            var review = await _unitOfWork.Repository<Reviews, int>().GetWithSpecAsync(reviewSpec);
            if (review == null) { return "Review Not found"; }

            if (review.UserId != userId) { return "You can only Delete your own reviews."; }
            _unitOfWork.Repository<Reviews, int>().Delete(review);
            var result = await _unitOfWork.CompleteAsync();
            if (result == 0 || result == null) { return null; }
            return "Review Deleted Successfully";
        }

        public async Task<ProductReviewsDto> GetAllReviewsForProductAsync(int productId)
        {
            var reviewSpec = new ReviewSpecification(productId);
            var productSpec = new ProductSpecification(productId);
            var reviews = await _unitOfWork.Repository<Reviews,int>().GetAllWithSpecAsync(reviewSpec);
            var products = await _unitOfWork.Repository<Product, int>().GetWithSpecAsync(productSpec);

            if (reviews == null || products == null) return null;

            var reviewList = reviews.Select(r => new ReviewsDto
            {
                Id = r.Id,
                UserImageUrl = _configuration["BASEURL"] + r.User.ProfileImage.Replace("\\", "/"),
                UserName = r.User.FullName,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();

            var productReviewsDto = new ProductReviewsDto()
            {
                ProductImageUrl = _configuration["BASEURL"] + products.ImageUrl,
                AverageRate = AverageRating(reviews),
                ProductName = products.Name,
                ReviewsCount = reviews.Count(),

                // user
                Reviews = reviewList
                //UserImageUrl = _configuration["BASEURL"] + r.User.ProfileImage,
                //UserName = r.User.FullName,
                //Rate = r.Rating,
                //Comment = r.Comment,
                //DateOfCreation = r.CreatedAt



            };

            return productReviewsDto;
            
        }

        public async Task<IEnumerable<UsersReviewsDto>> GetAllReviewsForUserAsync(string userId)
        {
            var reviewSpec = new ReviewSpecification(userId);
            var reviews = await _unitOfWork.Repository<Reviews, int>().GetAllWithSpecAsync(reviewSpec);
            if (reviews == null) return null;

           var userReviews = reviews.Select(r => new UsersReviewsDto()
            {
               Name = r.Product.Name,   
               ProductImageUrl = _configuration["BASEURL"] + r.Product.ImageUrl.Replace("\\", "/"),
               Price = r.Product.Price,
               Rate = r.Rating,
               Comment = r.Comment,
               DateOfCreation = r.CreatedAt,
            });
            return userReviews;
        }

        private double AverageRating(IEnumerable<Reviews> reviews)
        {
            if (reviews == null || !reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating ?? 0);
        }
    }
}
