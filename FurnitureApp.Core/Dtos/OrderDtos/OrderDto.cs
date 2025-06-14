using FurnitureApp.Core.Dtos.ShippingAddressesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.OrderDtos
{
    public class OrderDto
    {
        public int? CartId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public AddressDto? ShipToAddress { get; set; }
    }
}
