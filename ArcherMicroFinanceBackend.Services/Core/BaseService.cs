using PanoramBackend.Data;
using PanoramBackend.Services.Validations;
using NukesLab.Core.Common;
using NukesLab.Core.Repository;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using PanoramBackend.Services.Services;
using Nest;

namespace PanoramBackend.Services.Core
{
    public class BaseService : IBaseService
    {
        protected readonly RequestScope ScopeContext;

        public BaseService(RequestScope scopeContext)
        {
            this.ScopeContext = scopeContext;
        }

        public virtual void Dispose()
        {
        }
    }
    public class BaseService<TEntity, TKey> : BaseService, IBaseService<TEntity, TKey>
         where TEntity : class, IBaseEntity<TKey>, new()
    {
        private readonly List<IPropertyValidation<TEntity>> _propertyValidations;
        protected IEFRepository<TEntity, TKey> Repository { get; private set; }
        protected readonly IEnumerable<Expression<Func<TEntity, IEnumerable<IBaseEntity>>>> _childExpressions;
        public IEnumerable<Expression<Func<TEntity, object>>> navigations;
        public Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression;
        public IQueryable<TEntity> _baseQuery { get; set; }
        public BaseService(RequestScope scopeContext, IEFRepository<TEntity, TKey> repository) : base(scopeContext)
        {
            this.Repository = repository;
            if (_propertyValidations == null)
            {
                _propertyValidations = new List<IPropertyValidation<TEntity>>();
                Validation();
            }
            //this._baseQuery = this.Repository._baseQuery;
          
        }
        public BaseService(RequestScope scopeContext, IEFRepository<TEntity, TKey> repository, params Expression<Func<TEntity, IEnumerable<IBaseEntity>>>[] childExpressions)
            : this(scopeContext, repository)
        {
            this._childExpressions = childExpressions;
        }
        public BaseService(RequestScope scopeContext, IEFRepository<TEntity, TKey> repository,  Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression)
          : this(scopeContext, repository)
        {
            this.includeExpression = includeExpression;
        }
        //public BaseService(RequestScope scopeContext, IEFRepository<TEntity, TKey> repository, params Expression<Func<TEntity, object>>[] navigations)
        // : this(scopeContext, repository)
        //{
        //    this.navigations = navigations;
        //}

        public void AddNavigation(params Expression<Func<TEntity, object>>[] navigations)
        {
            this.navigations = navigations;
        }
        public void AddIncludeExpression( Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression)
        {
            this.includeExpression = includeExpression;
        }



        #region Validations
        protected virtual void Validation()
        {

        }

        protected PropertyValidations<TEntity, TProperty> Validate<TProperty>(Expression<Func<TEntity, TProperty>> property,
            string caption = null)
        {
            var result = new PropertyValidations<TEntity, TProperty>(ScopeContext.ServiceProvider, this.Repository, property, ValidationType.None, caption ??
                (property as MemberExpression).Member.Name);
            _propertyValidations.Add(result);
            return result;
        }
        private async Task Validate(TEntity entity)
        {
            List<string> error = new List<string>();
            await Task.WhenAll(_propertyValidations.Select(async validator =>
            {
                var validateResult = await validator.Validate(entity);
                if (!validateResult.valid)
                {
                    error.Add(validateResult.error);
                }

            }));

            if (error.Count > 0)
            {
                throw new ServiceException(error.ToArray());
            }
        }
        #endregion

        #region Triggers
        protected virtual Task WhileInserting(IEnumerable<TEntity> entities) { return Task.FromResult(entities); }
        protected virtual Task OnInserted(IEnumerable<TEntity> entities) { return Task.FromResult(entities); }
        protected virtual Task WhileUpdating(IEnumerable<TEntity> entities) { return Task.FromResult(entities); }
        protected virtual Task OnUpdated(IEnumerable<TEntity> entities) { return Task.FromResult(entities); }
        protected virtual Task WhileDeleting(IEnumerable<TEntity> entities) { return Task.FromResult(entities); }
        protected virtual Task OnDeleted(IEnumerable<TEntity> entities) { return Task.FromResult(entities); }
        #endregion

        #region Get
        public async Task<IEnumerable<TEntity>> Get()
        {
            return await this.Repository.Get();
        }
        public async Task<IEnumerable<TEntity>> Get(params Expression<Func<TEntity, bool>>[] predicate)
        {
            return await this.Repository.Get(predicate);
        }
        public async Task<IEnumerable<TEntity>> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, params Expression<Func<TEntity, bool>>[] predicates)
        {
            return await this.Repository.Get(includeExpression, predicates);
        }
        public async Task<IEnumerable<TReturn>> Get<TReturn>(Expression<Func<TEntity, TReturn>> selectExpression)
        {
            return await this.Repository.Get(selectExpression);
        }
        public async Task<IEnumerable<TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, Expression<Func<TEntity, TReturn>> selectExpression)
        {
            return await this.Repository.Get(includeExpression, selectExpression);
        }
        public async Task<IEnumerable<TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
            Expression<Func<TEntity, TReturn>> selectExpression, params Expression<Func<TEntity, bool>>[] predicate)
        {
            return await this.Repository.Get(includeExpression, selectExpression, predicate);
        }
        public async Task<TEntity> GetOne(TKey id)
        {
            return await this.Repository.GetOne(id);
        }
        #endregion

        public async Task<(IEnumerable<TEntity> Entities, bool Success)> Insert(IEnumerable<TEntity> entities)
        {
            try
            {
                var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200/"));
                var elasticClient = new ElasticClient(connectionSettings);
                ConcurrentBag<TEntity> newEntities = new ConcurrentBag<TEntity>();
                var newEntity = new TEntity();
                await Task.WhenAll(entities.Select(async entity =>
                {
                 
                    this.Map(entity, newEntity);
                    newEntities.Add(newEntity);
                }));
                await this.WhileInserting(newEntities);
                await this.Repository.Insert(newEntities);

                //elasticClient.CreateDocument(newEntity);

                var result = await this.Repository.SaveChanges() ? (newEntities.ToList(), true) : (null, false);
                await this.OnInserted(newEntities);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
            

        }

        public async Task<(IEnumerable<TEntity> Entities, bool Success)> BulkInsert(IEnumerable<TEntity> entities)
        {
            var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200/"));
            var elasticClient = new ElasticClient(connectionSettings);
            ConcurrentBag<TEntity> newEntities = new ConcurrentBag<TEntity>();
            await Task.WhenAll(
                entities.Select(async entity =>
                {
                    var newEntity = new TEntity();
                    this.Map(entity, newEntity);
                    newEntities.Add(newEntity);
                     elasticClient.CreateDocument(newEntity);
                }));

            await this.WhileInserting(newEntities);
            await Repository.BulkInsert(newEntities);
        
            var result = (newEntities.ToList(), true);
            await OnInserted(newEntities);
            return result;
        }

        public async Task<(TEntity Entity, bool Success)> Update(TKey id, TEntity entity)
        {
            var dbEntity = await this.Repository.GetOne(id);
            this.Map(entity, dbEntity);

            await this.WhileUpdating(new[] { dbEntity });

            if (navigations != null)
            {
                if (includeExpression != null)
                {
                    var resul1 = await this.Repository.Update(id, dbEntity, includeExpression, this.navigations.ToArray());
                    await this.OnUpdated(new[] { dbEntity });
                    return (dbEntity, resul1);
                }
                else
                {
                    var result2 = await this.Repository.Update(id, dbEntity, null, this.navigations.ToArray());
                    await this.OnUpdated(new[] { dbEntity });

                    return (dbEntity, result2); ;

                }
            }
            else
            {
                var result = await this.Repository.Update(id, dbEntity) & await this.Repository.SaveChanges();
                await this.OnUpdated(new[] { dbEntity });
                return (dbEntity,result);
            }
          
      
        }
        

        
        public async Task<bool> Delete(TKey id)
        {
            var dbEntity = await this.Repository.GetOne(id);
            await this.WhileDeleting(new[] { dbEntity });
            if (navigations != null)
            {
                if (includeExpression!=null)
                {
                    var result = await this.Repository.Delete(id, dbEntity, includeExpression, this.navigations.ToArray()) & await this.Repository.SaveChanges();
                    await this.OnDeleted(new[] { dbEntity });
                    return result;
                }
                else
                {
                    var result = await this.Repository.Delete(id, dbEntity, null, this.navigations.ToArray()) & await this.Repository.SaveChanges();
                    await this.OnDeleted(new[] { dbEntity });
                    return result;

                }
           
            }
            else
            {
                var result = await this.Repository.Delete(id, dbEntity) & await this.Repository.SaveChanges();
                await this.OnDeleted(new[] { dbEntity });
                return result;
            }
       
        }


        public async Task<List<TEntity>> GetPaginated(int page, int pageSize) 
        {
            var items = (await this.Repository.Get()).Skip(page * pageSize).Take(pageSize).ToList();
            return items;
        }

        #region Mapper
        protected void Map(TEntity source, TEntity dest)
        {
            ScopeContext.Mapper.Map(source, dest);

            if (this._childExpressions is null) return;
            foreach (var child in this._childExpressions)
            {
                var func = child.Compile();
                var destLists = func(dest);
                var sourceLists = func(source);
                var toDelete = new List<IBaseEntity>();
                // Delete
                foreach (IBaseEntity destChild in destLists)
                {
                    if (!sourceLists.OfType<IBaseEntity>().Any(o => o.Id.Equals(destChild.Id)))
                    {
                        // Remove
                        destChild.IsDeleted = true;
                    }
                }

                // Add/Modify
                foreach (IBaseEntity sourceChild in sourceLists)
                {
                    IBaseEntity destChild;
                    destChild = destLists.OfType<IBaseEntity>().FirstOrDefault(o => o.JsonCompare(sourceChild));

                    if (destChild == null)
                    {
                        destChild = Activator.CreateInstance(destLists.GetType().GetGenericArguments()[0]) as IBaseEntity;
                        destLists.GetType().GetMethod("Add").Invoke(destLists, new[] { destChild });
                    }

                    ScopeContext.Mapper.Map(sourceChild, destChild);
                }
            }
        }
        #endregion

        public override void Dispose()
        {
            
            base.Dispose();
        }
    }
    public interface IBaseService : IDisposable
    {

    }
    public interface IBaseService<TEntity, TKey> : IBaseService
        where TEntity : class, IBaseEntity<TKey>, new()
    {
        #region Get
        Task<IEnumerable<TEntity>> Get();
        Task<TEntity> GetOne(TKey id);
        Task<IEnumerable<TEntity>> Get(params Expression<Func<TEntity, bool>>[] predicates);
        Task<IEnumerable<TEntity>> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, params Expression<Func<TEntity, bool>>[] predicates);
        Task<IEnumerable<TReturn>> Get<TReturn>(Expression<Func<TEntity, TReturn>> selectExpression);
        Task<IEnumerable<TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, Expression<Func<TEntity, TReturn>> selectExpression);
        Task<IEnumerable<TReturn>> Get<TReturn>(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
        Expression<Func<TEntity, TReturn>> selectExpression, params Expression<Func<TEntity, bool>>[] predicate);
        Task<List<TEntity>> GetPaginated(int page, int pageSize);
        #endregion
            #region Query
        void AddNavigation(params Expression<Func<TEntity, object>>[] navigations);
        IQueryable<TEntity> _baseQuery { get; set; }

        #endregion
        Task<(IEnumerable<TEntity> Entities, bool Success)> Insert(IEnumerable<TEntity> entities);
        Task<(IEnumerable<TEntity> Entities, bool Success)> BulkInsert(IEnumerable<TEntity> entities);
        Task<(TEntity Entity, bool Success)> Update(TKey id, TEntity entity);
        Task<bool> Delete(TKey id);

    }
    public static class ExtentionMethod
    {

        public static bool JsonCompare(this object obj, object another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            if (obj.GetType() != another.GetType()) return false;

            var objJson = JsonConvert.SerializeObject(obj);
            var anotherJson = JsonConvert.SerializeObject(another);

            return objJson == anotherJson;
        }
    }
}
