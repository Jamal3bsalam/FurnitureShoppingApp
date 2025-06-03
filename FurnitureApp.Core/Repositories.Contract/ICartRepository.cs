using FurnitureApp.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Repositories.Contract
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string userId);
    }
}
