﻿using FurnitureApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Specification
{
    public interface ISpecification<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Include { get; set; }
        public Expression<Func<TEntity, object>> OrderBy { get; set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagination { get; set; }
    }
}
