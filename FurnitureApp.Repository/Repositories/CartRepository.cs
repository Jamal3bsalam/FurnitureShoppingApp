using FurnitureApp.Core.Entities.Products;
using FurnitureApp.Core.Repositories.Contract;
using FurnitureApp.Repository.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FurnitureApp.Repository.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly FurnitureDbContext _context;

        public CartRepository(FurnitureDbContext context)
        {
            _context = context;
        }
        public async Task<Cart> GetCartAsync(string userId)
        {
            return await _context.Carts.Where(C => C.UserId == userId).Include(C => C.Items).ThenInclude(I => I.Product).FirstOrDefaultAsync();
        }
    }
}
