using FurnitureApp.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Specification.Orders
{
    public class OrderSpecification : BaseSpecification<Order,int>
    {
        public OrderSpecification(string buyerEmail,int orderId):base(O => O.BuyerEmail == buyerEmail && O.Id == orderId)
        {
            ApplyInclude();
        }
        public OrderSpecification(string buyerEmail) : base(O => O.BuyerEmail == buyerEmail)
        {
            ApplyInclude();
        }

        public OrderSpecification(OrderStatus orderStatus, string buyerEmail, bool isOrderStatus) : base(O => O.Status.Value == orderStatus && O.BuyerEmail == buyerEmail)
        {
            ApplyInclude();
        }

        public OrderSpecification(string paymentIntentId,bool IOrderHavePaymentIntent) : base(O => O.PaymentIntentId == paymentIntentId)
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(O => O.DeliveryMethod);
            Include.Add(O => O.Items);
            Include.Add(O => O.ShippingAddress);
        }
    }
}
