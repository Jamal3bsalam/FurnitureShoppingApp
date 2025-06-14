using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.ReviewDtos
{
    public class UsersReviewsDto
    {
        public string? Name { get; set; }
        public string? ProductImageUrl { get; set; }
        public decimal? Price { get; set; }
        public int? Rate { get; set; }
        public string? Comment { get; set; }
        public DateTime? DateOfCreation { get; set; }
    }
}
