using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Entities.Products
{
    public class Product:BaseEntity<int>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? Price { get; set; }
        public int? QuantityInStock { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Reviews>? Reviews { get; set; } = new List<Reviews>();
    }
}
