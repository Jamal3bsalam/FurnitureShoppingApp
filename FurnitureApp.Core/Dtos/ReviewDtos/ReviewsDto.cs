using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.ReviewDtos
{
    public class ReviewsDto
    {
        public int? Id { get; set; }
        public int? Rating { get; set; } // 1 - 5 scale
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? UserName { get; set; }
        public string? UserImageUrl { get; set; }
    }
}
