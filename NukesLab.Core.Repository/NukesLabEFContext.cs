using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using NukesLab.Core.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using EntityFramework.Exceptions.SqlServer;

namespace NukesLab.Core.Repository
{


    public  class NukesLabEFContext<TUser,TRole> : IdentityDbContext<TUser,TRole,Guid>
		where TUser : IdentityUser<Guid>
		where TRole :IdentityRole<Guid>
	
	{
        private readonly IRequestInfo _requestInfo;
        private readonly IServiceProvider serviceProvider;
        private readonly DbContextOptions _options;
        private readonly string _connectionString;
		private ModelBuilder _modelBuilder;
        public NukesLabEFContext()
        {

        }
        public NukesLabEFContext( DbContextOptions options) : base(options)
        {

            _options = options;

        }
        public NukesLabEFContext(string connectionString)
        {
			_connectionString = connectionString;

		}

        protected NukesLabEFContext( DbContextOptions options, string connectionString, ModelBuilder modelBuilder)
        {
            //_requestInfo = requestInfo;
            _options = options;
            _connectionString = connectionString;
            _modelBuilder = modelBuilder;
        }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(ConnectionStrings.PortalConnectionString);
            optionsBuilder.UseInternalServiceProvider(serviceProvider);
            optionsBuilder.UseExceptionProcessor();
            optionsBuilder.EnableDetailedErrors(true);
            optionsBuilder.EnableSensitiveDataLogging(true);

            base.OnConfiguring(optionsBuilder);
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			_modelBuilder = modelBuilder;
		

			//InitializeEntities();
			//SeedStaticData(modelBuilder);
			//SeedTestingData(modelBuilder);
		}

		//protected abstract void InitializeEntities();

		//protected abstract void SeedStaticData(ModelBuilder modelBuilder);

		//protected abstract void SeedTestingData(ModelBuilder modelBuilder);

		protected EntityTypeBuilder<TEntity> InitializeEntity<TEntity>()
			where TEntity : class, IBaseEntity
		{
			var entityTypeBuilder = _modelBuilder.Entity<TEntity>();

			//CreateRelation<TEntity,AuditEntries<TEntity>>(x=>~.AuditEntries<TEntity>,x=>x.)

			entityTypeBuilder
				.HasQueryFilter(o => !(o.IsDeleted??false));



			entityTypeBuilder
				.Property(o => o.CreateUserId)
			.HasColumnName("CreateUserId");
			entityTypeBuilder
		.Property(o => o.EditUserId).
		HasColumnName("EditUserId");


			//entityTypeBuilder
			//    .Property(o => o.AuditEntries<TEntity>.CreateTime)
			//    .HasValueGenerator<CurrentTimeGener~tor>();


			entityTypeBuilder
				.Property(o => o.Timestamp)
				.IsRowVersion()
				.IsConcurrencyToken(true)
				.ValueGeneratedOnAddOrUpdate();

			return entityTypeBuilder;
		}

		// protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => _connectionString !=null ? _connectionString:optionsBuilder=op /*optionsBuilder.UseSqlServer(_connectionString)*/;


		protected void CreateRelation<TEntity, TRelated>(Expression<Func<TEntity, IEnumerable<TRelated>>> navigationExpressionMany
		, Expression<Func<TRelated, TEntity>> navigationExpressionOne, Expression<Func<TRelated, dynamic>> foreignKeyExpression)
		where TEntity : class, IBaseEntity
		where TRelated : class, IBaseEntity
		{
			_modelBuilder.Entity<TEntity>()
				.HasMany(navigationExpressionMany)
				.WithOne(navigationExpressionOne)
				.HasForeignKey(foreignKeyExpression)
				.OnDelete(DeleteBehavior.Restrict);
		}

		protected void CreateRelation<TEntity, TRelated>(Expression<Func<TEntity, TRelated>> navigationExpressionThis
			, Expression<Func<TRelated, TEntity>> navigationExpressionOne, Expression<Func<TRelated, dynamic>> foreignKeyExpression)
			where TEntity : class, IBaseEntity
			where TRelated : class, IBaseEntity
		{
			_modelBuilder.Entity<TEntity>()
				.HasOne(navigationExpressionThis)
				.WithOne(navigationExpressionOne)
				.HasForeignKey(foreignKeyExpression)
				.OnDelete(DeleteBehavior.Restrict);
		}


		public void SeedData<TEntity>(params TEntity[] data)
			where TEntity : class
		{

			_modelBuilder.Entity<TEntity>().HasData(data);
		}
	}
}
