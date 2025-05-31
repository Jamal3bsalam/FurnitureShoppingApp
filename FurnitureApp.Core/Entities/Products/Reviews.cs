using FurnitureApp.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Entities.Products
{
    public class Reviews : BaseEntity<int>
    {
        public int? Rating { get; set; } // 1 - 5 scale
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
