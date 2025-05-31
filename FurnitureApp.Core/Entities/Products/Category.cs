using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Entities.Products
{
    public class Category:BaseEntity<int>
    {
        public string? Type { get; set; }
        public ICollection<Product>? Products { get; set; } = new List<Product>();
    }
}
