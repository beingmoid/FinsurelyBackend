using AIB.Data.Entities;
using AIB.Data.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Repositories
{
    public class BankAccountRepository:EFRepository<BankAccount,int>,IBankAccountRepository
    {
        public BankAccountRepository(AIBContext requestScope):base(requestScope)
        {

        }
    }
    public interface IBankAccountRepository:IEFRepository<BankAccount,int>
    {

    }
}
