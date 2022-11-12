//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using NukesLab.Core.Common;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//{


//    public class EFRepository<TEntity,TKey,TUser,TRole>: EFRepository<IdentityDbContext<TUser, TRole, string>, TUser, IdentityRole, TEntity, TKey>

//		where TEntity : class, IBaseEntity<TKey>, new()

//		where TUser : IdentityUser
//		where TRole : IdentityRole


//	{
//        public EFRepository(RequestScope<IdentityDbContext<IdentityUser, IdentityRole, string>, IdentityUser, IdentityRole> requestScope)
//            : base(requestScope)
//        {

//        }
//    }


//    public class EFRepository<TContext,TUser, TRole, TEntity,TKey> : IEFRepository<TEntity, TKey>
//		where TEntity :class ,IBaseEntity<TKey>,new()
//		where TUser:IdentityUser
//		where TRole: IdentityRole
//		where TContext : NukesLabEFContext<TUser,TRole> ,new()

//	{
//		private readonly RequestScope<TContext, TUser, TRole> _requestScope;

//        public EFRepository(RequestScope<TContext, TUser, TRole> requestScope)
//        {
//            _requestScope = requestScope;
//        }

     
//        private IQueryable<TEntity> _baseQuery => _requestScope.DbContext
//			.Set<TEntity>()
//			.AsNoTracking();

//		protected virtual IQueryable<TEntity> Query => _baseQuery;


//		private  IQueryable<TEntity> ApplyPredicates(IQueryable<TEntity> query, params Expression<Func<TEntity,bool>>[] predicates)
//        {
//            foreach (var predicate in predicates)
//            {
//				if (predicate != null)
//                {
					
//					query = query.Where(predicate);
//                }
//            }
//			return query;
//        }

//		#region Get

//		/// <summary>
//		/// Query Database by and filter sequence value based on predicate
//		/// </summary>
//		/// <param name="predicates"></param>
//		/// <returns></returns>
//		public async Task<IEnumerable<TEntity>> Get(params Expression<Func<TEntity, bool>>[] predicates)
//			=> await ApplyPredicates(this.Query, predicates).ToListAsync();

//		/// <summary>
//		/// This method returns List of entity with its navigation property if its been passed as reference in include parameter doing query with Child collections and parent navigation properties
//		/// </summary>
//		/// <param name="includeExpression">
//		/// Include Expression are the collections or property navigation items which are used to load entites while querying
//		/// </param>
//		/// <param name="predicates">
//		///		Your Condition
//		/// </param>
//		/// <returns></returns>
//		public async Task<IEnumerable<TEntity>> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
//			params Expression<Func<TEntity, bool>>[] predicates)
//				=> await ApplyPredicates(includeExpression(_baseQuery), predicates)
//																			.ToListAsync();
//		/// <summary>
//		/// This method works as a selector statement
//		/// </summary>
//		/// <typeparam name="TReturn">
//		///	Returns array of select expression
//		/// </typeparam>
//		/// <param name="selectExpression">
//		///		Use this like this.Get(x=> new { SomeValuee=x.SomeValue });
//		/// </param>
//		/// <returns></returns>
//		public async Task<IEnumerable<TReturn>> Get<TReturn>(Expression<Func<TEntity, TReturn>> selectExpression)
//			=> await Query.Select(selectExpression).ToListAsync();
//		/// <summary>
//		///	This overload wil applicable where you want to use include and select
//		/// </summary>
//		/// <typeparam name="TReturn"></typeparam>
//		/// <param name="includeExpression"></param>
//		/// <param name="selectExpression"></param>
//		/// <returns></returns>
//		public async Task<IEnumerable<TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
//			Expression<Func<TEntity, TReturn>> selectExpression)
//				=> await ApplyPredicates(includeExpression(_baseQuery)).Select(selectExpression).ToListAsync();
//		/// <summary>
//		/// This overload will be used where you want to include navigation properties then pass a select statement then use a predicte to filter the sequence
//		/// </summary>
//		/// <typeparam name="TReturn"></typeparam>
//		/// <param name="includeExpression"> Include navigtion entries </param>
//		/// <param name="selectExpression"> Pass a select statement </param>
//		/// <param name="predicate">Create a lambda expressions </param>
//		/// <returns></returns>
//		public async Task<IEnumerable<TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
//		Expression<Func<TEntity, TReturn>> selectExpression, params Expression<Func<TEntity, bool>>[] predicate)
//			=> await ApplyPredicates(includeExpression(_baseQuery), predicate).Select(selectExpression).ToListAsync();
//		/// <summary>
//		/// Pass Id as parameter
//		/// </summary>
//		/// <param name="id"></param>
//		/// <returns></returns>
//		public async Task<TEntity> GetOne(TKey id)
//			=> await Query.SingleOrDefaultAsync(x => x.Id.Equals(id));
//		/// <summary>
//		/// Pass Lambda expression as parameter
//		/// </summary>
//		/// <param name="predicates"></param>
//		/// <returns></returns>
//		public async Task<TEntity> GetOne(params Expression<Func<TEntity, bool>>[] predicates) => await ApplyPredicates(this.Query, predicates).FirstOrDefaultAsync();
//		public async Task<IEnumerable<TEntity>> Get() => await this.Query.ToListAsync();




//		#region DML

//		public async Task Insert(IEnumerable<TEntity> entities)
//								=> await this._requestScope.DbContext.Set<TEntity>().AddRangeAsync(entities);

//        public async Task BulkInsert(IEnumerable<TEntity> entities)
//					=> await this._requestScope.DbContext.BulkInsertAsync(entities);

//		public bool Update(TKey id, TEntity entity)
//				 => this._requestScope.DbContext.Set<TEntity>().Update(entity).State == EntityState.Modified;

//		public async Task<bool> Delete(TKey id, TEntity entity)
//        {
//			entity.IsDeleted = true;
//			return await Task.FromResult(this.Update(id, entity)); 
//		}
//		#endregion

//		#region Aggregate
//		public async Task<int> Count() => await this.Query.CountAsync();
//		public async Task<int> Count(params Expression<Func<TEntity, bool>>[] predicates) => await ApplyPredicates(this.Query, predicates).CountAsync();

//		public async Task<bool> Any() => await this.Query.AnyAsync();
//		public async Task<bool> Any(params Expression<Func<TEntity, bool>>[] predicates) => await ApplyPredicates(this.Query, predicates).AnyAsync();

	
   

//        public async Task<bool> Any(object id)
//						=> await this.Query.AnyAsync(o => o.Id.Equals(id));
//		#endregion
//		public async Task<bool> SaveChanges()
//		{
//			var changeTracker = _requestScope.DbContext.ChangeTracker;
//			changeTracker.DetectChanges();

//			var markedAsDeleted = changeTracker.Entries<IBaseEntity>().Where(x => x.Entity.IsDeleted);

//			foreach (EntityEntry<IBaseEntity> item in markedAsDeleted)
//			{

//				var navigations = item.Navigations.Where(n=>!n.Metadata.IsDependentToPrincipal()).ToArray();

//				foreach (var navigationEntry in navigations)
//				{
//					if (navigationEntry is CollectionEntry collectionEntry)
//					{
//						foreach (IBaseEntity dependentEntry in collectionEntry.CurrentValue)
//						{
//							dependentEntry.IsDeleted = true;
//						}
//					}
					
//					else if (navigationEntry is IBaseEntity dependentEntry)
//					{
//						dependentEntry.IsDeleted = true;
//					}
//				}
//			}

//			return await this._requestScope.DbContext.SaveChangesAsync() > 0;
//		}

		


     

      

  

//        #endregion


//    }


//    public interface IEFRepository<TEntity,TKey>:IEFRepository<TEntity>
//		where TEntity :class , IBaseEntity<TKey>,new()
//    {
//		Task<TEntity> GetOne(TKey id);
//	     Task<IEnumerable<TEntity>> Get(params Expression<Func<TEntity, bool>>[] predicates);
//		Task<IEnumerable<TEntity>> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, params Expression<Func<TEntity, bool>>[] predicates);
//		Task<IEnumerable<TReturn>> Get<TReturn>(Expression<Func<TEntity, TReturn>> selectExpression);
//		Task<IEnumerable<TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, Expression<Func<TEntity, TReturn>> selectExpression);
//		 Task<IEnumerable<TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
//		Expression<Func<TEntity, TReturn>> selectExpression, params Expression<Func<TEntity, bool>>[] predicate);
//		Task Insert(IEnumerable<TEntity> entities);
//		Task BulkInsert(IEnumerable<TEntity> entities);

//		bool Update(TKey id, TEntity entity);

//		Task<bool> Delete(TKey id, TEntity entity);
//	}


//	public interface IEFRepository<TEntity> : IEFRepository
//		where TEntity : class, IBaseEntity, new()
//	{
//		Task<TEntity> GetOne(params Expression<Func<TEntity, bool>>[] predicates);
//		Task<int> Count(params Expression<Func<TEntity, bool>>[] predicates);
//		Task<bool> Any(params Expression<Func<TEntity, bool>>[] predicates);
//	}

//	public interface IEFRepository
//	{
//		Task<int> Count();
//		Task<bool> Any();
//		Task<bool> Any(object id);

//		Task<bool> SaveChanges();
//	}
//}
