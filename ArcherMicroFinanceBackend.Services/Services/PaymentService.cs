using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static NukesLab.Core.Common.Constants;
using AutoMapper;
using PanoramBackend.Data;

namespace PanoramBackend.Services.Services
{
    public class PaymentService : BaseService<Payment, int>, IPaymentService
    {
        private readonly ITransactionService _transactionService;
        private readonly IAgentService _agentService;
        private readonly IMapper mapper;
        private readonly ILedgerEntriesService ledgerService;
        private readonly AMFContext _context;
        private readonly IInsuranceCompanyService _insuranceCompanyService;

        public PaymentService(RequestScope scopeContext, IPaymentRepository repo,
            IAgentService agentService,
            ILedgerEntriesService entriesService,
            ITransactionService transactionService,
            IInsuranceCompanyService insuranceCompanyService,
            AMFContext context) : base(scopeContext, repo)
        {
            _transactionService = transactionService;
            this.AddNavigation(x => x.Transactions);
            _agentService = agentService;
            mapper = scopeContext.Mapper;
            ledgerService = entriesService;
            _context = context;
            _insuranceCompanyService = insuranceCompanyService;
        }

        protected override Task WhileInserting(IEnumerable<Payment> entities)
        {
            foreach (var item in entities)
            {

                //item.IsPaymentCredit = false;
                //item.IsPaymentDebit = true;
            }
            return base.WhileInserting(entities);


        }
        protected override Task WhileUpdating(IEnumerable<Payment> entities)
        {
            foreach (var item in entities)
            {

                //item.IsPaymentCredit = false;
                //item.IsPaymentDebit = true;
            }
            return base.WhileUpdating(entities);
        }
        //protected async override Task OnInserted(IEnumerable<Payment> entities)
        //{
        //    var Id = entities.ElementAt(0).Id;
        //    var payment = entities.ElementAt(0);
        //    var transaction = new Transaction();
        //    transaction.Memo = payment.Memo;
        //    transaction.TransactionDate = payment.PaymentDate.Date;
        //    transaction.PaymentId = payment.Id;
        //    transaction.UserDetailId = payment.SalesAgentId;
        //    transaction.TransactionType = TransactionTypes.Payment;
        //    var debitLedger = new LedgarEntries();
        //    debitLedger.DebitAccountId = payment.CreditAccountId;
        //    debitLedger.Amount = payment.Amount;
        //    debitLedger.TransactionDate = payment.PaymentDate;
        //    transaction.LedgarEntries.Add(debitLedger);
        //    var creditLedger = new LedgarEntries();
        //    creditLedger.CreditAccountId = payment.SalesAgent.DefaultAccountId;
        //    creditLedger.Amount = payment.Amount;
        //    creditLedger.TransactionDate = payment.PaymentDate;
        //    transaction.LedgarEntries.Add(creditLedger);
        //   await _transactionService.Insert(new[] { transaction });
        //}
        //protected async override Task OnUpdated(IEnumerable<Payment> entities)
        //{
        //    var Id = entities.ElementAt(0).Id;
        //    //var payment = (await this.Get(x => x.Include(x => x.SalesAgent), x => x.Id == Id)).SingleOrDefault();
        //    var transaction = (await _transactionService.Get(x => x.Include(x => x.LedgarEntries).ThenInclude(x=>x.DebitAccount)
        //    .Include(x => x.LedgarEntries).ThenInclude(x => x.CreditAccount)
        //    .Include(x=>x.Payment), x => x.PaymentId == Id)).SingleOrDefault();
        //    transaction.Memo = transaction.Payment.Memo;
        //    transaction.TransactionDate = transaction.Payment.PaymentDate;
        //    transaction.PaymentId = transaction.Payment.Id;
        //    transaction.UserDetailId = transaction.Payment.SalesAgentId;
        //    transaction.TransactionType = TransactionTypes.Payment;
        //    var debitLedger = transaction.LedgarEntries.Where(x => x.DebitAccountId != null).SingleOrDefault();
        //    debitLedger.DebitAccountId = transaction.Payment.DepositAccountId;
        //    debitLedger.Amount = transaction.Payment.Amount;
        //    debitLedger.TransactionDate = transaction.Payment.PaymentDate;
        //    transaction.LedgarEntries.Add(debitLedger);

        //    //if (debitLedger != null)
        //    //{
        //    //    debitLedger.DebitAccountId = payment.DepositAccountId;
        //    //    debitLedger.Amount = payment.Amount;
        //    //    debitLedger.TransactionDate = payment.PaymentDate;
        //    //    transaction.LedgarEntries.Add(debitLedger);
        //    //}
        //    //else
        //    //{
        //    //    debitLedger = new LedgarEntries();
        //    //    debitLedger.Id = transaction.LedgarEntries.SingleOrDefault(x => x.DebitAccountId == null && x.CreditAccountId == null).Id;
        //    //    debitLedger.DebitAccountId = payment.DepositAccountId;
        //    //    debitLedger.Amount = payment.Amount;
        //    //    debitLedger.TransactionDate = payment.PaymentDate;
        //    //    transaction.LedgarEntries.Add(debitLedger);
        //    //}
        //    var creditLedger = transaction.LedgarEntries.Where(x => x.CreditAccountId != null).SingleOrDefault();
        //    creditLedger.CreditAccountId = transaction.Payment.SalesAgent.DefaultAccountId;
        //    creditLedger.Amount = transaction.Payment.Amount;
        //    creditLedger.TransactionDate = transaction.Payment.PaymentDate;
        //    transaction.LedgarEntries.Add(creditLedger);
        //    await _transactionService.Update(transaction.Id, transaction);
        //}


        public async Task<List<Payment>> GetRecevingPayment(DateTime from, DateTime to)
        {
            if (from.Date != DateTime.MinValue && to.Date != DateTime.MinValue)
            {
                var payments = (await this.Get(x => x.Include(x => x.SalesAgent).Include
                (x => x.InsuranceCompany).Include(x => x.DepositAccount).Include(x => x.PaymentMethod), x => (x.PaymentDate.Date >= from.Date && x.PaymentDate.Date <= to.Date) && x.IsPaymentDebit));
                OtherConstants.isSuccessful = true;
                return payments.ToList();
            }
            else
            {
                var payments = (await this.Get(x => x.Include(x => x.SalesAgent).Include
                (x => x.InsuranceCompany).Include(x => x.DepositAccount).Include(x => x.PaymentMethod), x => x.IsPaymentDebit)).ToList();
                OtherConstants.isSuccessful = true;
                return payments;
            }
       
        }

        public async Task<List<Payment>> GetSentPayment(DateTime from, DateTime to)
        {
            if (from.Date != DateTime.MinValue && to.Date != DateTime.MinValue)
            {
                var payments = (await this.Get(x=> x.Include(x=>x.SalesAgent).Include
                (x=>x.InsuranceCompany).Include(x=>x.CreditAccount).Include(x=>x.PaymentMethod),x =>  (x.PaymentDate.Date >= from.Date && x.PaymentDate.Date <= to.Date) && x.IsPaymentCredit)).ToList();
                OtherConstants.isSuccessful = true;
                return payments;
            }
            else
            {
                var payments =  await this.Get(x => x.Include(x => x.SalesAgent).Include
                (x => x.InsuranceCompany).Include(x => x.CreditAccount).Include(x => x.PaymentMethod), x => x.IsPaymentCredit);
                OtherConstants.isSuccessful = true;
                return payments.ToList();
            }

        }
        public async Task<bool> ReceivePayment(Payment payment)
        {

            var transaction = new Transaction();
            transaction.TransactionDate = payment.PaymentDate;
            transaction.TransactionType = TransactionTypes.Payment;
            transaction.Memo = payment.Memo;
            
            LedgarEntries DebitledgerEntry = new LedgarEntries();
            DebitledgerEntry.Amount = payment.Amount;
            DebitledgerEntry.DebitAccountId = payment.DepositAccountId; //Cash Debits
            DebitledgerEntry.TransactionDate = payment.PaymentDate;
            transaction.LedgarEntries.Add(DebitledgerEntry);
            LedgarEntries CreditLedgerEntry = new LedgarEntries();
            
            var salesAgent = await _agentService.GetOne(Convert.ToInt32(payment.SalesAgentId));
            CreditLedgerEntry.CreditAccountId = salesAgent.DefaultAccountId; //Accounts Receviable Credits
            CreditLedgerEntry.TransactionDate = payment.PaymentDate;
            CreditLedgerEntry.Amount = - payment.Amount;
            transaction.LedgarEntries.Add(CreditLedgerEntry);
           
            payment.Transactions.Add(transaction);

            payment.IsPaymentDebit = true;
            payment.IsPaymentCredit = false;
            var result =await this.Insert(new[] { payment });
            if (result.Success)
            {
                OtherConstants.isSuccessful = true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
            }
            return result.Success;
        }


        public async Task<bool> UpdateReceviedPayment(int Id, Payment payment)
        {
            var Dbtran = await _context.Database.BeginTransactionAsync();
            var oldPayment = await this.GetOne(Id);
           var newPayment=  mapper.Map(payment,oldPayment );
            var transaction = (await _transactionService.Get(x=>x.Include(x=>x.LedgarEntries),x => x.PaymentId == Id)).SingleOrDefault();
            transaction.TransactionDate = newPayment.PaymentDate;
            transaction.TransactionType = TransactionTypes.Payment;
            transaction.Memo = newPayment.Memo;

            LedgarEntries DebitledgerEntry = transaction.LedgarEntries.SingleOrDefault(x => x.DebitAccountId != null);
            DebitledgerEntry.Amount = newPayment.Amount;
            DebitledgerEntry.DebitAccountId = newPayment.DepositAccountId; //Cash Debits
            DebitledgerEntry.TransactionDate = newPayment.PaymentDate;

            LedgarEntries CreditLedgerEntry = transaction.LedgarEntries.SingleOrDefault(x => x.CreditAccountId != null);

            var salesAgent = await _agentService.GetOne(Convert.ToInt32(payment.SalesAgentId));
            CreditLedgerEntry.CreditAccountId = salesAgent.DefaultAccountId; //Accounts Receviable Credits
            CreditLedgerEntry.TransactionDate = newPayment.PaymentDate;
            CreditLedgerEntry.Amount = - newPayment.Amount;

            
             _context.Set<LedgarEntries>().Update(DebitledgerEntry).State = EntityState.Modified;
            var preInitialResult1 = await _context.SaveChangesAsync()>0;

            if (preInitialResult1)
            {
                _context.Set<LedgarEntries>().Update(CreditLedgerEntry).State = EntityState.Modified;
                var preInitialResult2 = await _context.SaveChangesAsync() > 0;
                if (preInitialResult2)
                {
                    _context.Set<Transaction>().Update(transaction).State = EntityState.Modified;
                    var initalResult = await _context.SaveChangesAsync() > 0;
                    if (initalResult)
                    {
                        newPayment.IsPaymentDebit = true;
                        newPayment.IsPaymentCredit = false;
                        _context.Set<Payment>().Update(newPayment).State = EntityState.Modified;
                        var result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            OtherConstants.isSuccessful = true;
                            Dbtran.Commit();
                            return result;
                        }
                        else
                        {
                            OtherConstants.isSuccessful = false;
                            Dbtran.Rollback();
                            return result;
                        }

                    }
                    else
                    {
                        Dbtran.Rollback();
                    }
                    return false;
                }

                else
                {
                    Dbtran.Rollback();
                    return false;
                }
            }
            else
            {
                Dbtran.Rollback();
                return false;
            }
          
            //transaction.LedgarEntries.Add(CreditLedgerEntry);
            //newPayment.Transactions.Add(transaction);
          
        }

        public async Task<bool> DeleteReceviedPayment(int Id)
        {
            var Dbtran = await _context.Database.BeginTransactionAsync();
            var Payment = await this.GetOne(Id);
            var transaction = (await _transactionService.Get(x => x.Include(x => x.LedgarEntries), x => x.PaymentId == Id)).SingleOrDefault();
            foreach (var item in transaction.LedgarEntries)
            {
                item.IsDeleted = true;
                _context.Set<LedgarEntries>().Update(item).State = EntityState.Modified;
                transaction.LedgarEntries.Remove(item);
            }
            transaction.IsDeleted = true;
            _context.Set<Transaction>().Update(transaction).State = EntityState.Modified;

           
            var preInitialResult1 = await _context.SaveChangesAsync() > 0;
            if (preInitialResult1)
            {
                Payment.IsDeleted = true;
                _context.Set<Payment>().Update(Payment).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync() > 0;
                if (result)
                {
                    OtherConstants.isSuccessful = true;
                    await Dbtran.CommitAsync();
                    return result;
                }
                else
                {
                    OtherConstants.isSuccessful = false;
                    await Dbtran.RollbackAsync();
                    return result;
                }
            }
            else
            {
                await Dbtran.RollbackAsync();
                return preInitialResult1;
            }
           
            



        }


        public async Task<bool> SendPayment(Payment payment)
        {

            var transaction = new Transaction();
            transaction.TransactionDate = payment.PaymentDate;
            transaction.TransactionType = TransactionTypes.Payment;
            transaction.Memo = payment.Memo;
            var insuranceCompany = await _insuranceCompanyService.GetOne(Convert.ToInt32(payment.InsuranceCompanyId));
            LedgarEntries DebitledgerEntry = new LedgarEntries();


            DebitledgerEntry.DebitAccountId = insuranceCompany.DefaultAccountId; // Payable Debits 
            DebitledgerEntry.TransactionDate = payment.PaymentDate;
            DebitledgerEntry.Amount = - payment.Amount;
            transaction.LedgarEntries.Add(DebitledgerEntry);

            LedgarEntries CreditLedgerEntry = new LedgarEntries();
            CreditLedgerEntry.Amount = - payment.Amount;
            CreditLedgerEntry.CreditAccountId = payment.CreditAccountId; //Cash Credits
            CreditLedgerEntry.TransactionDate = payment.PaymentDate;
            transaction.LedgarEntries.Add(CreditLedgerEntry);
         
            payment.Transactions.Add(transaction);

            payment.IsPaymentDebit = false;
            payment.IsPaymentCredit = true;
            var result = await this.Insert(new[] { payment });
            if (result.Success)
            {
                OtherConstants.isSuccessful = true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
            }
            return result.Success;
        }


        public async Task<bool> UpdateSendPayment(int Id, Payment payment)
        {
            var Dbtran = await _context.Database.BeginTransactionAsync();
            var oldPayment = await this.GetOne(Id);
            var newPayment = mapper.Map(payment, oldPayment);
            var transaction = (await _transactionService.Get(x => x.Include(x => x.LedgarEntries), x => x.PaymentId == Id)).SingleOrDefault();
            transaction.TransactionDate = newPayment.PaymentDate;
            transaction.TransactionType = TransactionTypes.Payment;
            transaction.Memo = newPayment.Memo;
            var insuranceCompany = await _insuranceCompanyService.GetOne(Convert.ToInt32(payment.InsuranceCompanyId));
            LedgarEntries CreditLedgerEntry = transaction.LedgarEntries.SingleOrDefault(x => x.CreditAccountId != null);
            CreditLedgerEntry.Amount = - payment.Amount;
            CreditLedgerEntry.CreditAccountId = payment.CreditAccountId; //Cash Credits
            CreditLedgerEntry.TransactionDate = payment.PaymentDate;
            transaction.LedgarEntries.Add(CreditLedgerEntry);


            LedgarEntries DebitledgerEntry = transaction.LedgarEntries.SingleOrDefault(x => x.DebitAccountId != null);
            DebitledgerEntry.DebitAccountId = insuranceCompany.DefaultAccountId; //Accounts Payable Debits
            DebitledgerEntry.TransactionDate = payment.PaymentDate;
            DebitledgerEntry.Amount = - payment.Amount;
            transaction.LedgarEntries.Add(DebitledgerEntry);



            _context.Set<LedgarEntries>().Update(DebitledgerEntry).State = EntityState.Modified;
            var preInitialResult1 = await _context.SaveChangesAsync() > 0;

            if (preInitialResult1)
            {
                _context.Set<LedgarEntries>().Update(CreditLedgerEntry).State = EntityState.Modified;
                var preInitialResult2 = await _context.SaveChangesAsync() > 0;
                if (preInitialResult2)
                {
                    _context.Set<Transaction>().Update(transaction).State = EntityState.Modified;
                    var initalResult = await _context.SaveChangesAsync() > 0;
                    if (initalResult)
                    {
                        newPayment.IsPaymentDebit = false;
                        newPayment.IsPaymentCredit = true;
                        _context.Set<Payment>().Update(newPayment).State = EntityState.Modified;
                        var result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            OtherConstants.isSuccessful = true;
                            Dbtran.Commit();
                            return result;
                        }
                        else
                        {
                            OtherConstants.isSuccessful = false;
                            Dbtran.Rollback();
                            return result;
                        }

                    }
                    else
                    {
                        Dbtran.Rollback();
                    }
                    return false;
                }

                else
                {
                    Dbtran.Rollback();
                    return false;
                }
            }
            else
            {
                Dbtran.Rollback();
                return false;
            }

            //transaction.LedgarEntries.Add(CreditLedgerEntry);
            //newPayment.Transactions.Add(transaction);

        }

        public async Task<bool> DeleteSendPayment(int Id)
        {
            var Dbtran = await _context.Database.BeginTransactionAsync();
            var Payment = await this.GetOne(Id);
            var transaction = (await _transactionService.Get(x => x.Include(x => x.LedgarEntries), x => x.PaymentId == Id)).SingleOrDefault();
            foreach (var item in transaction.LedgarEntries)
            {
                item.IsDeleted = true;
                _context.Set<LedgarEntries>().Update(item).State = EntityState.Modified;
                transaction.LedgarEntries.Remove(item);
            }
            transaction.IsDeleted = true;
            _context.Set<Transaction>().Update(transaction).State = EntityState.Modified;


            var preInitialResult1 = await _context.SaveChangesAsync() > 0;
            if (preInitialResult1)
            {
                Payment.IsDeleted = true;
                _context.Set<Payment>().Update(Payment).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync() > 0;
                if (result)
                {
                    OtherConstants.isSuccessful = true;
                    await Dbtran.CommitAsync();
                    return result;
                }
                else
                {
                    OtherConstants.isSuccessful = false;
                    await Dbtran.RollbackAsync();
                    return result;
                }
            }
            else
            {
                await Dbtran.RollbackAsync();
                return preInitialResult1;
            }





        }

    }
    public interface IPaymentService : IBaseService<Payment, int>
    {
        Task<List<Payment>> GetRecevingPayment(DateTime from , DateTime to);
        Task<List<Payment>> GetSentPayment(DateTime from, DateTime to);
        Task<bool> ReceivePayment(Payment payment);
        Task<bool> UpdateReceviedPayment(int Id, Payment payment);
        Task<bool> DeleteReceviedPayment(int Id);
        Task<bool> SendPayment(Payment payment);
        Task<bool> UpdateSendPayment(int Id, Payment payment);
        Task<bool> DeleteSendPayment(int Id);

    }
}
