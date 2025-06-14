using FurnitureApp.Core.Dtos.ProductDtos;
using FurnitureApp.Core.Specification.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Services.Contract.Product
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductsDto>>GetAllProductsAsync(ProductSpecParams productSpec);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<IEnumerable<ProductsDto>> GetAllProductForSpecificCategory(string categoryType);
        Task<ProductDto> GetProductById(int id);
    }
}
