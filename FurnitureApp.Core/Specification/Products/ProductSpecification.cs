using FurnitureApp.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Specification.Products
{
    public class ProductSpecification:BaseSpecification<Product,int>
    {
        public ProductSpecification(int id) : base(P => P.Id == id)
        {
            ApplyInclude();   
        }

        public ProductSpecification(string category):base(P => P.Category.Type == category)
        {
            
        }

        public ProductSpecification(ProductSpecParams productSpecParams):base(
            P => (string.IsNullOrEmpty(productSpecParams.Search) || P.Name.ToLower().Contains(productSpecParams.Search))
                  && (!productSpecParams.CategoryId.HasValue || productSpecParams.CategoryId == P.CategoryId))
        {
            if (!string.IsNullOrEmpty(productSpecParams.sort))
            {
                switch (productSpecParams.sort)
                {
                    case "priceAsc":
                        AddOrederBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrederByDesc(P => P.Price);
                        break;
                    default:
                        AddOrederBy(P => P.Name);
                        break;
                }
            }
            else
            {
                OrderBy = P => P.Name;
            }
            ApplyInclude();

            if (productSpecParams.pageIndex.HasValue && productSpecParams.pageSize.HasValue)
            {
                ApplyPagination(productSpecParams.pageSize.Value * (productSpecParams.pageIndex.Value - 1), productSpecParams.pageSize.Value);
            }
        }








        private void ApplyInclude()
        {
            Include.Add(P => P.Category);
            Include.Add(P => P.Reviews);
        }
    }
}
