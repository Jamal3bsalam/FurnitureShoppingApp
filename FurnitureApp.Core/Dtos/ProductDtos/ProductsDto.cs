using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.ProductDtos
{
    public class ProductsDto
    {
        public int? Id { get; set; }
        public string? ImageUrl { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }
}
