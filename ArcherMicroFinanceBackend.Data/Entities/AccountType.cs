using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class AccountType:BaseEntity<int>
    {
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Type { get; set; }




        private ICollection<AccountDetailType> _accountDetailTypes;
        public ICollection<AccountDetailType> AccountDetailTypes => _accountDetailTypes ?? (_accountDetailTypes = new List<AccountDetailType>());
    }
    public class AccountDetailType:BaseEntity<int>
    {

        public int AccountTypeId { get; set; }
        public AccountType AccountType { get; set; }
        public string Description { get; set; }
        private ICollection<Accounts> _Accounts;
        public ICollection<Accounts> Accounts => _Accounts ?? (_Accounts = new List<Accounts>());
    }
}
