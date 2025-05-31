using AutoMapper;
using FurnitureApp.Core;
using FurnitureApp.Core.Dtos.ShippingAddressesDtos;
using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Services.Contract.ShippingAddress;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Service.Services.AddressService
{
    public class ShippingAddressService : IShippingAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<AppUser> _userManager;

        public ShippingAddressService(IUnitOfWork unitOfWork,IMapper mapper,IHttpContextAccessor httpContext,UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContext = httpContext;
            _userManager = userManager;
        }
        public async Task<AddressDto> CreateAddressAsync(CreateAddressDto createAddressDto)
        {
            var userId = _httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier).Value;   

            if (createAddressDto == null) return null;
            var Address = new ShippingAddresses()
            {
                FullName = createAddressDto.FullName,
                ZipCode = createAddressDto.ZipCode,
                Country = createAddressDto.Country,
                City = createAddressDto.City,
                District = createAddressDto.District,
                UserId = userId
            };
            await _unitOfWork.Repository<ShippingAddresses,int>().AddAsync(Address);
           var result = await _unitOfWork.CompleteAsync();
            if (result == 0) return null;

           var addressDto = _mapper.Map<AddressDto>(Address);
           return addressDto;
           
        }

        public async Task<IEnumerable<AddressDto>> GetAllAddressesForUserAsync()
        {
            var userId =  _httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;
            var shippingAddresses = await _unitOfWork.Repository<ShippingAddresses,int>().GetAllForUserAsync(userId);
            if (shippingAddresses == null) return null;
            var addressesDto = _mapper.Map<IEnumerable<AddressDto>>(shippingAddresses);
            return addressesDto;
        }

        public async Task<AddressDto> UpdateAddressAsync(int id,UpdateAddressDto updateAddressDto)
        {
            if (id == null) return null;

            var address = await _unitOfWork.Repository<ShippingAddresses, int>().GetByIdAsync(id);
            if (address == null) return null;
            address.FullName = updateAddressDto.FullName;
            address.City = updateAddressDto.City;
            address.Country = updateAddressDto.Country;
            address.ZipCode = updateAddressDto.ZipCode;
            address.District = updateAddressDto.District;

            _unitOfWork.Repository<ShippingAddresses, int>().Update(address);
            await _unitOfWork.CompleteAsync();
            var addressDto = _mapper.Map<AddressDto>(address);
            return addressDto;
        }

        public async Task<string> DeleteAddress(int id)
        {
            if (id == null) return null;
            var address = await _unitOfWork.Repository<ShippingAddresses,int>().GetByIdAsync(id);
            if (address == null) return null;
            _unitOfWork.Repository<ShippingAddresses, int>().Delete(address);
            var result = await _unitOfWork.CompleteAsync();
            if (result == 0) return null;
            return "Address Deleted Succesfully";
        }

       
    }
}
