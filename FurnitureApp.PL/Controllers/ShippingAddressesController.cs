using FurnitureApp.Core.Dtos.ShippingAddressesDtos;
using FurnitureApp.Core.Services.Contract.ShippingAddress;
using FurnitureApp.PL.Response.Error;
using FurnitureApp.PL.Response.GeneralResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureApp.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingAddressesController : ControllerBase
    {
        private readonly IShippingAddressService _shippingAddress;

        public ShippingAddressesController(IShippingAddressService shippingAddress)
        {
            _shippingAddress = shippingAddress;
        }

        [HttpGet("allAddresses")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AddressDto>>>> GetAllShippingAddresses()
        {
            var addresses = await _shippingAddress.GetAllAddressesForUserAsync();
            if (addresses == null) return BadRequest(new ErrorResponse(400));
            return Ok(new ApiResponse<IEnumerable<AddressDto>>(true, 200, "User shipping addresses fetched successfully.", addresses));
        }

        [HttpPost("addShippingAddress")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<AddressDto>>> CreateShippingAddress (CreateAddressDto createAddressDto)
        {
            var addressDto = await _shippingAddress.CreateAddressAsync(createAddressDto);
            if (addressDto == null) return BadRequest(new ErrorResponse(400));
            return Ok(new ApiResponse<AddressDto>(true, 200, "Shipping Address Added Successfully", addressDto));
        }

        [HttpPut("updateShippingAddress")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<AddressDto>>> UpdateShippingAddress(int id,UpdateAddressDto updateAddressDto)
        {
            var addressDto = await _shippingAddress.UpdateAddressAsync(id,updateAddressDto);
            if (addressDto == null) return BadRequest(new ErrorResponse(400));
            return Ok(new ApiResponse<AddressDto>(true, 200, "Shipping Address Updated Successfully", addressDto));
        }

        [HttpDelete("deleteShippingAddress")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteShippingAddress(int id)
        {
            var result = await _shippingAddress.DeleteAddress(id);
            if (result == null) return BadRequest(new ErrorResponse(400));
            return Ok(new ApiResponse<string>(true, 200, "Shipping Address Deleted Successfully", result));
        }

    }
}
