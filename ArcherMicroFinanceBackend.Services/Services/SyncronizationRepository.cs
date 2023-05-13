using AIB.Data;
using AIB.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PanoramaBackend.Services;
using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using Transaction = PanoramaBackend.Data.Entities.Transaction;
using Refund = PanoramaBackend.Data.Entities.Refund;
namespace PanoramaBackend.Service.Syncronization
{
    public class SyncronizationRepository : ISyncronizationRepository
    {
        private AMFContext _importContext;
        private AIBContext _exportContext;
        private readonly ISalesInvoiceService _salesInvoiceService;
        private readonly IServiceProvider _services;

        public SyncronizationRepository(AMFContext importContext,
            AIBContext exportContext,
            ISalesInvoiceService salesInvoiceService,
            IServiceProvider
            services
            )
        {
            _importContext = importContext;
            _exportContext = exportContext;
            _salesInvoiceService = salesInvoiceService;
            _services = services;

        }
        public async Task<ISyncResult> MergeAll()
        {
         //     await MergeBranch();
            //_importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[InsuranceType] ON");
            //_importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[SalesInvoice] ON");
            //_importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[UserDetails] ON");
            //_importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[InsuranceType] ON");
            // _importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[Vehicle] ON");
            //_importContext.Database.ExecuteSqlRaw(" DBCC CHECKIDENT('[UserDetails]', RESEED, 0);");
            await this.MergeBanksToAccounts();
            await this.MergeAgentData();
            await this.MergeBrokerData();
            await this.MergeInsuranceTypes();
            await this.MergeVehicles();

            await this.MergeSales();
            //_importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[InsuranceType] OFF");
            //_importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[SalesInvoice] OFF");
            //_importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[UserDetails] OFF");
            //_importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[InsuranceType] OFF");
            //_importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[Vehicle] OFF");
        await this.MergeTransactions();
            return new SyncResult();
        }
        //done
        public async Task<ISyncResult> MergeAgentData()
        {

            this._importContext = this._services.GetRequiredService<AMFContext>();
            using (_importContext)
            {
                ISyncResult result = new SyncResult();
            var existingAgents = _importContext.Set<UserDetails>().Where(x => x.IsAgent == true).ToList();
            var ids = existingAgents.Select(x => x.Id).ToList();
            IList<AIB.Data.Entities.Agent> agents = _exportContext.Set<Agent>().Where(x => x.IsDeleted == false).ToList();

            var NewlyAddedAgents = agents.Where(x => !(ids.Contains(x.Id))).ToList();


            result.TargetRows = agents.Count();
          
                try
                {

                    _importContext.Database.OpenConnection();
                    _importContext.Database.BeginTransaction();

                    var list = new List<UserDetails>();


                    foreach (var item in agents)
                    {
                        var agentAccount = new Accounts();

                        agentAccount.Name = item.Name + " " + "Agent's (A/R)";
                        agentAccount.Description = item.Name + " " + "SYSTEM GENERATED";
                        agentAccount.AccountDetailTypeId = 1; // Account's Receviable
                        agentAccount.Number = Guid.NewGuid().ToString();

                        _importContext.Set<Accounts>().Add(agentAccount);
                        var accountSaving = await _importContext.SaveChangesAsync() > 0;

                        if (accountSaving)
                        {
                            var salesAgent = new UserDetails();
                            salesAgent.Id = item.Id;
                            salesAgent.IsAgent = true;
                            salesAgent.DisplayNameAs = item.Name;

                            //salesAgent.FirstName = item.Name.Split(' ')[0];
                            //salesAgent.LastName = item.Name.Split(' ')[1];
                            salesAgent.Mobile = "+971 52 516 1118";
                            salesAgent.Email = item.Name.Split(' ')[0] + "@gmail.com";
                            salesAgent.DefaultAccountId = agentAccount.Id;
                            _importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [UserDetails] ON");
                            _importContext.Set<UserDetails>().Add(salesAgent);

                        }


                    }


                    //    var query = list.GroupBy(x => x.Id)
                    //.Where(g => g.Count() > 1)
                    //.Select(y => y.Key)
                    //.ToList();



                    await _importContext.SaveChangesAsync();
                    //var finalResult = await _importContext.SaveChangesAsync();

                    _importContext.Database.CommitTransaction();
                    //_importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[UserDetails] OFF");
                    return result;
                }
                catch (Exception ex)
                {
                    _importContext.Database.RollbackTransaction();
                    throw ex;
                }

            }



        }
        //done
        public async Task<ISyncResult> MergeBrokerData()
        {
            this._importContext = this._services.GetRequiredService<AMFContext>();
            using (_importContext)
            {
                ISyncResult result = new SyncResult();
            IList<AIB.Data.Entities.Broker> brokers = _exportContext.Set<Broker>().Where(x => x.IsDeleted == false).ToList();
            result.TargetRows = brokers.Count();
          
                try
                {

                    _importContext.Database.OpenConnection();
                    _importContext.Database.BeginTransaction();
                    //   var execute = _importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[UserDetails] ON");
                    var list = new List<UserDetails>();
                    _importContext.Database.ExecuteSqlRaw(" DBCC CHECKIDENT('[UserDetails]', RESEED, 1000);");
                    foreach (var item in brokers)
                    {
                        var agentAccount = new Accounts();
                        agentAccount.Name = item.Name + " " + "Broker's (A/P)";
                        agentAccount.Description = item.Name + " " + "SYSTEM GENERATED";
                        agentAccount.AccountDetailTypeId = 41; // Account's Receviable
                        agentAccount.Number = Guid.NewGuid().ToString();

                        _importContext.Set<Accounts>().Add(agentAccount);
                        var accountSaving = await _importContext.SaveChangesAsync() > 0;

                        if (accountSaving)
                        {
                            var broker = new UserDetails();
                            broker.Id = item.Id + 1000;
                            broker.IsInsuranceCompany = true;
                            broker.DisplayNameAs = item.Name;
                            broker.Mobile = "+971 52 516 1118";
                            broker.Email = item.Name.Split(' ')[0] + "@gmail.com";
                            broker.DefaultAccountId = agentAccount.Id;
                            _importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [UserDetails] ON");
                            _importContext.Add(broker);
                        }


                    }
                    _importContext.Database.ExecuteSqlRaw(" DBCC CHECKIDENT('[UserDetails]', RESEED, 1000);");
                    await _importContext.SaveChangesAsync();
                    // ..,,var finalResult = await _importContext.SaveChangesAsync();
                    // _importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[UserDetails] OFF");
                    _importContext.Database.CommitTransaction();
                    return result;
                }
                catch (Exception ex)
                {
                    _importContext.Database.RollbackTransaction();
                    throw;
                }
            }



        }
        public async Task<ISyncResult> MergeBranch()
        {
            using (_importContext = this._services.GetRequiredService<AMFContext>())
            {
                ISyncResult result = new SyncResult();
            try
                {
                    _importContext.Database.BeginTransaction();

                    var branches = _exportContext.Set<AIB.Data.Entities.Branch>().ToList();
                    foreach (var item in branches)
                    {
                        var branch = new PanoramaBackend.Data.Entities.Branch();
                        branch.Id = item.Id;
                        branch.BranchName = item.Name;
                        _importContext.Set<PanoramaBackend.Data.Entities.Branch>().Add((branch));

                    }
           
                await _importContext.SaveChangesAsync();
                _importContext.Database.CommitTransaction();
                return result;
            }
            catch (Exception ex)
            {
                _importContext.Database.RollbackTransaction();
           
                throw ex;
            }
            }



        }
  
        //done
        public async Task<ISyncResult> MergeInsuranceTypes()
        {
            using (_importContext)
            {

                this._importContext = this._services.GetRequiredService<AMFContext>();
            this._exportContext=this._services.GetRequiredService<AIBContext>();    
            try
                  
            {
               
              
                    ISyncResult result = new SyncResult();
                var motorTypes = _exportContext.Set<MotorType>().ToList();
                IList<InsuranceType> list = new List<InsuranceType>();
            
                    _importContext.Database.BeginTransaction();
                    _importContext.Database.ExecuteSqlRaw(" DBCC CHECKIDENT('[InsuranceType]', RESEED, 0);");

                    foreach (var item in motorTypes)
                    {
                        var insuranceType = new InsuranceType();
                        insuranceType.Id = item.Id;
                        insuranceType.IsDeleted = item.IsDeleted;
                        insuranceType.Name = item.Name;
                        _importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[InsuranceType] ON");
                        _importContext.Set<InsuranceType>().Add(insuranceType);
                    }
                    await _importContext.SaveChangesAsync();

                    _importContext.Database.CommitTransaction();

                    return result;
                }
           
            catch (Exception e)
            {
                _importContext.Database.RollbackTransaction();
                throw e;
            }
            }
        }
        //done
        public async Task<ISyncResult> MergeVehicles()
        {
            this._importContext = this._services.GetRequiredService<AMFContext>();
            using (_importContext)
            {
                try
                {
               
                    ISyncResult result = new SyncResult();

                    //var existingVehicles = await _importContext.Set<Vehicle>().ToListAsync();
                    //var ids = existingVehicles.Select(x => x.Id).ToList();
                    var vehicles = _exportContext.Set<VehicleModel>().Where(x => x.IsDeleted || !x.IsDeleted).ToList();
                    //var NewlyAddedVehicles = vehicles.Where(x => !(ids.Contains(x.Id))).ToList();
                    //var temp = _exportContext.Set<VehicleModel>().Where(x => x.IsDeleted).ToList();
                    var list = new List<Vehicle>();
                    _importContext.Database.ExecuteSqlRaw(" DBCC CHECKIDENT('[Vehicle]', RESEED, 0);");
                    _importContext.Database.OpenConnection();
                    _importContext.Database.BeginTransaction();

                    //if (NewlyAddedVehicles.Count>0)
                    //{
                    //    foreach (var item in NewlyAddedVehicles)
                    //    {
                    //        var vehicleModel = new Vehicle();
                    //        vehicleModel.Id = item.Id;
                    //        vehicleModel.IsDeleted = item.IsDeleted;
                    //        vehicleModel.Make = item.Name;
                    //        list.Add(vehicleModel);
                    //    }
                    //}
                    //else if(existingVehicles.Count==0)
                    //{
                    //
                    foreach (var item in vehicles)
                    {
                        var vehicleModel = new Vehicle();
                        vehicleModel.Id = item.Id;
                        vehicleModel.IsDeleted = item.IsDeleted;
                        vehicleModel.Make = item.Name;
                        _importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[Vehicle] ON");

                        _importContext.Set<Vehicle>().Add(vehicleModel);
                    }
                    //}
                    //else
                    //{
                    //    return result;
                    //}


                    await _importContext.SaveChangesAsync();

                    _importContext.Database.CommitTransaction();
                          _importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[Vehicle] ON");
                    return new SyncResult();
                }
           
            catch (Exception e)
            {
                _importContext.Database.RollbackTransaction();
                throw e;
            }


            }



        }
        public async Task<ISyncResult> MergeSales()
        {

            _importContext = _services.GetRequiredService<AMFContext>();
            var sales = _exportContext.Set<Sales>().Include(x => x.Company).Where(x => x.IsDeleted == false).ToList();
            IList<SalesInvoice> list = new List<SalesInvoice>();
                    SalesInvoice lastItem = null;
            using (_importContext)
            {
                try
                {
                    _importContext.Database.BeginTransaction();
                    //_importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[SalesInvoice] ON");
            
                    foreach (var item in sales)
                    {

                        var salesInvoice = new SalesInvoice();
                        salesInvoice.Id = item.Id;
                        salesInvoice.PolicyNumber = item.PolicyNumber;
                        salesInvoice.CustomerName = item.CustomerName;
                        salesInvoice.VehilcleId = item.VehicleModelId;
                        salesInvoice.BodyTypeId = 1;
                        salesInvoice.InsuranceTypeId = item.MotorTypeId;
                        salesInvoice.SalesInvoiceDate = item.SalesDate;
                        salesInvoice.Gross = item.PremiumPrice;
                        salesInvoice.Commission = item.Commission;
                        salesInvoice.Net = item.NETPrice;
                        salesInvoice.SalesPrice = item.SalesPrice;
                        salesInvoice.SalesInvoicePersonId = item.SalesAgentId;
                        salesInvoice.InsuranceCompanyId = item.BrokerId + 1000;
                        salesInvoice.BodyTypeId = 1;
                        salesInvoice.InsuranceCompanyName = item.Company.Name;
                        salesInvoice.BranchId = item.BranchId == null? Guid.Empty: (Guid)item.BranchId;
                        salesInvoice.UnderWritter = "SYSTEM MIGRATED";
                        salesInvoice.Notes = "SYSTEM MIGRATED";
                        salesInvoice.PaymentMethodId = 3;

                        lastItem = salesInvoice;
                        list.Add(salesInvoice);
                        _importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[SalesInvoice] ON");
                        _importContext.Set<SalesInvoice>().Add(salesInvoice);
                    }


                    await _importContext.SaveChangesAsync();


                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine("SALES PERSON ID = " + lastItem.SalesInvoicePersonId+
                    //        "\n" +
                    //        "BROKER PERSON ID =" + lastItem.InsuranceCompanyId + 1000

                    //        );
                    //    _importContext.Database.RollbackTransaction();
                    //    throw ex;
                    //}





                    _importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[SalesInvoice] OFF");
                    _importContext.Database.CommitTransaction();
                    await this.OnInserted(list);

                    _importContext.Database.CommitTransaction();


                    return new SyncResult();


                }
                catch (Exception ex)
                {
                    _importContext.Database.RollbackTransaction();
                    throw ex;
                }

            }
            throw new NotImplementedException();
        }


        protected async Task OnInserted(IEnumerable<SalesInvoice> entities)
        {
            try
            {
                _importContext = _services.GetRequiredService<AMFContext>();

                using (_importContext)
                {
                    await _importContext.Database.BeginTransactionAsync();

                    List<PanoramaBackend.Data.Entities.Transaction> li = new List<PanoramaBackend.Data.Entities.Transaction>();

                    foreach (var item in entities)
                    {
                        ////item.IsSupplier = true;
                        //item.Total = item.SaleLineItem.Sum(x=>x.Total);




                        var agent = await _importContext.Set<UserDetails>().SingleOrDefaultAsync(x => x.Id == item.SalesInvoicePersonId

                        &&
                        x.IsAgent == true
                        );
                        var broker = await _importContext.Set<UserDetails>().SingleOrDefaultAsync(x => x.Id == item.InsuranceCompanyId

                      &&
                      x.IsInsuranceCompany == true
                      );
                        ////Making Transaction
                        var transactionForAgent = new Transaction();
                        //transaction.Memo = "Opening Balance";
                        transactionForAgent.TransactionDate = item.SalesInvoiceDate;
                        transactionForAgent.UserDetailId = item.SalesInvoicePersonId;
                        transactionForAgent.SalesInvoiceId = item.Id;
                        transactionForAgent.TransactionType = TransactionTypes.Invoice;


                        //Recording Transaction In Ledger
                        LedgarEntries Debitledgar = new LedgarEntries();
                        Debitledgar.TransactionDate = item.SalesInvoiceDate;
                        Debitledgar.DebitAccountId = agent.DefaultAccountId; //(A/R)
                        Debitledgar.Amount = (decimal)item.SalesPrice;
                        transactionForAgent.LedgarEntries.Add(Debitledgar);
                        var credit_ledger = new LedgarEntries();
                        credit_ledger.TransactionDate = item.SalesInvoiceDate;
                        credit_ledger.CreditAccountId = BuiltinAccounts.SalesAccount; //(Income)
                        credit_ledger.Amount = (decimal)item.SalesPrice;
                        transactionForAgent.LedgarEntries.Add(credit_ledger);
                        //var taxEntry = new LedgarEntries();
                        //taxEntry.TransactionDate = item.SalesInvoiceDate;
                        //taxEntry.CreditAccountId = BuiltinAccounts.VATPayable;
                        //taxEntry.Amount = (decimal)item.SaleLineItem.FirstOrDefault().VAT;
                        //transactionForAgent.LedgarEntries.Add(taxEntry);
                        var transactionForCompany = new Transaction();

                        transactionForCompany.TransactionDate = item.SalesInvoiceDate;
                        transactionForCompany.UserDetailId = item.InsuranceCompanyId;
                        transactionForCompany.SalesInvoiceId = item.Id;
                        transactionForCompany.TransactionType = TransactionTypes.InsuranceCredit;

                        var cCreditLedger = new LedgarEntries();
                        cCreditLedger.TransactionDate = item.SalesInvoiceDate;
                        cCreditLedger.CreditAccountId = broker.DefaultAccountId; //AP
                        cCreditLedger.Amount = ((decimal)item.Net);
                        transactionForCompany.LedgarEntries
                            .Add(cCreditLedger); LedgarEntries Cdebitledger = new LedgarEntries();
                        Cdebitledger.TransactionDate = item.SalesInvoiceDate;
                        Cdebitledger.DebitAccountId = BuiltinAccounts.AccountsPayable; //EX
                        Cdebitledger.Amount = (decimal)item.Net;
                        transactionForCompany.LedgarEntries.Add(Cdebitledger);

                        _importContext.Set<Transaction>().Add(transactionForAgent);
                        _importContext.Set<Transaction>().Add(transactionForCompany);

                    }
                    var result = await _importContext.SaveChangesAsync();


                    _importContext.Database.CommitTransaction();




                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private async Task<bool> MergeBanksToAccounts()
        {
            _importContext = _services.GetRequiredService<AMFContext>();
            _exportContext = _services.GetRequiredService<AIBContext>();

            using (_importContext)
            {
                _importContext.Database.BeginTransaction();
                _importContext.Database.ExecuteSqlRaw(" DBCC CHECKIDENT('[Accounts]', RESEED, 1000);");
                //  await this.MergeAgentData();

                try
                {


                    var banks = _exportContext.Set<AIB.Data.Entities.BankAccount>().ToList();

                    foreach (var item in banks)
                    {
                        var bankAccount = new Accounts();
                        bankAccount.Id = item.Id + 1000;
                        bankAccount.Name = item.BankName;
                        bankAccount.Description = item.BankShortName;
                        bankAccount.Number = item.BankAccountNo + " /" + item.BankIBANNo;
                        bankAccount.AccountDetailTypeId = 14; // Bank
                        _importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[Accounts] ON");
                        _importContext.Set<Accounts>().Add(bankAccount);

                    }
                    _importContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[Accounts] ON");
                    return await _importContext.SaveChangesAsync() > 0 ? true : false;
                }
                catch (Exception EX)
                {
                    _importContext.Database.RollbackTransaction();
                    throw EX;
                }
            }

        }
        private async Task MergePaymentFromAgent()
        {
            this.Dispose();

            var AgentPaidInvoices = _exportContext.Set<AIB.Data.Entities.Transaction>()
                .Where(x => x.AgentId != null && x.BankId != null && x.CompanyId == null ).ToList();

            List<bool> results = new List<bool>();
            var paymentService = _services.GetRequiredService<IPaymentService>();
            _importContext = _services.GetRequiredService<AMFContext>();

            using (_importContext)
            {

                foreach (var item in AgentPaidInvoices)
                {



                    var payment = new Payment();
                    payment.Amount = item.Amount;
                    payment.TransactionReferenceNumber = item.TransactionReferenceNumber;
                    payment.PaymentDate = item.TransactionDate;
                    payment.DepositAccountId = item.BankId
                   ;
                    payment.SalesAgentId = item.AgentId;

                    // var checkResult = await paymentService.SendPayment(payment);
                    try
                    {
                        var checkResult = await paymentService.ReceivePayment(payment);

                    }
                    catch (Exception X)
                    {

                        throw X;
                    }

                }
            }
        }

        private async Task MergePaymentFromBroker()
        {
            var BrokerPayments = _importContext.Set<AIB.Data.Entities.Transaction>()
                .Where(x => x.AgentId == null && x.BankId != null && x.CompanyId != null ).ToList();
            var paymentService = _services.GetRequiredService<IPaymentService>();
            List<bool> results = new List<bool>();



            foreach (var item in BrokerPayments)
            {
                var payment = new Payment();
                payment.Amount = item.Amount;
                payment.TransactionReferenceNumber = item.TransactionReferenceNumber;
                payment.PaymentDate = item.TransactionDate;
                payment.DepositAccountId = item.BankId
               ;
                payment.InsuranceCompanyId = item.AgentId;

                // var checkResult = await paymentService.SendPayment(payment);
                try
                {
                    var checkResult = await paymentService.SendPayment(payment);

                }
                catch (Exception X)
                {

                    throw X;
                }
            }
        }

        public async Task<ISyncResult> MergeTransactions()
        {



            await this.MergePaymentFromAgent();
            await this.MergePaymentFromBroker();
            await this.MergeRefunds();
            return new SyncResult();
        }
        public async Task<ISyncResult> MergeRefunds()
        {

            var refundPayments = _importContext.Set<AIB.Data.Entities.Transaction>()
                .Where(x => x.isRefund != null && x.isRefund == true).ToList();

            var refundService = _services.GetRequiredService<IRefundService>();
            List<bool> results = new List<bool>();

            foreach (var item in refundPayments)
            {
                var refund = new Refund();
                refund.AmountForBroker = (decimal)item.RecevingFromBroker;
                refund.AmountForSalesAgent = (decimal)item.GivenToAgent;
                refund.VehilcleId = item.VehicleModelId;
                refund.InsuranceTypeId = item.MotorTypeId;
                refund.PolicyNumber = item.PolicyNumber;

                try
                {
                    var result = await refundService.Insert(new[] { refund });

                    results.Add(result.Success);


                }
                catch (Exception X)
                {

                    throw X;
                }

            }
            return new SyncResult() { SuccessfulMerged=results.Count() };

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
    public interface ISyncronizationRepository
    {
        Task<ISyncResult> MergeAgentData();
        Task<ISyncResult> MergeBrokerData();
        Task<ISyncResult> MergeVehicles();
        Task<ISyncResult> MergeInsuranceTypes();
        Task<ISyncResult> MergeSales();
        Task<ISyncResult> MergeTransactions();
        Task<ISyncResult> MergeRefunds();
        Task<ISyncResult> MergeAll();
    }
    public interface ISyncResult
    {
        int TargetRows { get; set; }
        bool[] ResultsByEachRow { get; set; }
        int SuccessfulMerged { get; set; }
        int FailuireMerged { get; set; }


    }
    public class SyncResult : ISyncResult
    {
        public int TargetRows { get; set; }
        public bool[] ResultsByEachRow { get; set; }
        public int SuccessfulMerged { get; set; }
        public int FailuireMerged { get; set; }
    }
}
