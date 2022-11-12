using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NukesLab.Core.Common;
using NukesLab.Core.Repository;
using PanoramBackend.Data.CatalogDb.Repos;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace PanoramBackend.Data.CatalogDb
{
   
    public class CatalogUser : IdentityUser
    {

    }
    public class CatalogRoles : IdentityRole
    {

    }
    public partial class CatalogDbContext : IdentityDbContext<CatalogUser,CatalogRoles,string>

    {
        private readonly IServiceProvider _serviceProvider;
        private ModelBuilder _modelBuilder;
        public virtual DbSet<Tenants> Tenants { get; set; }
        public virtual DbSet<Databases> Databases { get; set; }
        public virtual DbSet<ElasticPools> ElasticPools { get; set; }
        public virtual DbSet<Servers> Servers { get; set; }
        public virtual DbSet<SubscriptionPlans> SubscriptionPlans { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<StripeConfigurations> StripeConfiguration { get; set; }
        public virtual DbSet<PaymentInfo> PaymentInfo { get; set; }
        public virtual DbSet<BillingPlan> BillingPlan { get; set; }



        public CatalogDbContext(DbContextOptions<CatalogDbContext> options, IServiceProvider serviceProvider) :
          base(options)
        {
            _serviceProvider = serviceProvider;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            optionsBuilder.UseSqlServer(ConnectionStrings.PortalConnectionString);

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
            modelBuilder.Entity<Tenants>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Tenants__2E9B47E15565CFCB");

                entity.HasIndex(e => e.TenantName)
                    .HasName("IX_Tenants_TenantName");

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.ServicePlan)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDefaultValueSql("'standard'");

                entity.Property(e => e.TenantName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RecoveryState)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastUpdated)
                    .IsRequired();
            });

            modelBuilder.Entity<Databases>(entity =>
            {
                entity.HasKey(e => new { e.ServerName, e.DatabaseName });

                entity.Property(e => e.ServerName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.DatabaseName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.ServiceObjective)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ElasticPoolName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.RecoveryState)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastUpdated)
                    .IsRequired();
            });

            modelBuilder.Entity<ElasticPools>(entity =>
            {
                entity.HasKey(e => new { e.ServerName, e.ElasticPoolName });
                entity.Property(e => e.Dtu)
                    .IsRequired();
                entity.Property(e => e.Edition)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.Property(e => e.DatabaseDtuMax)
                    .IsRequired();
                entity.Property(e => e.DatabaseDtuMin)
                    .IsRequired();
                entity.Property(e => e.StorageMB)
                    .IsRequired();
                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(30);
                entity.Property(e => e.RecoveryState)
                    .IsRequired()
                    .HasMaxLength(30);
                entity.Property(e => e.LastUpdated)
                    .IsRequired();
            });

            modelBuilder.Entity<Servers>(entity =>
            {
                entity.HasKey(e => e.ServerName);
                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(30);
                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(30);
                entity.Property(e => e.RecoveryState)
                    .IsRequired()
                    .HasMaxLength(30);
                entity.Property(e => e.LastUpdated)
                    .IsRequired();
            });
        }

		private void SeedTestingData(ModelBuilder modelBuilder)
		{
            //this.SeedData<SubscriptionPlans>(new SubscriptionPlans()
            //{

            //})
            this.SeedData<Company>(new Company() { });
		

		}
        public void SeedData<TEntity>(params TEntity[] data)
        where TEntity : class
        {
            _modelBuilder.Entity<TEntity>().HasData(data);
        }
    }

}
