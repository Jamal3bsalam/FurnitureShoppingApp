using FurnitureApp.Core.Dtos.ReviewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Services.Contract.Review
{
    public interface IReviewService
    {
        Task<ProductReviewsDto> GetAllReviewsForProductAsync(int productId);    
        Task<IEnumerable<UsersReviewsDto>> GetAllReviewsForUserAsync(string userId);    
        Task<ReviewsDto> AddReviewAsync(AddReviewDto review);
        Task<ReviewsDto> UpdateReviewAsync(int reviewId,UpdateReviewDto review,string userId);
        Task<string> DeleteReviewAsync(int reviewId , string userId);
    }
}
