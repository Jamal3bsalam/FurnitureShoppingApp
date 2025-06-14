using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Services.Contract.Orders
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail,string userId, int deliverMethod , ShippingAddresses shippingAddress);
        Task<IEnumerable<Order>> GetAllOrdersForUserAsnc(string buyerEmail);
        Task<Order>GetOrderByIdForUserAsnc(string buyerEmail,int orderId);
        Task<IEnumerable<Order>> GetAllOrdersForSpecificOrderStatus(OrderStatus orderSatuts,string buyerEmail);
        Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethodrAsnc();


    }
}
