using FurnitureApp.Core.Entities;
using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Entities.Products;
using FurnitureApp.Core.Repositories.Contract;
using FurnitureApp.Core.Specification;
using FurnitureApp.Repository.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Repository.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly FurnitureDbContext _context;

        public GenericRepository(FurnitureDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (typeof(TEntity) == typeof(ShippingAddresses))
            {
                return (IEnumerable<TEntity>)await _context.Set<ShippingAddresses>().ToListAsync();
            }
            if (typeof(TEntity) == typeof(Category))
            {
                return (IEnumerable<TEntity>)await _context.Set<Category>().ToListAsync();
            }
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllForUserAsync(string id)
        {
            if(typeof(TEntity) == typeof(ShippingAddresses))
            {
                return (IEnumerable<TEntity>)await _context.Set<ShippingAddresses>().Where(s => s.UserId == id).ToListAsync();
            }
            if (typeof(TEntity) == typeof(Reviews))
            {
                return (IEnumerable<TEntity>)await _context.Set<Reviews>().Where(r => r.UserId == id).Include(r => r.Product).ToListAsync();
            }
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Tkey Id)
        {
            if(typeof(TEntity) == typeof(ShippingAddresses))
            {
                return await _context.Set<ShippingAddresses>().Where(S => S.Id == Id as int?).FirstOrDefaultAsync() as TEntity;
            }
            return await _context.Set<TEntity>().FindAsync(Id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }


        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, Tkey> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity, Tkey> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecification<TEntity, Tkey> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }

        private IQueryable<TEntity> ApplySpecifications(ISpecification<TEntity, Tkey> spec)
        {
            return SpecificEvaluator<TEntity, Tkey>.GetQuery(_context.Set<TEntity>(), spec);
        }

    }
}
