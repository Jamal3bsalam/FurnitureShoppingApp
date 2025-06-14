using FurnitureApp.Core.Dtos.ShippingAddressesDtos;
using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.OrderDtos
{
    public class OrderToReturnDto
    {
        public int? Id { get; set; }
        public string? BuyerEmail { get; set; }
        public DateTimeOffset? OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public string? Status { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public string? DeliveryMethodName { get; set; }
        public ICollection<OrderItemDto>? Items { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Total { get; set; }
        public string? PaymentIntentId { get; set; } = string.Empty;
    }
}
