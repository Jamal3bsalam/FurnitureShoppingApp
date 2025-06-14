using FurnitureApp.Core;
using FurnitureApp.Core.Dtos.OrderDtos;
using FurnitureApp.Core.Dtos.ShippingAddressesDtos;
using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Entities.Orders;
using FurnitureApp.Core.Services.Contract.Orders;
using FurnitureApp.PL.Response.Error;
using FurnitureApp.PL.Response.GeneralResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureApp.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderService orderService, IConfiguration configuration)
        {
            _orderService = orderService;
            _configuration = configuration;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderToReturnDto>>> CreateOrder(OrderDto orderDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userEmail == null || userId == null) return Unauthorized(new ErrorResponse(StatusCodes.Status401Unauthorized));

            var Address = new ShippingAddresses()
            {
                FullName = orderDto.ShipToAddress.FullName,
                District = orderDto.ShipToAddress.District,
                City = orderDto.ShipToAddress.City,
                Country = orderDto.ShipToAddress.Country,
                ZipCode = orderDto.ShipToAddress.ZipCode,
            };
            var order = await _orderService.CreateOrderAsync(userEmail, userId, orderDto.DeliveryMethodId.Value, Address);

            var orderToReturn = new OrderToReturnDto()
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                ShippingAddress = new AddressDto() { FullName = order.ShippingAddress.FullName, City = order.ShippingAddress.City, District = order.ShippingAddress.District, Country = order.ShippingAddress.Country, ZipCode = order.ShippingAddress.ZipCode },
                DeliveryMethodName = order.DeliveryMethod.ShortName,
                SubTotal = order.SubTotal,
                Total = order.GetTotal(),
                PaymentIntentId = order.PaymentIntentId,
                Items = order.Items.Select(O => new OrderItemDto()
                {
                    ProductId = O.Product.ProductId,
                    ProductName = O.Product.ProductName,
                    PictureUrl = _configuration["BASEURL"] + O.Product.PictureUrl,
                    Price = O.Price,
                    Quantity = O.Quantity,
                }).ToList()
            };

            return Ok(new ApiResponse<OrderToReturnDto>(true, 200, "Order created successfully.", orderToReturn));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<ApiResponse<OrderToReturnDto>>>> GetAllOrdersForSpcificUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userEmail == null || userId == null) return Unauthorized(new ErrorResponse(StatusCodes.Status401Unauthorized));

            var orders = await _orderService.GetAllOrdersForUserAsnc(userEmail);

            var orderToReturn = orders.Select(order => new OrderToReturnDto()
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                ShippingAddress = new AddressDto() { FullName = order.ShippingAddress.FullName, City = order.ShippingAddress.City, District = order.ShippingAddress.District, Country = order.ShippingAddress.Country, ZipCode = order.ShippingAddress.ZipCode },
                DeliveryMethodName = order.DeliveryMethod.ShortName,
                SubTotal = order.SubTotal,
                Total = order.GetTotal(),
                PaymentIntentId = order.PaymentIntentId,
                Items = order.Items.Select(O => new OrderItemDto()
                {
                    ProductId = O.Product.ProductId,
                    ProductName = O.Product.ProductName,
                    PictureUrl = _configuration["BASEURL"] + O.Product.PictureUrl,
                    Price = O.Price,
                    Quantity = O.Quantity,
                }).ToList()
            });

            return Ok(new ApiResponse<IEnumerable<OrderToReturnDto>>(true, 200, "Orders Retrived successfully.", orderToReturn));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Id")]
        public async Task<ActionResult<IEnumerable<ApiResponse<OrderToReturnDto>>>> GetOrderById(int Id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userEmail == null || userId == null) return Unauthorized(new ErrorResponse(StatusCodes.Status401Unauthorized));

            var order = await _orderService.GetOrderByIdForUserAsnc(userEmail,Id);

            var orderToReturn = new OrderToReturnDto()
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                ShippingAddress = new AddressDto() { FullName = order.ShippingAddress.FullName, City = order.ShippingAddress.City, District = order.ShippingAddress.District, Country = order.ShippingAddress.Country, ZipCode = order.ShippingAddress.ZipCode },
                DeliveryMethodName = order.DeliveryMethod.ShortName,
                SubTotal = order.SubTotal,
                Total = order.GetTotal(),
                PaymentIntentId = order.PaymentIntentId,
                Items = order.Items.Select(O => new OrderItemDto()
                {
                    ProductId = O.Product.ProductId,
                    ProductName = O.Product.ProductName,
                    PictureUrl = _configuration["BASEURL"] + O.Product.PictureUrl,
                    Price = O.Price,
                    Quantity = O.Quantity,
                }).ToList()
            };

            return Ok(new ApiResponse<OrderToReturnDto>(true, 200, "Order Retrived successfully.", orderToReturn));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("status")]
        public async Task<ActionResult<IEnumerable<ApiResponse<OrderToReturnDto>>>> GetAllOrdersForSpecificOrderStatus(OrderStatus orderStatus)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userEmail == null || userId == null) return Unauthorized(new ErrorResponse(StatusCodes.Status401Unauthorized));

            var orders = await _orderService.GetAllOrdersForSpecificOrderStatus(orderStatus,userEmail);

            var orderToReturn = orders.Select(order => new OrderToReturnDto()
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                ShippingAddress = new AddressDto() { FullName = order.ShippingAddress.FullName, City = order.ShippingAddress.City, District = order.ShippingAddress.District, Country = order.ShippingAddress.Country, ZipCode = order.ShippingAddress.ZipCode },
                DeliveryMethodName = order.DeliveryMethod?.ShortName,
                SubTotal = order.SubTotal,
                Total = order.GetTotal(),
                PaymentIntentId = order.PaymentIntentId,
                Items = order.Items?.Select(O => new OrderItemDto()
                {
                    ProductId = O.Product.ProductId,
                    ProductName = O.Product.ProductName,
                    PictureUrl = _configuration["BASEURL"] + O.Product.PictureUrl,
                    Price = O.Price,
                    Quantity = O.Quantity,
                }).ToList()
            });

            return Ok(new ApiResponse<IEnumerable<OrderToReturnDto>>(true, 200, "Orders Retrived successfully.", orderToReturn));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IEnumerable<ApiResponse<DeliveryMethod>>>> GetAllDeliveryMethod()
        {
            var deliveryMethod = await _orderService.GetAllDeliveryMethodrAsnc();
            if (deliveryMethod == null) return BadRequest(StatusCodes.Status400BadRequest);
            return Ok(new ApiResponse<IEnumerable<DeliveryMethod>>(true,200,"Delivery Methods Retrived Successfully",deliveryMethod));
        }

    }
}
