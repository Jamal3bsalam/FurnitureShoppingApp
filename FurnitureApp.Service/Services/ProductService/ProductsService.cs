using AutoMapper;
using FurnitureApp.Core;
using FurnitureApp.Core.Dtos.ProductDtos;
using FurnitureApp.Core.Dtos.ReviewDtos;
using FurnitureApp.Core.Entities.Products;
using FurnitureApp.Core.Services.Contract.Product;
using FurnitureApp.Core.Specification.Products;
using FurnitureApp.Core.Specification.Review;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Service.Services.ProductService
{
    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProductsService(IUnitOfWork unitOfWork,IMapper mapper , IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<ProductsDto>> GetAllProductsAsync(ProductSpecParams productSpec)
        {
            var spec = new ProductSpecification(productSpec);
            var products = await _unitOfWork.Repository<Product,int>().GetAllWithSpecAsync(spec);
            if (products == null) return null;

            var productsDto = products.Select(P => new ProductsDto()
            {
                Id = P.Id,
                ImageUrl = _configuration["BASEURL"] + P.ImageUrl,
                Name = P.Name,
                Price = P.Price,    
            }).ToList();

            return productsDto;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var spec = new ProductSpecification(id);
            var reviewSpec = new ReviewSpecification(id);
            var product = await _unitOfWork.Repository<Product,int>().GetWithSpecAsync(spec);
            var reviews = await _unitOfWork.Repository<Reviews, int>().GetAllWithSpecAsync(reviewSpec);
            if (reviews == null) return null;
            if (product == null) return null;
            var productDto = new ProductDto()
            {
                Id = product.Id,
                ImageUrl = _configuration["BASEURL"] + product.ImageUrl,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ReviewsCount = product.Reviews?.Count(),
                Reviews = reviews.Select(R => new ReviewsDto()
                {
                    Id = R.Id,
                    Comment = R.Comment,
                    Rating = R.Rating,
                    UserImageUrl = _configuration["BASEURL"] + R.User?.ProfileImage,
                    UserName = R.User?.FullName

                }).ToList(),
                reveiewAverageRating = AverageRating(product.Reviews),
            };

            return productDto;  
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Repository<Category, int>().GetAllAsync();
            if (categories == null) return null;
            var categoriesDto = categories.Select(C => new CategoryDto()
            {
                Id = C.Id,
                Type = C.Type,
            });
            return categoriesDto;
        }

        public async Task<IEnumerable<ProductsDto>> GetAllProductForSpecificCategory(string categoryType)
        {
            var spec = new ProductSpecification(categoryType);
            var productForCategory = await _unitOfWork.Repository<Product,int>().GetAllWithSpecAsync(spec);
            if (productForCategory == null) return null;

            var productsDto = productForCategory.Select(P => new ProductsDto()
            {
                Id = P.Id,
                ImageUrl = _configuration["BASEURL"] + P.ImageUrl,
                Name = P.Name,
                Price = P.Price,
            }).ToList();

            return productsDto;

        }

        private double AverageRating(IEnumerable<Reviews> reviews)
        {
            if (reviews == null || !reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating ?? 0);
        }

    }
}
