using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.CartDtos
{
    public class CartItemDto
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }   // optional for display
        public string? ProductImageUrl { get; set; } // optional for display
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? SubTotal { get; set; }
    }
}
