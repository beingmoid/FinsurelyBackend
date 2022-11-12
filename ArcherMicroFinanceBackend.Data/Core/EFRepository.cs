using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using NukesLab.Core.Common;
using NukesLab.Core.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PanoramBackend.Data
{
	public class EFRepository<TEntity, TKey> : EFRepository<AMFContext, TEntity, TKey>
		where TEntity : class, IBaseEntity<TKey>, new()
	{
		public EFRepository(AMFContext requestScope)
			: base(requestScope)
		{

		}
	}


	public class EFRepository<Context, TEntity, TKey> : IEFRepository<TEntity, TKey>
		where Context : AMFContext
		where TEntity : class, IBaseEntity<TKey>, new()
	{
		private readonly Context _requestScope;

		public EFRepository(Context requestScope)
		{
			_requestScope = requestScope;

		}


		public IQueryable<TEntity> _baseQuery
		{
			get => _requestScope
.Set<TEntity>()
.AsNoTracking(); set { this._baseQuery = value; }
		}


		protected virtual IQueryable<TEntity> Query => _baseQuery;
			

		private IQueryable<TEntity> ApplyPredicates(IQueryable<TEntity> query, params Expression<Func<TEntity, bool>>[] predicates)
		{
			foreach (var predicate in predicates)
			{
				if (predicate != null)
				{

					query = query.Where(predicate);
				}
			}
			return query;
		}


		#region Get

		/// <summary>
		/// Query Database by and filter sequence value based on predicate
		/// </summary>
		/// <param name="predicates"></param>
		/// <returns></returns>
		public async Task<IEnumerable<TEntity>> Get(params Expression<Func<TEntity, bool>>[] predicates)
			=> await ApplyPredicates(this.Query, predicates).ToListAsync();

		/// <summary>
		/// This method returns List of entity with its navigation property if its been passed as reference in include parameter doing query with Child collections and parent navigation properties
		/// </summary>
		/// <param name="includeExpression">
		/// Include Expression are the collections or property navigation items which are used to load entites while querying
		/// </param>
		/// <param name="predicates">
		///		Your Condition
		/// </param>
		/// <returns></returns>
		public async Task<IEnumerable<TEntity>> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
			params Expression<Func<TEntity, bool>>[] predicates)
				=> await ApplyPredicates(includeExpression(_baseQuery), predicates)
																			.ToListAsync();
		/// <summary>
		/// This method works as a selector statement
		/// </summary>
		/// <typeparam name="TReturn">
		///	Returns array of select expression
		/// </typeparam>
		/// <param name="selectExpression">
		///		Use this like this.Get(x=> new { SomeValuee=x.SomeValue });
		/// </param>
		/// <returns></returns>
		public async Task<IEnumerable<TReturn>> Get<TReturn>(Expression<Func<TEntity, TReturn>> selectExpression)
			=> await Query.Select(selectExpression).ToListAsync();
		/// <summary>
		///	This overload wil applicable where you want to use include and select
		/// </summary>
		/// <typeparam name="TReturn"></typeparam>
		/// <param name="includeExpression"></param>
		/// <param name="selectExpression"></param>
		/// <returns></returns>
		public async Task<IEnumerable<TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
			Expression<Func<TEntity, TReturn>> selectExpression)
				=> await ApplyPredicates(includeExpression(_baseQuery)).Select(selectExpression).ToListAsync();
		/// <summary>
		/// This overload will be used where you want to include navigation properties then pass a select statement then use a predicte to filter the sequence
		/// </summary>
		/// <typeparam name="TReturn"></typeparam>
		/// <param name="includeExpression"> Include navigtion entries </param>
		/// <param name="selectExpression"> Pass a select statement </param>
		/// <param name="predicate">Create a lambda expressions </param>
		/// <returns></returns>
		public async Task<IEnumerable<TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
		Expression<Func<TEntity, TReturn>> selectExpression, params Expression<Func<TEntity, bool>>[] predicate)
			=> await ApplyPredicates(includeExpression(_baseQuery), predicate).Select(selectExpression).ToListAsync();


		public async Task<TEntity> GetOne(TKey id) => await Query.SingleOrDefaultAsync(x => x.Id.Equals(id));

		/// <summary>
		/// Pass Id as parameter
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<TEntity> GetOne(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression)
		{
			var result = await ApplyPredicates(includeExpression(_baseQuery), x => x.Id.Equals(id)).SingleOrDefaultAsync();

			return result;
		}

		/// <summary>
		/// Pass Lambda expression as parameter
		/// </summary>
		/// <param name="predicates"></param>
		/// <returns></returns>
		public async Task<TEntity> GetOne(params Expression<Func<TEntity, bool>>[] predicates) => await ApplyPredicates(this.Query, predicates).FirstOrDefaultAsync();
		public async Task<IEnumerable<TEntity>> Get() => await this.Query.ToListAsync();




		#region DML

		public async Task Insert(IEnumerable<TEntity> entities)
        {
			this._requestScope.Set<TEntity>().AddRange(entities);
			await Task.FromResult(true);
		}
							  

		public async Task BulkInsert(IEnumerable<TEntity> entities)
					=> await this._requestScope.BulkInsertAsync(entities);
		public async Task<bool> Update(TKey id, TEntity entity)
        {
			//TEntity dbEntity = await _requestScope.FindAsync<TEntity>(entity.Id);
			return  _requestScope.Update(entity).State == EntityState.Modified;
			//var dbEntry = _requestScope.Entry(dbEntity);
			//dbEntry.CurrentValues.SetValues(entity);
			
			//return await Task.FromResult(true);
		}


        //public async Task<bool> Update(TKey id, TEntity entity, params Expression<Func<TEntity, object>>[] navigations)
        //{
        //    TEntity dbEntity = await _requestScope.FindAsync<TEntity>(entity.Id);

        //    EntityEntry<TEntity> dbEntry = _requestScope.Entry(dbEntity);
        //    dbEntry.CurrentValues.SetValues(entity);

        //    foreach (Expression<Func<TEntity, object>> property in navigations)
        //    {
        //        var propertyName = property.GetPropertyAccess().Name;
        //        CollectionEntry dbItemsEntry = dbEntry.Collection(propertyName);
        //        IClrCollectionAccessor accessor = dbItemsEntry.Metadata.GetCollectionAccessor();

        //        await dbItemsEntry.LoadAsync();
        //        var dbItemsMap = ((IEnumerable<object>)dbItemsEntry.CurrentValue)
        //            .ToDictionary(e => string.Join('|', FindPrimaryKeyValues(e)));

        //        foreach (var item in (IEnumerable)accessor.GetOrCreate(entity, false))
        //        {
        //            if (!dbItemsMap.TryGetValue(string.Join('|', FindPrimaryKeyValues(item)), out object oldItem))
        //            {
        //                accessor.Add(dbEntity, item, false);
        //            }
        //            else
        //            {
        //                _requestScope.Entry(oldItem).CurrentValues.SetValues(item);
        //                dbItemsMap.Remove(string.Join('|', FindPrimaryKeyValues(item)));
        //            }
        //        }

        //        foreach (var oldItem in dbItemsMap.Values)
        //        {
        //            accessor.Remove(dbEntity, oldItem);
        //            //await Delete((oldItem as TEntity).Id, (oldItem as TEntity));

        //        }
        //    }

        //    return await this.SaveChanges();
        //}
        public IReadOnlyList<IProperty> FindPrimaryKeyProperties<TEntity>(TEntity entity)
		{
			return _requestScope.Model.FindEntityType(entity.GetType()).FindPrimaryKey().Properties;
		}

		public IEnumerable<object> FindPrimaryKeyValues<TEntity>(TEntity entity) where TEntity : class
		{
			return from p in FindPrimaryKeyProperties(entity)
				   select entity.GetType().GetProperty(p.Name).GetValue(entity, null);
		}


        public async Task<bool> Update(TKey id, TEntity entity, Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression = null, params Expression<Func<TEntity, object>>[] navigations)
        {
            //TEntity dbEntity = await _requestScope.FindAsync<TEntity>(entity.Id);
      


            var dbEntry = _requestScope.Entry(entity);
            dbEntry.CurrentValues.SetValues(entity);

            foreach (var property in navigations)
            {
                var propertyName = property.GetPropertyAccess().Name;
                var dbItemsEntry = dbEntry.Collection(propertyName);
                var accessor = dbItemsEntry.Metadata.GetCollectionAccessor();

                await dbItemsEntry.LoadAsync();
                var dbItemsMap = ((IEnumerable<IBaseEntity>)dbItemsEntry.CurrentValue)
                    .ToDictionary(e => e.Id);

                var items = (IEnumerable<IBaseEntity>)accessor.GetOrCreate(entity, false);

             

         

            }

            return await this.SaveChanges();
        }


        public async Task<bool> Delete(TKey id, TEntity entity,  Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression = null,params Expression<Func<TEntity, object>>[] navigations)
		
		{
			entity.IsDeleted = true;
            if (navigations != null)
            {
                if (includeExpression != null)
                {
                    return await this.Update(id, entity, includeExpression, navigations);
                }
                else
                {
                    return await this.Update(id, entity, null, navigations);
                }

            }
            else
            {
                return await this.Update(id, entity);
            }
            return await this.Update(id, entity);

		}
		#endregion

		#region Aggregate
		public async Task<int> Count() => await this.Query.CountAsync();
		public async Task<int> Count(params Expression<Func<TEntity, bool>>[] predicates) => await ApplyPredicates(this.Query, predicates).CountAsync();

		public async Task<bool> Any() => await this.Query.AnyAsync();
		public async Task<bool> Any(params Expression<Func<TEntity, bool>>[] predicates) => await ApplyPredicates(this.Query, predicates).AnyAsync();




		public async Task<bool> Any(object id)
						=> await this.Query.AnyAsync(o => o.Id.Equals(id));
		#endregion
		public async Task<bool> SaveChanges()
		{
			var changeTracker = _requestScope.ChangeTracker;
			changeTracker.DetectChanges();

			var markedAsDeleted = changeTracker.Entries<IBaseEntity>().Where(x => x.Entity.IsDeleted);

			foreach (EntityEntry<IBaseEntity> item in markedAsDeleted)
			{
				var navigations = item.Navigations.Where(n => !n.Metadata.IsDependentToPrincipal()).ToArray();

				foreach (var navigationEntry in navigations)
				{
					if (navigationEntry is CollectionEntry collectionEntry && collectionEntry.CurrentValue!=null)
					{
						foreach (IBaseEntity dependentEntry in collectionEntry?.CurrentValue
							)
						{
							dependentEntry.IsDeleted = true;
						}
					}
					else if (navigationEntry.CurrentValue is IBaseEntity dependentEntry)
					{
						dependentEntry.IsDeleted = true;
					}
				}
			}

			return await this._requestScope.SaveChangesAsync() > 0;

		}










		#endregion


	}


	public interface IEFRepository<TEntity, TKey> : IEFRepository<TEntity>
		where TEntity : class, IBaseEntity<TKey>, new()
	{
		Task<TEntity> GetOne(TKey id);
		Task<TEntity> GetOne(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression);
		Task<IEnumerable<TEntity>> Get(params Expression<Func<TEntity, bool>>[] predicates);
		Task<IEnumerable<TEntity>> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, params Expression<Func<TEntity, bool>>[] predicates);
		Task<IEnumerable<TReturn>> Get<TReturn>(Expression<Func<TEntity, TReturn>> selectExpression);
		Task<IEnumerable<TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, Expression<Func<TEntity, TReturn>> selectExpression);
		Task<IEnumerable<TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
	   Expression<Func<TEntity, TReturn>> selectExpression, params Expression<Func<TEntity, bool>>[] predicate);
		Task Insert(IEnumerable<TEntity> entities);
		Task BulkInsert(IEnumerable<TEntity> entities);

		Task<bool> Update(TKey id, TEntity entity);
		Task<bool> Update(TKey id, TEntity entity, Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression = null, params Expression<Func<TEntity, object>>[] navigations);
		//Task<bool> Delete(TKey id, TEntity entity);
		Task<bool> Delete(TKey id, TEntity entity, Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression = null, params Expression<Func<TEntity, object>>[] navigations);


		IQueryable<TEntity> _baseQuery { get; set; }
	}


	public interface IEFRepository<TEntity> : IEFRepository
		where TEntity : class, IBaseEntity, new()
	{
		Task<TEntity> GetOne(params Expression<Func<TEntity, bool>>[] predicates);
		Task<int> Count(params Expression<Func<TEntity, bool>>[] predicates);
		Task<bool> Any(params Expression<Func<TEntity, bool>>[] predicates);
	}

	public interface IEFRepository
	{
		Task<int> Count();
		Task<bool> Any();
		Task<bool> Any(object id);

		Task<bool> SaveChanges();
	}
	public static class ExtentionMethods
    {
		public static object GetPropValue<TEntity,TKey>(this TEntity src, string propName) where TEntity : class, IBaseEntity<TKey>, new()
		{
			return src.GetType().GetProperty(propName).GetValue(src, null);
		}
	}
}
