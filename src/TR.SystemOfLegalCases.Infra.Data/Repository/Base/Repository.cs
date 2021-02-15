using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using TR.SystemOfLegalCases.Domain.AbstractBaseModel;
using TR.SystemOfLegalCases.Domain.Interfaces;
using TR.SystemOfLegalCases.Infra.Data.Context;
using TR.SystemOfLegalCases.Infra.Data.Pagination;

namespace TR.SystemOfLegalCases.Infra.Data.Repository.Base
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        public SLCContext Db { get; protected set; }

        public virtual DbSet<TEntity> DbSet { get; protected set; }

        protected PaginatedList<TEntity> _paginated { get; set; }

        protected async Task<PaginatedList<TEntity>> ReturnPaginatedList(IQueryable<TEntity> source, int? pageIndex = 1, int? pageSize = 50)
        {
            if (!pageIndex.HasValue || pageIndex < 1)
            {
                pageIndex = 1;
            }

            if (!pageSize.HasValue || (pageSize < 1 || pageSize > 50))
            {
                pageSize = 50;
            }

            return await PaginatedList<TEntity>.CreateAsync(source, pageIndex.Value, pageSize.Value);
        }

        protected Repository(SLCContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public bool UpdateValeuWithViewModel(object model, object viewmodel)
        {
            bool hasColumnUpdate = false;

            var all_properties = viewmodel.GetType().GetProperties();

            foreach (var item in all_properties)
            {
                string propertyname = item.Name;

                object viewmodel_value = GetPropValue(viewmodel, propertyname);
                object model_value = GetPropValue(model, propertyname);

                if (viewmodel_value == null || string.Compare(propertyname, "Id") == 0)
                    continue;

                if (!viewmodel_value.Equals(model_value))
                {
                    hasColumnUpdate = true;
                    SetValue(model, propertyname, viewmodel_value);
                    Db.Entry(model).Property(propertyname).IsModified = true;
                }
                else
                {
                    Db.Entry(model).Property(propertyname).IsModified = false;
                }
            }

            return hasColumnUpdate;
        }

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private static void SetValue(object inputObject, string propertyName, object propertyVal)
        {
            Type type = inputObject.GetType();

            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            //Type propertyType = propertyInfo.PropertyType;
            var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;

            propertyVal = Convert.ChangeType(propertyVal, targetType);

            propertyInfo.SetValue(inputObject, propertyVal, null);
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        public virtual IQueryable<TEntity> ReturnIQueryable()
        {
            return Db.Set<TEntity>().AsQueryable().AsNoTracking();
        }

        public virtual async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await ReturnIQueryable().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            var model = await ReturnIQueryable().Where(d => d.Id.Equals(id)).FirstOrDefaultAsync();
            return model;
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await ReturnIQueryable().ToListAsync();
        }

        public virtual async Task Add(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Update(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remove(Guid id)
        {
            DbSet.Remove(new TEntity { Id = id });
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }

        public virtual bool DomainExist(Guid id, Guid? idempresa = null)
        {
            return DbSet.Count(b => b.Id.Equals(id)) > 0;
        }
    }

    public static class RepositoryExtesion
    {
        public static string ToScape(this string str)
        {
            return string.Format("%{0}%", str);
        }

        public static IQueryable<T> OrderByNew<T>(this IQueryable<T> source, string ordering)
        {
            if (ordering == null || string.IsNullOrEmpty(ordering.Trim()))
            {
                return source;
            }

            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");
            bool ascending = !ordering.Contains(" DESC");
            ordering = ordering.Replace(" DESC", "").Replace(" ASC", "");

            try
            {
                PropertyInfo property;
                Expression propertyAccess;
                if (ordering.Contains('.'))
                {
                    // support to be sorted on child fields.
                    String[] childProperties = ordering.Split('.');
                    property = type.GetProperty(childProperties[0]);
                    propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    for (int i = 1; i < childProperties.Length; i++)
                    {
                        property = property.PropertyType.GetProperty(childProperties[i]);
                        propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                    }
                }
                else
                {
                    property = typeof(T).GetProperty(ordering);
                    propertyAccess = Expression.MakeMemberAccess(parameter, property);
                }
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                MethodCallExpression resultExp = Expression.Call(typeof(Queryable),
                                                                 ascending ? "OrderBy" : "OrderByDescending",
                                                                 new[] { type, property.PropertyType }, source.Expression,
                                                                 Expression.Quote(orderByExp));
                //return  source.OrderBy(x => orderByExp);
                return source.Provider.CreateQuery<T>(resultExp);
            }
            catch
            {
                return source;
            }
        }

        private static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> q, string SortField, bool Ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
    }
}
