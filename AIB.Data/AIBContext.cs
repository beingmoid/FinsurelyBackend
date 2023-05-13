using AIB.Common;
using AIB.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NukesLab.Core.Common;
using System;
using IBaseEntity = AIB.Common.IBaseEntity;

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AIB.Data
{
    public class AIBContext:IdentityDbContext<ExtendedUser,ExtendedRole,string>
    {
		private ModelBuilder _modelBuilder;

		public AIBContext(DbContextOptions<AIBContext> options) : base(options)
		{
			Database.SetCommandTimeout(120000);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			_modelBuilder = modelBuilder;

			InitializeEntities();
			SeedStaticData(modelBuilder);
			SeedTestingData(modelBuilder);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{

			optionsBuilder.UseSqlServer(ConnectionStrings.AIBConnectionString, builder =>
			{
				builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
				
			});

			optionsBuilder.EnableSensitiveDataLogging();

			base.OnConfiguring(optionsBuilder);
		}
	
		protected void InitializeEntities()
        {
			this.InitializeEntity<VehicleModel>();
			this.InitializeEntity<MotorType>();
			this.InitializeEntity<Company>();
			this.InitializeEntity<Invoice>();
			this.InitializeEntity<BankAccount>();
			this.InitializeEntity<Outstandings>();
			this.InitializeEntity<Sales>();
			this.InitializeEntity<Transaction>();
			this.InitializeEntity<Agent>();
			this.InitializeEntity<Broker>();
			this.InitializeEntity<Expense>();
			this.InitializeEntity<Branch>();



			this.CreateRelation<Company, Outstandings>(x => x.Outstandings, x => x.Company, x => x.CompanyId);
			this.CreateRelation<Company, Transaction>(x => x.Transactions, x => x.Company, x => x.CompanyId);
			this.CreateRelation<Invoice, Sales>(x => x.Sales, x => x.Invoice, x => x.InvoiceId);
			this.CreateRelation<MotorType, Sales>(x => x.Sales, x => x.MotorType, x => x.MotorTypeId);
			this.CreateRelation<VehicleModel, Sales>(x => x.Sales, x => x.VehicleModel, x => x.VehicleModelId);
			this.CreateRelation<Agent, Transaction>(x => x.Transactions, x => x.Agent, x => x.AgentId);
			this.CreateRelation<Agent, Sales>(x => x.Sales, x => x.SalesAgent, x => x.SalesAgentId);
			this.CreateRelation<Agent, Outstandings>(x => x.Outstandings, x => x.Agent, x => x.AgentId);
			this.CreateRelation<Company, Sales>(x => x.Sales, x => x.Company, x => x.CompanyId);
			this.CreateRelation<Sales, Outstandings>(x => x.Outstandings, x => x.Sales, x => x.SalesId);
			this.CreateRelation<Invoice, Outstandings>(x => x.Outstandings, x => x.Invoice, x => x.InvoiceId);
			this.CreateRelation<Expense, Expense>(x => x.Expenses, x => x.ParentExpense, x => x.ExpenseId);
			this.CreateRelation<Expense, Transaction>(x => x.Transactions, x => x.Expense, x => x.ExpenseId);
			this.CreateRelation<Branch,Agent>(x=>x.Agents,x=>x.Branch,x=>x.BranchId);
			this.CreateRelation<Branch, Expense>(x => x.Expenses, x => x.Branch, x => x.BranchId);
			this.CreateRelation<Branch,Sales>(x=>x.Sales,x=>x.Branch,x=>x.BranchId);

			_modelBuilder.Entity<Branch>().HasMany(x => x.Managers)
				.WithOne(x=>x.Branch).HasForeignKey(x => x.BranchId);



		}


		protected EntityTypeBuilder<TEntity> InitializeEntity<TEntity>()
		where TEntity : class, IBaseEntity
		{
			var entityTypeBuilder = _modelBuilder.Entity<TEntity>();

			entityTypeBuilder
				.HasQueryFilter(o => !o.IsDeleted);

			entityTypeBuilder
				.Property(o => o.Timestamp)
				.IsRowVersion();

			return entityTypeBuilder;
		}

		private void SeedTestingData(ModelBuilder modelBuilder)
        {
			//
			const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
			// any guid, but nothing is against to use the same one
			const string ROLE_ID = ADMIN_ID;
			modelBuilder.Entity<ExtendedRole>().HasData(new IdentityRole
			{
				Id = ROLE_ID,
				Name = "Admin",
				NormalizedName = "Admin"
			});

			var hasher = new PasswordHasher<ExtendedUser>();
		
			modelBuilder.Entity<ExtendedUser>().HasData(new ExtendedUser
			{
				Id = ADMIN_ID,
				UserName = "sales1@panoramains.com",
				FirstName = "sales1@panoramains.com",

                MarriageStatus = MarriageStatus.Single,
				TypeOfUser = TypeOfUser.Broker,
				Gender = Gender.Male,
				NormalizedUserName = "sales1@panoramains.com",
				Email = "sales1@panoramains.com",
				NormalizedEmail = "sales1@panoramains.com",
				EmailConfirmed = true,
				PasswordHash = hasher.HashPassword(null, "Ram@ajman2023"),
				SecurityStamp = string.Empty
			});
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
			{
				RoleId = ROLE_ID,
				UserId = ADMIN_ID
			});

			var array=new string[14] { "Dashboard","Branches", "SalesManagement", "BankAccount", "Broker", "Outstandings", "Transaction", "TeamMember", "Statements", "Settings","Agent","VehicleType","MotorType","Expenses" };
            var array2 = new string[4] {"Create","Edit","View","Delete" };

			var list = new List<IdentityRoleClaim<string>>();
			var counter = 1;
            for (int i = 0; i < array.Length; i++)
            {
                foreach (var item in array2)
                {
					var roleClaim = new IdentityRoleClaim<string>();
					roleClaim.Id = counter;
					roleClaim.RoleId = ROLE_ID;
					roleClaim.ClaimType = array[i];
					roleClaim.ClaimValue = item;
					list.Add(roleClaim);
					counter++;
				}
            }
			modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(list);
			


		}

        private void SeedStaticData(ModelBuilder modelBuilder)
        {
         //
        }

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
