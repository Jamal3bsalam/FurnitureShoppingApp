using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.CartDtos
{
    public class CartDto
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public List<CartItemDto>? Items { get; set; }
    }
}
