using FurnitureApp.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Specification.Products
{
    public class ProductWithCountSpec: BaseSpecification<Product, int>
    {
        public ProductWithCountSpec(ProductSpecParams productSpecParams) : base(
           P => (string.IsNullOrEmpty(productSpecParams.Search) || P.Name.ToLower().Contains(productSpecParams.Search))
                 && (!productSpecParams.CategoryId.HasValue || productSpecParams.CategoryId == P.CategoryId))
        {

        }
    }
}
