using FurnitureApp.Core;
using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Entities.Orders;
using FurnitureApp.Core.Entities.Products;
using FurnitureApp.Core.Services.Contract.Carts;
using FurnitureApp.Core.Services.Contract.Orders;
using FurnitureApp.Core.Services.Contract.Payment;
using FurnitureApp.Core.Specification.Orders;
using FurnitureApp.Core.Specification.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Service.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork,ICartService cartService,IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _paymentService = paymentService;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string userId, int deliverMethodId, ShippingAddresses shippingAddress)
        {
            var cart = await _cartService.GetCartWithUserId(userId);
            if (cart is null) return null;

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(deliverMethodId);

            var orderItems = new List<OrderItem>();


            foreach (var item in cart.Items)
            {
                var spec = new ProductSpecification((int)item.ProductId);
                var product = await _unitOfWork.Repository<Product, int>().GetWithSpecAsync(spec);
                var productOrderItem = new ProductItemOrder(product.Id, product.Name, product.ImageUrl);
                var orderItem = new OrderItem(productOrderItem, product.Price, item.Quantity);

                orderItems.Add(orderItem);
            }

            var subTotal = orderItems.Sum(I => I.Price * I.Quantity);


            //TO DO
            if (!string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                 var specPayment = new OrderSpecification(cart.PaymentIntentId);
                var exOrder = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(specPayment);

                _unitOfWork.Repository<Order, int>().Delete(exOrder);

            };

            var cartPayment =  await _paymentService.CreateOrUpdatePaymentIntentId(cart.Id);

            var order = new Order(buyerEmail,shippingAddress, deliveryMethod,orderItems,subTotal,cartPayment.PaymentIntentId);

            await _unitOfWork.Repository<Order,int>().AddAsync(order);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;

            return order;

        }

        public async Task<Order?> GetOrderByIdForUserAsnc(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecification(buyerEmail,orderId);
            var order = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);

            if (order is null) return null;
            return order;

        }

        public async Task<IEnumerable<Order>> GetAllOrdersForUserAsnc(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);

            if (orders is null) return null;
            return orders;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersForSpecificOrderStatus(OrderStatus orderSatuts,string buyerEmail)
        {
            var spec = new OrderSpecification(orderSatuts, buyerEmail,true);
            var orders = await _unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);

            if (orders is null) return null;
            return orders;
        }

        public async Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethodrAsnc()
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();
            if (deliveryMethod is null) return null;
            return deliveryMethod;
        }
    }
}
