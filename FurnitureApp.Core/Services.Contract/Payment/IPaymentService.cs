using FurnitureApp.Core.Dtos.CartDtos;
using FurnitureApp.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Services.Contract.Payment
{
    public interface IPaymentService
    {
        Task<CartDto> CreateOrUpdatePaymentIntentId(int CartId);
    }
}
