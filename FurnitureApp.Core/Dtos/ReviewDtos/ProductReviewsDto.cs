using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.ReviewDtos
{
    public class ProductReviewsDto
    {
        public string? ProductImageUrl { get; set; }
        public double? AverageRate { get; set; }
        public string? ProductName { get; set; }
        public int? ReviewsCount { get; set; }

        // user
        public List<ReviewsDto>? Reviews { get; set; }
    }
}
