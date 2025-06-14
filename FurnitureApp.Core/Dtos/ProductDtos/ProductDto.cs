using FurnitureApp.Core.Dtos.ReviewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.ProductDtos
{
    public class ProductDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? ReviewsCount { get; set; }
        public double? reveiewAverageRating { get; set; }
        public ICollection<ReviewsDto>? Reviews { get; set; }
    }
}
