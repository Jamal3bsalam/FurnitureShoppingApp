using FurnitureApp.Core;
using FurnitureApp.Core.Entities;
using FurnitureApp.Core.Repositories.Contract;
using FurnitureApp.Repository.Data.Context;
using FurnitureApp.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FurnitureDbContext _context;
        private readonly Hashtable _repositories;

        public UnitOfWork(FurnitureDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();    
        }
        public async Task<int> CompleteAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public IGenericRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity, Tkey>(_context);
                _repositories.Add(type, repository);
            }
            return _repositories[type] as IGenericRepository<TEntity, Tkey>;
        }
    }
}
