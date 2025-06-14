using FurnitureApp.Core;
using FurnitureApp.Core.Dtos.CartDtos;
using FurnitureApp.Core.Entities.Products;
using FurnitureApp.Core.Repositories.Contract;
using FurnitureApp.Core.Services.Contract.Carts;
using FurnitureApp.Core.Services.Contract.Product;
using FurnitureApp.Core.Specification.Products;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Service.Services.CartsService
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductsService _productsService;

        public CartService(ICartRepository cartRepository,IConfiguration configuration,IUnitOfWork unitOfWork, IProductsService productsService)
        {
            _cartRepository = cartRepository;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _productsService = productsService;
        }
        public async Task<CartDto> GetCartWithUserId(string userId)
        {
            var cart = await _cartRepository.GetCartAsync(userId);
            if(cart == null) return new CartDto();

            var cartDto = new CartDto()
            {
                Id = cart.Id,
                Items = cart.Items.Select(I => new CartItemDto()
                {
                    Id = I.Id,
                    ProductId = I.ProductId,
                    ProductName = I.Product.Name,
                    ProductImageUrl = _configuration["BASEURL"] + I.Product.ImageUrl,
                    Price = I.Product.Price,
                    Quantity = I.Product.QuantityInStock,
                    SubTotal = I.Product.Price * I.Product.QuantityInStock
                }).ToList(),
                Total = GetCartTotal(cart.Items)
                
            };
            return cartDto;
        }

        public async Task<bool> AddItemToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await _cartRepository.GetCartAsync(userId);
            var productSpec = new ProductSpecification(productId);
            var product = await _unitOfWork.Repository<Product,int>().GetWithSpecAsync(productSpec);

            product.QuantityInStock = quantity;
            await _unitOfWork.CompleteAsync();

            if (product == null) return false;

            if (cart == null)
            {
                cart = new Cart { UserId = userId, Items = new List<CartItem>() };
                await _unitOfWork.Repository<Cart, int>().AddAsync(cart);
            }

            if (!cart.Items.Any(i => i.ProductId == productId))
            {
                cart.Items.Add(new CartItem { ProductId = productId , Product = product});
                await _unitOfWork.CompleteAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveItemFromCartAsync(string userId, int productId)
        {
            var cart = await _cartRepository.GetCartAsync(userId);
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(I => I.ProductId == productId);
                if (item != null)
                {
                    cart.Items.Remove(item);
                    await _unitOfWork.CompleteAsync();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartAsync(userId);
            if (cart != null && cart.Items.Any())
            {
                cart.Items.Clear();
                await _unitOfWork.CompleteAsync();
                return true;
            }

            return false;
        }

        private decimal GetCartTotal(List<CartItem> items)
        {
            return (decimal)items.Sum(I => I.Product.Price * I.Product.QuantityInStock);
        }

        
    }
}
