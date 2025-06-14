using FurnitureApp.Core.Dtos.CartDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Services.Contract.Carts
{
    public interface ICartService
    {
        Task<CartDto> GetCartWithUserId(string userId);

        Task<bool> AddItemToCartAsync(string userId, int productId, int quantity);

        Task<bool> RemoveItemFromCartAsync(string userId, int productId);

        Task<bool> ClearCartAsync(string userId);

    }
}
