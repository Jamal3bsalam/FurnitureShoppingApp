using FurnitureApp.Core.Dtos.ShippingAddressesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Services.Contract.ShippingAddress
{
    public interface IShippingAddressService
    {
        Task<AddressDto>CreateAddressAsync(CreateAddressDto createAddressDto);
        Task<IEnumerable<AddressDto>> GetAllAddressesForUserAsync();
        Task<AddressDto> UpdateAddressAsync(int id,UpdateAddressDto updateAddressDto);
        Task<string>DeleteAddress(int id);   


    }
}
