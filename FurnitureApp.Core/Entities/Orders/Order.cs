﻿using FurnitureApp.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Entities.Orders
{
    public class Order : BaseEntity<int>
    {
        public Order()
        {
            
        }
        public Order(string? buyerEmail,ShippingAddresses shippingAddress,DeliveryMethod? deliveryMethod, ICollection<OrderItem>? items, decimal? subTotal, string? paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string? BuyerEmail { get; set; }
        public DateTimeOffset? OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus? Status { get; set; } = OrderStatus.Pending;
        public ShippingAddresses ShippingAddress { get; set; }
        public int? DeliveryMethodId { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        public ICollection<OrderItem>? Items { get; set; }
        public decimal? SubTotal { get; set; }

        public decimal? GetTotal() => SubTotal + DeliveryMethod.Cost;
        public string? PaymentIntentId { get; set; }

    }
}
