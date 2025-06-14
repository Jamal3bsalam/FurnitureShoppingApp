using FurnitureApp.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Specification.Review
{
    public class ReviewSpecification : BaseSpecification<Reviews,int>
    {
        public ReviewSpecification(int productId) : base(r => r.ProductId == productId)
        {
            ApplyInclude();
        }

        public ReviewSpecification(int reviewId , bool isReviewId) : base(r => r.Id == reviewId)
        {
            ApplyInclude();
        }

        public ReviewSpecification(string userId) : base(r => r.UserId == userId)   
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(r => r.User);
            Include.Add(r => r.Product);
        }
    }
}
