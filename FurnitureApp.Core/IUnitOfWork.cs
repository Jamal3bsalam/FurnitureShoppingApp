using FurnitureApp.Core.Entities;
using FurnitureApp.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
        IGenericRepository<TEntity,Tkey> Repository<TEntity,Tkey>() where TEntity : BaseEntity<Tkey>;
    }
}
