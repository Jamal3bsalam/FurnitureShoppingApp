using FurnitureApp.Core;
using FurnitureApp.Core.Dtos.CartDtos;
using FurnitureApp.Core.Entities.Orders;
using FurnitureApp.Core.Entities.Products;
using FurnitureApp.Core.Repositories.Contract;
using FurnitureApp.Core.Services.Contract.Carts;
using FurnitureApp.Core.Services.Contract.Payment;
using FurnitureApp.Core.Specification.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Product = FurnitureApp.Core.Entities.Products.Product;

namespace FurnitureApp.Service.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;
        private readonly IConfiguration _configuration;

        public PaymentService(ICartService cartService,IHttpContextAccessor httpContext,IUnitOfWork unitOfWork,ICartRepository cartRepository,IConfiguration configuration)
        {
            _cartService = cartService;
            _httpContext = httpContext;
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _configuration = configuration;
        }
        public async Task<CartDto> CreateOrUpdatePaymentIntentId(int CartId)
        {



            var userId = _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return null;
            var cart = await _cartService.GetCartWithUserId(userId);
            if (cart is null) return null;

            var shippingPrice = 0m;
            if (cart.DeliveryMethodId.HasValue)
            {
               var deliveryMethod =  await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(cart.DeliveryMethodId.Value);
                shippingPrice = (decimal)deliveryMethod.Cost;
            }

            if(cart.Items.Count > 0)
            {
                foreach (var item in cart.Items)
                {
                    var spec = new ProductSpecification((int)item.ProductId);
                    var product = await _unitOfWork.Repository<Product, int>().GetWithSpecAsync(spec);
                    if(item.Price != product.Price)
                    {
                        item.Price = product.Price; 
                    }
                }
            }
            var subTotal = cart.Items.Sum(I => I.Price * I.Quantity);


            var service = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                //create
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long?)(subTotal * 100 + shippingPrice * 100),
                    PaymentMethodTypes = new List<string>() {"card"},
                    Currency = "USD"
                };
               paymentIntent = await service.CreateAsync(options);
                cart.PaymentIntentId = paymentIntent.Id;
                cart.ClientSecret = paymentIntent.ClientSecret;

            }
            else
            {
                //Update
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long?)(subTotal * 100 + shippingPrice * 100),
                };
                paymentIntent = await service.UpdateAsync(cart.PaymentIntentId,options);
                cart.PaymentIntentId = paymentIntent.Id;
                cart.ClientSecret = paymentIntent.ClientSecret;
            }
          
            var cartDb = await _cartRepository.GetCartAsync(userId);
            cartDb.PaymentIntentId = cart.PaymentIntentId;
            cartDb.ClientSecret = cart.ClientSecret;    
            _unitOfWork.Repository<Cart, int>().Update(cartDb);
            var cartResult = await _unitOfWork.CompleteAsync();
            if (cartResult <= 0) return null;
            return cart;

        }
    }
}
