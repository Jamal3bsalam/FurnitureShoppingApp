using AutoMapper;
using FurnitureApp.Core.Dtos.ShippingAddressesDtos;
using FurnitureApp.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Mapping.ShippingAddressProfile
{
    public class AddressProfile:Profile
    {
        public AddressProfile()
        {
            CreateMap<ShippingAddresses,AddressDto>().ReverseMap();
        }
    }
}
