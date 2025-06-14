using FurnitureApp.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Entities.Products
{
    public class Cart : BaseEntity<int>
    {
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public List<CartItem>? Items { get; set; } = new List<CartItem>();
        public int? DeliveryMethodId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set;}
    }
}
