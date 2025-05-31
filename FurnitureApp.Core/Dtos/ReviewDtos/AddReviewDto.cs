using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.ReviewDtos
{
    public class AddReviewDto
    {
        public int? Rating { get; set; } // 1 - 5 scale
        public string? Comment { get; set; }
        public int? ProductId { get; set; }
    }
}
