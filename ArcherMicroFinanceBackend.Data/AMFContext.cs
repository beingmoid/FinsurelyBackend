using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NukesLab.Core.Common;
using NukesLab.Core.Repository;
using PanoramaBackend.Data.Entities;
using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Transaction = PanoramBackend.Data.Entities.Transaction;

namespace PanoramBackend.Data
{

	public class AMFContext : NukesLabEFContext<ExtendedUser,ExtendedRole>
    {
        private ModelBuilder _modelBuilder;
		private readonly byte[] tenantId;
		private readonly IServiceProvider _serviceProvider;
        public AMFContext(DbContextOptions<AMFContext> otp) : base(otp)
        {
		
			//tenantId = Utils.GetTenantId(_serviceProvider);
			Database.SetCommandTimeout(120000);
		}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _modelBuilder = modelBuilder;



			InitializeEntities();
            SeedStaticData(modelBuilder);
            SeedTestingData(modelBuilder);
            //foreach (var item in modelBuilder.Model.GetEntityTypes())
            //{
            //    var p = item.FindPrimaryKey().Properties.FirstOrDefault(i => i.ValueGenerated != Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never);
            //    if (p != null)
            //    {
            //        p.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never;
            //    }

            //}
            //_modelBuilder.Entry(entity).Reference(x => x.CreatedBy).IsModified = false;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

           optionsBuilder.UseSqlServer(ConnectionStrings.PortalConnectionString);
			optionsBuilder.UseInternalServiceProvider(_serviceProvider);
            base.OnConfiguring(optionsBuilder); 
        }
        protected  void InitializeEntities()
        {
			//this.InitializeEntity<RCM>();
			
			this.InitializeEntity<AccountType>();
			this.InitializeEntity<AccountDetailType>();
			this.InitializeEntity<Accounts>();
			this.InitializeEntity<Address>();
			this.InitializeEntity<PaymentAndBilling>();
			this.InitializeEntity<Attachments>();
			this.InitializeEntity<PreferredPaymentMethod>();
			this.InitializeEntity<Terms>();
			this.InitializeEntity<PaymentMethod>();
			this.InitializeEntity<SalesInvoice>();
			this.InitializeEntity<SaleLineItem>();
			this.InitializeEntity<InsuranceType>();
			this.InitializeEntity<Vehicle>();
			this.InitializeEntity<PanoramBackend.Data.Entities.Transaction>();
			this.InitializeEntity<LedgarEntries>();
			this.InitializeEntity<Login>();
			this.InitializeEntity<AccountsMapping>();
			this.InitializeEntity<UserDetails>().Ignore(x => x.SalesInvoicePersons)
				.Ignore(x => x.CustomerSalesInvoice)
				.Ignore(x => x.InsuranceCompanyInvoices)
				;
			this.InitializeEntity<ComissionRate>();
			this.InitializeEntity<Documents>();
			this.InitializeEntity<Reconcilation>();
			this.InitializeEntity<Corrections>();

			//EmployeeEntities
			this.InitializeEntity<EmploymentDetails>();
			this.InitializeEntity<Compensation>();
			this.InitializeEntity<VacationPolicy>();
			this.InitializeEntity<BenefitsAndDeduction>();
			this.InitializeEntity<Benefits>();
			this.InitializeEntity<Deduction>();
			this.InitializeEntity<BDType>();
			this.InitializeEntity<EmployeeFiles>();
			this.InitializeEntity<BankDetails>();
			this.InitializeEntity<EmploymentStatus>();
			this.InitializeEntity<StaffOffBoarding>();
			this.InitializeEntity<LeaveApplication>();
			this.InitializeEntity<Teams>();
			this.InitializeEntity<TaskTodo>();
			this.InitializeEntity<Status>();
			this.InitializeEntity<Priority>();
			this.InitializeEntity<Branch>();
			this.InitializeEntity<Payment>();
			this.InitializeEntity<BodyType>();
			this.InitializeEntity<PolicyType>();
			this.InitializeEntity<Service>();
			this.InitializeEntity<Refund>();
			this.InitializeEntity<Expense>();
			this.InitializeEntity<Announcement>();
			this.InitializeEntity<ExpenseCategory>();
			this.InitializeEntity<Payroll>();

			//EmployeeRelations
			this.CreateRelation<UserDetails, Teams>(x => x.ManagerTeams, x => x.Manager, x => x.ManagerId);
			this.CreateRelation<UserDetails, EmploymentDetails>(x => x.ManagerResources, x => x.Manager, x => x.ManagerId);
			this.CreateRelation<UserDetails, EmploymentDetails>(x => x.EmploymentDetails, x => x.UserDetails, x => x.UserDetailId);
			this.CreateRelation<EmploymentDetails, Compensation>(x => x.Compensations, x => x.EmploymentDetails, x => x.EmploymentDetailId);
			this.CreateRelation<EmploymentDetails, VacationPolicy>(x => x.VacationPolicies, x => x.EmploymentDetails, x => x.EmploymentDetailId);
			this.CreateRelation<EmploymentDetails, BenefitsAndDeduction>(x => x.BenefitsAndDeductions, x => x.EmploymentDetails, x => x.EmploymentDetailId);
			this.CreateRelation<BenefitsAndDeduction, Benefits>(x => x.Benefits, x => x.BenefitsAndDeduction, x => x.BenefitAndDeductionId);
			this.CreateRelation<BenefitsAndDeduction, Deduction>(x => x.Deduction, x => x.BenefitsAndDeduction, x => x.BenefitAndDeductionId);
			this.CreateRelation<BDType,Benefits>(x => x.Benefits, x => x.Type, x => x.BenefitTypeId);
			this.CreateRelation<BDType, Deduction>(x => x.Deductions, x => x.Type, x => x.DeductionTypeId);
			this.CreateRelation<EmploymentDetails, EmployeeFiles>(x => x.EmployeeFiles, x => x.EmploymentDetails, x => x.EmploymentDetailId);
			this.CreateRelation<EmploymentDetails, BankDetails>(x => x.BankDetails, x => x.EmploymentDetails, x => x.EmploymentDetailId);
			this.CreateRelation<EmploymentDetails, EmploymentStatus>(x => x.EmploymentStatus, x => x.EmploymentDetails, x => x.EmploymentDetailId);
			this.CreateRelation<EmploymentStatus, StaffOffBoarding>(x => x.StaffOffBoardings, x => x.EmploymentStatus, x => x.EmploymentStatusId);
			this.CreateRelation<EmploymentStatus, LeaveApplication>(x => x.LeaveApplications, x => x.EmploymentStatus, x => x.EmploymentStatusId);
			this.CreateRelation<BDType, BDType>(x => x.ChildernTypes, x => x.Category, x => x.CategoryId);
			this.CreateRelation<Status, TaskTodo>(x => x.TaskTodos, x => x.Status, x => x.StatusId);
			this.CreateRelation<Priority, TaskTodo>(x => x.TaskTodos, x => x.Priority, x => x.PriorityId);
			this.CreateRelation<Priority, TaskTodo>(x => x.TaskTodos, x => x.Priority, x => x.PriorityId);
			this.CreateRelation<UserDetails, TaskTodo>(x => x.AssignedByTask, x => x.AssignedBy, x => x.AssignedById);
			this.CreateRelation<UserDetails, TaskTodo>(x => x.AssignedTask, x => x.AssignedTo, x => x.AssignedToId);
			this.CreateRelation<Branch, Expense>(x => x.ExpensesByBranch, x => x.Branch, x => x.BranchId);

			this.CreateRelation< Branch, Payroll>(x => x.PaidToBranch, x => x.Branch, x => x.BranchId);

			this.CreateRelation<Accounts, Payroll>(x => x.PayrollExpenseAccount, x => x.ExpenseAccount, x => x.ExpenseAccountId);











			this.CreateRelation<AccountType, AccountDetailType>(x => x.AccountDetailTypes, x => x.AccountType, x => x.AccountTypeId);
			this.CreateRelation<AccountDetailType, Accounts>(x => x.Accounts, x => x.AccountDetailType, x => x.AccountDetailTypeId);
			this.CreateRelation<Accounts, Accounts>(x => x.SubAccounts, x => x.Account, x => x.AccountId);
			//this.CreateRelation<Accounts, UserDetails>(x => x.UserDetail, x => x.Accounts, x => x.DefaultAccountId);
			this.CreateRelation<AccountType, AccountDetailType>(x => x.AccountDetailTypes, x => x.AccountType, x => x.AccountTypeId);
			this.CreateRelation<ExtendedUser, UserDetails>(x => x.UserDetails, x => x.ExtendedUser, X => X.UserId);
			this.CreateRelation<UserDetails, UserDetails>(x => x.Parent, x => x.UserDetail, X => X.UserDetailId);
			this.CreateRelation<UserDetails, Address>(x => x.Addresses, x => x.UserDetails, x=>x.UserDetailId);
			this.CreateRelation<UserDetails, PaymentAndBilling>(x => x.PaymentAndBilling, x => x.UserDetails, x => x.UserDetailId);
			this.CreateRelation<UserDetails, Attachments>(x => x.Attachments, x => x.UserDetails, x => x.UserDetailId);
			this.CreateRelation<UserDetails, SalesInvoice>(x => x.CustomerSalesInvoice, x => x.CustomerDetails, x => x.CustomerDetailId);
			this.CreateRelation<UserDetails, SalesInvoice>(x => x.SalesInvoicePersons, x => x.SalesInvoicePerson, x => x.SalesInvoicePersonId);
			this.CreateRelation<UserDetails, Refund>(x => x.AgentRefunds, x => x.Agent, x => x.AgentId);
			this.CreateRelation<UserDetails, Refund>(x => x.InsuranceCompanyRefunds, x => x.InsuranceCompany, x => x.InsuranceCompanyId);
			this.CreateRelation<PaymentMethod, SalesInvoice>(x => x.SalesInvoice, x => x.PaymentMethod, x => x.PaymentMethodId);
		
			this.CreateRelation<PreferredPaymentMethod, PaymentAndBilling>(x => x.PaymentAndBilling, x => x.PreferredPaymentMethod, x => x.PreferredPaymentMethodId);
			this.CreateRelation<Terms, PaymentAndBilling>(x => x.PaymentAndBilling, x => x.Terms, x => x.TermsId);
			this.CreateRelation<SalesInvoice,SaleLineItem >(x => x.SaleLineItem, x => x.SalesInvoice, x => x.SaleId);
			this.CreateRelation<UserDetails, SalesInvoice>(x => x.InsuranceCompanyInvoices, x => x.InsuranceCompany, x => x.InsuranceCompanyId);
			this.CreateRelation<InsuranceType, SalesInvoice>(x => x.SalesInvoice, x => x.InsuranceType, x => x.InsuranceTypeId);
			this.CreateRelation<Vehicle, SaleLineItem>(x => x.SaleLineItem, x => x.Vehicle, x => x.VehilcleId);
			this.CreateRelation<Accounts, AccountsMapping>(x => x.AccountsMappings, x => x.Accounts, x => x.AccountId);
			this.CreateRelation<Accounts, LedgarEntries>(x => x.DebitLedgarEntries, x => x.DebitAccount, x => x.DebitAccountId);
			this.CreateRelation<Accounts, LedgarEntries>(x => x.CreditLedgarEntries, x => x.CreditAccount, x => x.CreditAccountId);
			this.CreateRelation<Accounts, LedgarEntries>(x => x.CreditLedgarEntries, x => x.CreditAccount, x => x.CreditAccountId);
		
			this.CreateRelation<InsuranceType, Refund>(x => x.InsuranceTypeRefunds, x => x.InsuranceType, x => x.InsuranceTypeId);
			this.CreateRelation<Vehicle, Refund>(x => x.RefundsOnVehicles, x => x.Vehicle, x => x.VehilcleId);
			this.CreateRelation<SalesInvoice, Transaction>(x => x.Transactions, x => x.SalesInvoice, x => x.SalesInvoiceId);
			this.CreateRelation<Payment, Transaction>(x => x.Transactions, x => x.Payment, x => x.PaymentId);
			this.CreateRelation<Refund, Transaction>(x => x.Transactions, x => x.Refund, x => x.RefundId);
			this.CreateRelation<UserDetails, Transaction>(x => x.Transactions, x => x.UserDetails, x => x.UserDetailId);
			this.CreateRelation<UserDetails, ComissionRate>(x => x.ComissionRates, x => x.UserDetail, x => x.UserDetailId);
			this.CreateRelation<UserDetails, Payment>(x => x.InsuranceCompanyPayment, x => x.InsuranceCompany, x => x.InsuranceCompanyId);
			this.CreateRelation<Accounts, Payment>(x => x.CreditPayment, x => x.CreditAccount, x => x.CreditAccountId);
			this.CreateRelation<Accounts, Payment>(x => x.DepositPayments, x => x.DepositAccount, x => x.DepositAccountId);
			this.CreateRelation<UserDetails, Reconcilation>(x => x.ReconcilationAgents, x => x.SalesAgent, x => x.SalesAgentId);
			this.CreateRelation<UserDetails, Reconcilation>(x => x.ReconcilationInsuranceCompany, x => x.InsuranceCompany, x => x.InsuranceCompanyId);
			this.CreateRelation<Reconcilation, Corrections>(x => x.Corrections, x => x.Reconcilation, x => x.ReconcilationReportId);
			this.CreateRelation<Documents, Reconcilation>(x => x.ReconcilationInsuranceCompany, x => x.Documents, x => x.DocumentId);
			this.CreateRelation<Branch, SalesInvoice>(x => x.Sales, x => x.Branch, x => x.BranchId);
			this.CreateRelation<BodyType, SalesInvoice>(x => x.SalesInvoice, x => x.BodyType, x => x.BodyTypeId);
			this.CreateRelation<PolicyType, SalesInvoice>(x => x.SalesInvoice, x => x.PolicyType, x => x.PolicyTypeId);
			this.CreateRelation<Service, SalesInvoice>(x => x.SalesInvoice, x => x.Service, x => x.ServiceId);
			this.CreateRelation<Expense, Transaction>(x => x.Transactions, x => x.Expense, x => x.ExpenseId);
            this.CreateRelation<Accounts,Refund>(x => x.Refunds, x => x.Account, x => x.AccountId);
			

        }

		
		//protected EntityTypeBuilder<TEntity> InitializeEntity<TEntity>()
		//where TEntity : class, IBaseEntity
		//{
		//	var entityTypeBuilder = _modelBuilder.Entity<TEntity>();

		//	entityTypeBuilder
		//		.HasQueryFilter(o => !o.IsDeleted);

		//	entityTypeBuilder
		//		.Property(o => o.Timestamp)
		//		.IsRowVersion();

		//	return entityTypeBuilder;
		//}

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
			const string UserRole_Id = "a18be9c0-aa65-4af8-bd17-00bd9344e678";
			modelBuilder.Entity<ExtendedRole>().HasData(new IdentityRole
			{
				Id = UserRole_Id,
				Name = "CompanyAdmin",
				NormalizedName = "CompanyAdmin"
			});

	


			var hasher = new PasswordHasher<ExtendedUser>();
			modelBuilder.Entity<ExtendedUser>().HasData(new ExtendedUser
			{
				Id = ADMIN_ID,
				UserName = "moid",	
				NormalizedUserName = "admin",
				Email = "admin@nukeslab.com",
				NormalizedEmail = "admin@nukeslab.com",
				PhoneNumber="+923400064394",
				EmailConfirmed = true,
				PasswordHash = hasher.HashPassword(null, "Test@0000"),
				SecurityStamp = string.Empty
			});
			modelBuilder.Entity<UserDetails>().HasData(new UserDetails

			{
				Id=1,
			UserId=ADMIN_ID,
			FirstName="Muhamamad",
			MiddleName="Moid",
			LastName="Shams",
			Title="Mr.",
			DisplayNameAs="Moid",
			Company="Systems Limited",
			ImageUrl= "https://pbs.twimg.com/profile_images/633202777695514625/tUVSrLDG.jpg"
			});

			modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
			{
				RoleId = ROLE_ID,
				UserId = ADMIN_ID
			});

			var array = new string[13] { "Dashboard", 
				"Branch", 
				"Sales Agent",
				"Insurance Companies", 
				"Sales",
				"Transactions",  
				"Task",
				"Documents",
				"Expenses" , 
				"Accounting",
				"Workplace" ,
				"Teams", 
				"Reports" };
			var array2 = new string[5] { "Create", "Edit", "View", "Delete","Search" };

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
			//var accountType = new Dictionary<int, string>() { { 1, "Assets" }, { 2, "Liablity" }, { 3, "Expense" }, { 4, "Revenues/Income" }, { 5, "Owner’s equity" } };
			var account = new List<Tuple<string, string,int>>();

			account.Add(Tuple.Create( "Accounts Receivable (A/R)", "Assets", 1));
			account.Add(Tuple.Create("Current Assets", "Assets", 2));
			account.Add(Tuple.Create("Cash and cash equivalents", "Assets", 3));
			account.Add(Tuple.Create("Fixed assets", "Assets", 4));
			account.Add(Tuple.Create("Non-current assetss", "Assets", 5));
			account.Add(Tuple.Create("Accounts Payable (A/P)", "Liablity", 6));
			account.Add(Tuple.Create("Income", "Revenues/Income", 7));
			account.Add(Tuple.Create("Credit Card", "Liablity", 8));
			account.Add(Tuple.Create("Current liabilities", "Liablity", 9));
			account.Add(Tuple.Create("Non-current liabilities", "Liablity", 10));
			account.Add(Tuple.Create("Owners Equity", "Owner’s equity", 11));
			account.Add(Tuple.Create("Other income", "Revenues/Income", 12));
			account.Add(Tuple.Create("Cost of Sales Invoice", "Liablity", 13));
			account.Add(Tuple.Create("Expense", "Expense", 14));
			account.Add(Tuple.Create("Other Expense", "Expense", 15));
			var accountTypeList = new List<AccountType>();


            #region Account Detail Type MetaData

            var accountTypeDetail = new List<AccountDetailType>() { 
			new AccountDetailType()
            {
				Id=1,
				AccountTypeId=1,
				Description="Accounts Receivable (A/R)"
			},
		
					new AccountDetailType()
			{
				Id=2,
				AccountTypeId=2,
				Description="Allowance for bad debits"
			},
								new AccountDetailType()
			{
				Id=3,
				AccountTypeId=2,
				Description="Assets Available for sale"
			},
											new AccountDetailType()
			{
				Id=4,
				AccountTypeId=2,
				Description="Development Cost"
			},          new AccountDetailType()
			{
				Id=5,
				AccountTypeId=2,
				Description="Employee Cash Advances"
			},          new AccountDetailType()
			{
				Id=6,
				AccountTypeId=2,
				Description="Inventory"
			},          new AccountDetailType()
			{
				Id=7,
				AccountTypeId=2,
				Description="Investments - Other"
			},          new AccountDetailType()
			{
				Id=8,
				AccountTypeId=2,
				Description="Loans To Officers"
			},          new AccountDetailType()
			{
				Id=9,
				AccountTypeId=2,
				Description="Loans to Others"
			},          new AccountDetailType()
			{
				Id=10,
				AccountTypeId=2,
				Description="Loans to Shareholders"
			},          new AccountDetailType()
			{
				Id=11,
				AccountTypeId=2,
				Description="Prepaid Expenses"
			},          new AccountDetailType()
			{
				Id=12,
				AccountTypeId=2,
				Description="Retainage"
			},          new AccountDetailType()
			{
				Id=13,
				AccountTypeId=2,
				Description="Undeposited Funds"
			},          new AccountDetailType()
			{
				Id=14,
				AccountTypeId=3,
				Description="Bank"
			},          new AccountDetailType()
			{
				Id=15,
				AccountTypeId=3,
				Description="Cash and cash equivalents"
			},          new AccountDetailType()
			{
				Id=16,
				AccountTypeId=3,
				Description="Cash on hand"
			},          new AccountDetailType()
			{
				Id=17,
				AccountTypeId=3,
				Description="Client trust account"
			},          new AccountDetailType()
			{
				Id=18,
				AccountTypeId=3,
				Description="Money Market"
			},          new AccountDetailType()
			{
				Id=19,
				AccountTypeId=3,
				Description="Rents Held in Trust"
			},          new AccountDetailType()
			{
				Id=20,
				AccountTypeId=3,
				Description="Savings"
			},          new AccountDetailType()
			{
				Id=21,
				AccountTypeId=4,
				Description="Accumulated depletion"
			},          new AccountDetailType()
			{
				Id=22,
				AccountTypeId=4,
				Description="Buildings"
			},          new AccountDetailType()
			{
				Id=23,
				AccountTypeId=4,
				Description="Depletable Assets"
			},          new AccountDetailType()
			{
				Id=24,
				AccountTypeId=4,
				Description="Furniture and Fixtures"
			},                  new AccountDetailType()
			{
				Id=25,
				AccountTypeId=4,
				Description="Leasehold Improvements"
			},          new AccountDetailType()
			{
				Id=26,
				AccountTypeId=4,
				Description="Machinery and equipment"
			},          new AccountDetailType()
			{
				Id=27,
				AccountTypeId=4,
				Description="Other fixed assets"
			},          new AccountDetailType()
			{
				Id=28,
				AccountTypeId=4,
				Description="Vehicles"
			},          new AccountDetailType()
			{
				Id=29,
				AccountTypeId=5,
				Description="Accumulated amortisation of non-current assets"
			},          new AccountDetailType()
			{
				Id=30,
				AccountTypeId=5,
				Description="Assets held for sale"
			},          new AccountDetailType()
			{
				Id=31,
				AccountTypeId=5,
				Description="Deferred tax"
			},          new AccountDetailType()
			{
				Id=32,
				AccountTypeId=5,
				Description="Goodwill"
			},          new AccountDetailType()
			{
				Id=33,
				AccountTypeId=5,
				Description="Intangible Assets"
			},          new AccountDetailType()
			{
				Id=34,
				AccountTypeId=5,
				Description="Lease Buyout"
			},          new AccountDetailType()
			{
				Id=35,
					AccountTypeId=5,
				Description="Licences"
			},          new AccountDetailType()
			{
				Id=37,
					AccountTypeId=5,
				Description="Long-term investments"
			},          new AccountDetailType()
			{
				Id=38,
				AccountTypeId=5,
				Description="Organisational Costs"
			},          new AccountDetailType()
			{
				Id=39,
				AccountTypeId=5,
				Description="Other non-current assets"
			},          new AccountDetailType()
			{
				Id=40,
				AccountTypeId=5,
				Description="Security Deposits"
			},          new AccountDetailType()
			{
				Id=41,
				AccountTypeId=6,
				Description="Accounts Payable (A/P)"
			},          new AccountDetailType()
			{
				Id=42,
				AccountTypeId=7,
				Description="Discounts/Refunds Given"
			},          new AccountDetailType()
			{
				Id=43,
					AccountTypeId=7,
				Description="Non-Profit Income"
			},          new AccountDetailType()
			{
				Id=44,
				AccountTypeId=7,
				Description="Other Primary Income"
			},          new AccountDetailType()
			{
				Id=45,
				AccountTypeId=7,
				Description="SalesInvoice - services"
			},                new AccountDetailType()
			{
				Id=46,
				AccountTypeId=7,
				Description="Unapplied Cash Payment Income"
			},          new AccountDetailType()
			{
				Id=47,
				AccountTypeId=8,
				Description="Credit Card"
			},          new AccountDetailType()
			{
				Id=48,
				AccountTypeId=9,
				Description="Accrued liabilities"
			},          new AccountDetailType()
			{
				Id=49,
				AccountTypeId=9,
				Description="Client Trust Accounts - Liabilities"
			},
   new AccountDetailType()
			{
				Id=50,
				AccountTypeId=9,
				Description="Current Tax Liability"
			},
	  new AccountDetailType()
			{
				Id=51,
				AccountTypeId=9,
				Description="Current portion of obligations under finance leases"
			},
		 new AccountDetailType()
			{
				Id=52,
				AccountTypeId=9,
				Description="Dividends payable"
			},   new AccountDetailType()
			{
				Id=53,
				AccountTypeId=9,
				Description="Income tax payable"
			},   new AccountDetailType()
			{
				Id=54,
				AccountTypeId=9,
				Description="Insurance payable"
			},   new AccountDetailType()
			{
				Id=55,
				AccountTypeId=9,
				Description="Line of Credit"
			},   new AccountDetailType()
			{
				Id=56,
				AccountTypeId=9,
				Description="Loan Payable"
			},   new AccountDetailType()
			{
				Id=57,
				AccountTypeId=9,
				Description="Other current liabilities"
			},   new AccountDetailType()
			{
				Id=58,
				AccountTypeId=9,
				Description="Payroll Clearing"
			},   new AccountDetailType()
			{
				Id=59,
				AccountTypeId=9,
				Description="Payroll liabilities"
			},   new AccountDetailType()
			{
				Id=60,
				AccountTypeId=9,
				Description="Prepaid Expenses Payable"
			},   new AccountDetailType()
			{
				Id=61,
				AccountTypeId=9,
				Description="Rents in trust - Liability"
			},   new AccountDetailType()
			{
				Id=62,
				AccountTypeId=9,
				Description="SalesInvoice and service tax payable"
			},   new AccountDetailType()
			{
				Id=63,
				AccountTypeId=10,
				Description="Accrued holiday payable"
			},   new AccountDetailType()
			{
				Id=64,
				AccountTypeId=10,
				Description="Accrued non-current liabilities"
			},   new AccountDetailType()
			{
				Id=65,
				AccountTypeId=10,
				Description="Liabilities related to assets held for sale"
			},   new AccountDetailType()
			{
				Id=66,
				AccountTypeId=10,
				Description="Long-term debt"
			},   new AccountDetailType()
			{
				Id=67,
				AccountTypeId=10,
				Description="Notes Payable"
			},   new AccountDetailType()
			{
				Id=68,
				AccountTypeId=10,
				Description="Other non-current liabilities"
			},   new AccountDetailType()
			{
				Id=69,
				AccountTypeId=10,
				Description="Shareholder Notes Payable"
			},   new AccountDetailType()
			{
				Id=70,
				AccountTypeId=11,
				Description="Accumulated adjustment"
			},   new AccountDetailType()
			{
				Id=71,
				AccountTypeId=11,
				Description="Dividend disbursed"
			},   new AccountDetailType()
			{
				Id=72,
				AccountTypeId=11,
				Description="Opening Balance Equity"
			},   new AccountDetailType()
			{
				Id=73,
				AccountTypeId=11,
				Description="Ordinary shares"
			},   new AccountDetailType()
			{
				Id=74,
				AccountTypeId=11,
				Description="Other comprehensive income"
			},   new AccountDetailType()
			{
				Id=75,
				AccountTypeId=11,
				Description="Owner's Equity"
			},   new AccountDetailType()
			{
				Id=76,
				AccountTypeId=11,
				Description="Paid-in capital or surplus"
			},   new AccountDetailType()
			{
				Id=77,
				AccountTypeId=11,
				Description="Partner Contributions"
			},   new AccountDetailType()
			{
				Id=78,
				AccountTypeId=11,
				Description="Partner Distributions"
			},   new AccountDetailType()
			{
				Id=79,
				AccountTypeId=11,
				Description="Partner's Equity"
			},   new AccountDetailType()
			{
				Id=80,
				AccountTypeId=11,
				Description="Preferred shares"
			},   new AccountDetailType()
			{
				Id=81,
				AccountTypeId=11,
				Description="Retained Earnings"
			},   new AccountDetailType()
			{
				Id=82,
				AccountTypeId=11,
				Description="Share capital"
			},   new AccountDetailType()
			{
				Id=83,
				AccountTypeId=11,
				Description="Treasury Shares"
			},   new AccountDetailType()
			{
				Id=84,
				AccountTypeId=12,
				Description="Dividend income"
			},   new AccountDetailType()
			{
				Id=85,
					AccountTypeId=12,
				Description="Other Investment Income"
			},   new AccountDetailType()
			{
				Id=86,
						AccountTypeId=12,
				Description="Other Miscellaneous Income"
			},   new AccountDetailType()
			{
				Id=87,
						AccountTypeId=12,
				Description="Other operating income"
			},   new AccountDetailType()
			{
				Id=88,
					AccountTypeId=12,
				Description="Tax-Exempt Interest"
			},   new AccountDetailType()
			{
				Id=89,
						AccountTypeId=12,
				Description="Unrealised loss on securities, net of tax"
			},   new AccountDetailType()
			{
				Id=90,
				AccountTypeId=13,
				Description="Cost of labour - COS"
			},   new AccountDetailType()
			{
				Id=91,
				AccountTypeId=13,
				Description="Equipment rental - COS"
			},   new AccountDetailType()
			{
				Id=92,
				AccountTypeId=13,
				Description="Freight and delivery - COS"
			},   new AccountDetailType()
			{
				Id=93,
			AccountTypeId=13,
				Description="Other costs of SalesInvoice - COS"
			},   new AccountDetailType()
			{
				Id=94,
			AccountTypeId=13,
				Description="Supplies and materials - COS"
			},   new AccountDetailType()
			{
				Id=95,
				AccountTypeId=14,
				Description="Advertising/Promotional"
			},   new AccountDetailType()
			{
				Id=96,
				AccountTypeId=15,
				Description="Amortisation expense"
			},   new AccountDetailType()
			{
				Id=97,
				AccountTypeId=14,
				Description="Auto"
			},   new AccountDetailType()
			{
				Id=98,
					AccountTypeId=14,
				Description="Bad debts"
			},   new AccountDetailType()
			{
				Id=99,
					AccountTypeId=14,
				Description="Bank charges"
			},   new AccountDetailType()
			{
				Id=100,
				AccountTypeId=14,
				Description="Charitable Contributions"
			},   new AccountDetailType()
			{
				Id=101,
				AccountTypeId=14,
				Description="Commissions and fees"
			},   new AccountDetailType()
			{
				Id=102,
				AccountTypeId=14,
				Description="Cost of Labour"
			},   new AccountDetailType()
			{
				Id=103,
				AccountTypeId=14,
				Description="Dues and Subscriptions"
			},   new AccountDetailType()
			{
				Id=104,
			AccountTypeId=14,
				Description="Equipment rental"
			},   new AccountDetailType()
			{
				Id=105,
					AccountTypeId=14,
				Description="Finance costs"
			},   new AccountDetailType()
			{
				Id=106,
			AccountTypeId=14,
				Description="Income tax expense"
			},   new AccountDetailType()
			{
				Id=107,
		AccountTypeId=14,
				Description="Insurance"
			},

		  new AccountDetailType()
			{
				Id=108,
			AccountTypeId=14,
				Description="Interest paid"
			},   new AccountDetailType()
			{
				Id=109,
			AccountTypeId=14,
				Description="Legal and professional fees"
			},   new AccountDetailType()
			{
				Id=110,
		AccountTypeId=14,
				Description="Loss on discontinued operations, net of tax"
			},   new AccountDetailType()
			{
				Id=111,
			AccountTypeId=14,
				Description="Management compensation"
			},   new AccountDetailType()
			{
				Id=112,
				AccountTypeId=14,
				Description="Meals and entertainment"
			},   new AccountDetailType()
			{
				Id=113,
				AccountTypeId=14,
				Description="Office/General Administrative Expenses"
			},   new AccountDetailType()
			{
				Id=114,
				AccountTypeId=14,
				Description="Other Miscellaneous Service Cost"
			},   new AccountDetailType()
			{
				Id=115,
			AccountTypeId=14,
				Description="Payroll Expenses"
			},   new AccountDetailType()
			{
				Id=116,
			AccountTypeId=14,
				Description="Rent or Lease of Buildings"
			},   new AccountDetailType()
			{
				Id=117,
				AccountTypeId=14,
				Description="Repair and maintenance"
			},   new AccountDetailType()
			{
				Id=118,
				AccountTypeId=14,
				Description="Shipping and delivery expense"
			},   new AccountDetailType()
			{
				Id=119,
				AccountTypeId=14,
				Description="Taxes Paid"
			},
		  new AccountDetailType()
				{
				Id=120,
				AccountTypeId=14,
				Description="Travel expenses - general and admin expenses"
			},   new AccountDetailType()
			{
				Id=121,
					AccountTypeId=14,
				Description="Travel expenses - selling expense"
			},   new AccountDetailType()
			{
				Id=122,
					AccountTypeId=14,
				Description="Unapplied Cash Bill Payment Expense"
			},   new AccountDetailType()
			{
				Id=123,
					AccountTypeId=14,
				Description="Utilities"
			},   new AccountDetailType()
			{
				Id=124,
				AccountTypeId=15,
				Description="Amortisation"
			},   new AccountDetailType()
			{
				Id=125,
				AccountTypeId=15,
				Description="Depreciation"
			},   new AccountDetailType()
			{
				Id=126,
				AccountTypeId=15,
				Description="Exchange Gain or Loss"
			},   new AccountDetailType()
			{
				Id=127,
				AccountTypeId=15,
				Description="Other Expense"
			},   new AccountDetailType()
			{
				Id=128,
				AccountTypeId=15,
				Description="Penalties and settlements"
			}
			};

            #endregion
            foreach (var item in account)
			{
				var entity = new AccountType();
				entity.Id = item.Item3;
				entity.Name = item.Item1;
				entity.Type = item.Item2;
				accountTypeList.Add(entity);
			}


			
         


		
			
    //        foreach (var item in account)
    //        {
				//var entity = new Accounts()
				//{
				//	Id = item.Item3,
				//	Name = item.Item1,
				//	AccountTypeId = Convert.ToInt32(item.Item2),

				//};
				//accountsList.Add(entity);
				
    //        }
			modelBuilder.Entity<AccountType>().HasData(accountTypeList);
            modelBuilder.Entity<AccountDetailType>().HasData(accountTypeDetail);
			modelBuilder.Entity<Accounts>().HasData( new List<Accounts>()
			{
				new Accounts(){ Id=1,AccountDetailTypeId= 15,Name="Cash Account" , Description="Asset"},
				new Accounts() { Id = 2, AccountDetailTypeId = 15, Name = "Test Bank Account", Description = "Asset" },
				new Accounts() { Id = 3, AccountDetailTypeId = 1, Name = "Accounts Receivable(A/ R)", Description = "Asset" },
					new Accounts() { Id = 4, AccountDetailTypeId = 41, Name = "Accounts Payable(A/ P)", Description = "Liability" },
						new Accounts() { Id = 5, AccountDetailTypeId = 105, Name = "Expense", Description = "Expense" },
							new Accounts() { Id = 6, AccountDetailTypeId = 45, Name = "Sales Account", Description = "Income" },
								new Accounts() { Id = 7, AccountDetailTypeId = 41, Name = "VAT Payable", Description = "Liablity" },
								new Accounts() { Id = 8, AccountDetailTypeId = 81, Name = "Retained Earning", Description = "Owner's Equity" },
									new Accounts() { Id = 9, AccountDetailTypeId = 81, Name = "Opening Balance Equity", Description = "Owner's Equity" },

			});

			modelBuilder.Entity<PreferredPaymentMethod>().HasData(new List<PreferredPaymentMethod>() { 
			new PreferredPaymentMethod(){ 
			Id=1,
			Text="Cash",

			
			},new PreferredPaymentMethod(){
				Id=2,
			Text="Cheque",


			},
				new PreferredPaymentMethod(){
			Id=3,
			Text="Debit Card",


			},
					new PreferredPaymentMethod(){
			Id=4,
			Text="Credit Card",


			}


			});


			modelBuilder.Entity<Terms>().HasData(new List<Terms>() { new Terms() {

			Id=1,
			Text="Due on recipt"


			},
			 new Terms() {

			Id=2,
			Text="Net 15"


			},
			 new Terms() {

			Id=3,
			Text="Net 30"


			},
			 new Terms() {

			Id=4,
			Text="Net 60"


			},



			});




			modelBuilder.Entity<PaymentMethod>().HasData(new List<PaymentMethod>() { new PaymentMethod(){
			Id=1,
			Name="Cash",


			},new PaymentMethod(){
				Id=2,
			Name="Cheque",


			},
				new PaymentMethod(){
			Id=3,
			Name="Debit Card",


			},
					new PaymentMethod(){
			Id=4,
			Name="Credit Card",


			} });

			modelBuilder.Entity<InsuranceType>().HasData(new List<InsuranceType>() {
			new InsuranceType()
			{
				Id=1,
				Name="Third Party Liability"
			},

			new InsuranceType()
			{
				Id=2,
				Name="Comprehensive"
			},
			});
			modelBuilder.Entity<Vehicle>().HasData(new List<Vehicle>() {
			new Vehicle()
            {
				Id=1,
				Make="Honda Civic",
				Model="2010-2020"
			},
				new Vehicle()
			{
				Id=2,
				Make="Toyota Corolla",
				Model="2010-2020"
			},
						new Vehicle()
			{
				Id=3,
				Make="Mitsubishi Lancer",
				Model="2010-2020"
			},

			});


			modelBuilder.Entity<AccountsMapping>().HasData(new List<AccountsMapping>() {
			
			new AccountsMapping()
            {
				Id=1,
				FormName="Sales Agent",
				
				
            },
			new AccountsMapping()
			{
				Id=2,
				FormName="Sales",


			}
			,
			new AccountsMapping()
			{
				Id=3,
				FormName="Insurance Broker",


			}
			,
			new AccountsMapping()
			{
				Id=4,
				FormName="Transfer",


			}

			});
			modelBuilder.Entity<Status>().HasData(new List<Status>() {
			new Status()
			{
				Id=1,
				Name="completed"
			},
			new Status()
			 {
				Id=2,
				Name="inprocess"
			},
			new Status()
			 {
				Id=3,
				Name="pending"
			},



			});

			modelBuilder.Entity<Priority>().HasData(new List<Priority>() {
			new Priority()
			{
				Id=1,
				Name="low"
			},
			new Priority()
			 {
				Id=2,
				Name="medium"
			},
			new Priority()
			 {
				Id=3,
				Name="high"
			},



			});
			var bdTypes = new List<BDType>()
			{
				new BDType()
			{
				Id=1,
				Name="Deduction For Leave",
				IsForDeduction=true
			}

			};
			modelBuilder.Entity<BDType>().HasData(bdTypes.ToArray());
			var policyTypes = new List<PolicyType>()
			{
				new PolicyType()
			{
				Id=1,
				Name="Motor Insurance",

			},      new PolicyType()
			{
				Id=2,
				Name="Life Insurance",

			}

			};
			modelBuilder.Entity<PolicyType>().HasData(policyTypes.ToArray());
		}

		private void SeedStaticData(ModelBuilder modelBuilder)
		{
			//
		}

		protected void CreateRelation<TEntity, TRelated>(Expression<Func<TEntity, IEnumerable<TRelated>>> navigationExpressionMany
			, Expression<Func<TRelated, TEntity>> navigationExpressionOne, Expression<Func<TRelated, dynamic>> foreignKeyExpression)
			where TEntity : class
			where TRelated : class
		{
			_modelBuilder.Entity<TEntity>()
				.HasMany(navigationExpressionMany)
				.WithOne(navigationExpressionOne)
				.HasForeignKey(foreignKeyExpression)
				.OnDelete(DeleteBehavior.Restrict);
		}

		protected void CreateRelation<TEntity, TRelated>(Expression<Func<TEntity, TRelated>> navigationExpressionThis
			, Expression<Func<TRelated, TEntity>> navigationExpressionOne, Expression<Func<TRelated, dynamic>> foreignKeyExpression)
			where TEntity : class
			where TRelated : class
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
