using FurnitureApp.Core.Dtos.ProductDtos;
using FurnitureApp.Core.Enums;
using FurnitureApp.Core.Services.Contract.Product;
using FurnitureApp.Core.Specification.Products;
using FurnitureApp.PL.Response.Error;
using FurnitureApp.PL.Response.GeneralResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureApp.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet("allProducts")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductsDto>>>> GetAllProducts([FromQuery]ProductSpecParams productSpec)
        {
            if (productSpec.pageIndex == 0) return BadRequest(new ErrorResponse(400, "Page Index Can not be Zero Indexed"));
            var products = await _productsService.GetAllProductsAsync(productSpec);
            if (products == null) return BadRequest(new ErrorResponse(400, "No products available."));

            return Ok(new ApiResponse<IEnumerable<ProductsDto>>(true,200, "Products retrieved successfully.", products));
        }

        [HttpGet("id")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(int id)
        {
            var product = await _productsService.GetProductById(id);
            if (product == null) return BadRequest(new ErrorResponse(400, "No Product Available With This Id"));
            return Ok(new ApiResponse<ProductDto>(true,200, "Product retrieved successfully.",product));
        }

        [HttpGet("allCategories")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetAllCategories()
        {
            var categories = await _productsService.GetAllCategoriesAsync();
            if (categories == null) return BadRequest(new ErrorResponse(400));
            return Ok(new ApiResponse<IEnumerable<CategoryDto>>(true,200, "Categories retrieved successfully.",categories));
        }

        [HttpGet("allProductsForCategry")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductsDto>>>> GetAllProductsForSpecificCategory(Categories categories)
        {
            var products = await _productsService.GetAllProductForSpecificCategory(categories.ToString());
            if (products == null) return BadRequest(new ErrorResponse(400, "No products available."));

            return Ok(new ApiResponse<IEnumerable<ProductsDto>>(true, 200, "Products retrieved successfully.", products));
        }

    }
}
