using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TR.SystemOfLegalCases.Domain.AbstractBaseModel;

namespace TR.SystemOfLegalCases.Domain.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Add(TEntity entity);
        Task<TEntity> GetById(Guid id);
        Task<List<TEntity>> GetAll();
        Task Update(TEntity entity);
        Task Remove(Guid id);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChanges();
        bool DomainExist(Guid id, Guid? idempresa = null);
        bool UpdateValeuWithViewModel(object model, object viewmodel);
    }
}
