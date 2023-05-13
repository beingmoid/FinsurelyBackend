using AIB.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AIB.Data.Repositories.Core
{
	public class EFRepository<TEntity, TKey> : EFRepository<AIBContext, TEntity, TKey>
		  where TEntity : class, IBaseEntity<TKey>, new()
	{
		public EFRepository(AIBContext requestScope) : base(requestScope)
		{
		}
	}

	public class EFRepository<Context, TEntity, TKey> : IEFRepository<TEntity, TKey>
	where TEntity : class, IBaseEntity<TKey>, new()
	{
		public EFRepository(AIBContext requestScope)
		{
			this._requestScope = requestScope;
		}

		public readonly AIBContext _requestScope;
		private IQueryable<TEntity> _baseQuery => this._requestScope
			.Set<TEntity>()
			.AsNoTracking()
			;


		public virtual IQueryable<TEntity> Query => _baseQuery;

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
		
		public async Task<Dictionary<TKey, TEntity>> Get(params Expression<Func<TEntity, bool>>[] predicates)
			=> await ApplyPredicates(this.Query, predicates).ToDictionaryAsync(o => o.Id);

		public async Task<Dictionary<TKey, TEntity>> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, params Expression<Func<TEntity, bool>>[] predicates)
			=> await ApplyPredicates(includeExpression(_baseQuery), predicates).ToDictionaryAsync(o => o.Id);

		public async Task<Dictionary<TKey, TReturn>> Get<TReturn>(Expression<Func<TEntity, TReturn>> selectExpression, Func<TReturn, TKey> keySelector, params Expression<Func<TEntity, bool>>[] predicates)
			=> await ApplyPredicates(this.Query, predicates).Select(selectExpression).ToDictionaryAsync(keySelector);
		public async Task<Dictionary<TKey, TReturn>> Get<TReturn>(Expression<Func<TEntity, TReturn>> selectExpression, Func<TReturn, TKey> keySelector)
			=> await this.Query.Select(selectExpression).ToDictionaryAsync(keySelector);
		public async Task<Dictionary<TKey, TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, Expression<Func<TEntity, TReturn>> selectExpression, Func<TReturn, TKey> keySelector)
			=> await ApplyPredicates(includeExpression(_baseQuery)).Select(selectExpression).ToDictionaryAsync(keySelector);
		public async Task<Dictionary<TKey, TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, Expression<Func<TEntity, TReturn>> selectExpression, Func<TReturn, TKey> keySelector, params Expression<Func<TEntity, bool>>[] predicates)
			=> await ApplyPredicates(includeExpression(_baseQuery), predicates).Select(selectExpression).ToDictionaryAsync(keySelector);
		public async Task<TEntity> GetOne(params Expression<Func<TEntity, bool>>[] predicates) => await ApplyPredicates(this.Query, predicates).FirstOrDefaultAsync();

		public async Task<TEntity> GetOne(TKey id) => await this.Query.FirstOrDefaultAsync(o => o.Id.Equals(id));

		#endregion

		#region Aggregate
		public async Task<int> Count() => await this.Query.CountAsync();
		public async Task<int> Count(params Expression<Func<TEntity, bool>>[] predicates) => await ApplyPredicates(this.Query, predicates).CountAsync();

		public async Task<bool> Any() => await this.Query.AnyAsync();
		public async Task<bool> Any(params Expression<Func<TEntity, bool>>[] predicates) => await ApplyPredicates(this.Query, predicates).AnyAsync();

		async Task<bool> IEFRepository.Any(object id) => await this.Query.AnyAsync(o => o.Id.Equals(id));

		#endregion
		public async Task Insert(IEnumerable<TEntity> entities) => await this._requestScope.Set<TEntity>().AddRangeAsync(entities);

		//public async Task BulkInsert(IEnumerable<TEntity> entities) => await this._requestScope.BulkInsertAsync(entities);
	

		public bool Update(TKey id, TEntity entity) => this._requestScope.Set<TEntity>().Update(entity).State == EntityState.Modified;

		public async Task<bool> SaveChanges()
		{
			var changeTracker = _requestScope.ChangeTracker;
			changeTracker.DetectChanges();

			var markedAsDeleted = changeTracker.Entries<IBaseEntity>().Where(x => x.Entity.IsDeleted);

			foreach (EntityEntry<IBaseEntity> item in markedAsDeleted)
			{
				var navigations = item.Metadata.GetNavigations().Where(n => !n.IsDependentToPrincipal()).ToArray();

				foreach (var navigationEntry in navigations)
				{
					if (navigationEntry is CollectionEntry collectionEntry)
					{
						foreach (IBaseEntity dependentEntry in collectionEntry.CurrentValue)
						{
							dependentEntry.IsDeleted = true;
						}
					}
					else if (navigationEntry is IBaseEntity dependentEntry)
					{
						dependentEntry.IsDeleted = true;
					}
				}
			}

			return await this._requestScope.SaveChangesAsync() > 0;
		}

		public async Task<bool> Delete(TKey id, TEntity entity)
		{
			//var dbEntity = await this.GetOne(id);
			//dbEntity.IsDeleted = true;
			entity.IsDeleted = true;
			return this.Update(id, entity);
		}

		public async Task<List<TEntity>> GetPaginated(int page, int pageSize) 
		{
			var items = await this._requestScope.Set<TEntity>().Skip(page * pageSize).Take(pageSize).ToListAsync();
			return items;
		}
	}

	public interface IEFRepository<TEntity, TKey> : IEFRepository<TEntity>
		where TEntity : class, IBaseEntity<TKey>, new()
	{
		IQueryable<TEntity> Query { get; }
		Task<TEntity> GetOne(TKey id);
		Task<Dictionary<TKey, TEntity>> Get(params Expression<Func<TEntity, bool>>[] predicates);
		Task<Dictionary<TKey, TEntity>> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, params Expression<Func<TEntity, bool>>[] predicates);
		Task<Dictionary<TKey, TReturn>> Get<TReturn>(Expression<Func<TEntity, TReturn>> selectExpression, Func<TReturn, TKey> keySelector);
		Task<Dictionary<TKey, TReturn>> Get<TReturn>(Expression<Func<TEntity, TReturn>> selectExpression, Func<TReturn, TKey> keySelector, params Expression<Func<TEntity, bool>>[] predicates);
		Task<Dictionary<TKey, TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, Expression<Func<TEntity, TReturn>> selectExpression, Func<TReturn, TKey> keySelector);
		Task<Dictionary<TKey, TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, Expression<Func<TEntity, TReturn>> selectExpression, Func<TReturn, TKey> keySelector, params Expression<Func<TEntity, bool>>[] predicates);
		Task Insert(IEnumerable<TEntity> entities);



		bool Update(TKey id, TEntity entity);

		Task<bool> Delete(TKey id, TEntity entity);

		Task<List<TEntity>> GetPaginated(int page, int pageSize);
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
}
