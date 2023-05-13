
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PanoramaBackend.Services.Data.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace PanoramaBackend.Services.Services
{
    public class CustomerService : BaseService<UserDetails, int>, ICustomerService
    {
        private readonly IServiceProvider _serviceProvider;

        public  CustomerService(IServiceProvider serviceProvider,RequestScope scopeContext, ICustomerRepository repo) 
            
            : base(scopeContext, repo,x=>x.Addresses, x=>x.Attachments)
        {
            //this.IncludeExpressions(x => x.Address, x => x.Attachments).GetAwaiter().GetResult();
            //this.IncludeExpressions(x => x.Address, x => x.Attachments).GetAwaiter().GetResult();
            this.AddNavigation(x => x.Addresses, x => x.Attachments);
            this.AddIncludeExpression(x => x.Include(x => x.Addresses)
            .Include(x=>x.Attachments));

            _serviceProvider = serviceProvider;
        }
        protected override Task WhileInserting(IEnumerable<UserDetails> entities)
        {
            //var _userManager = _serviceProvider.GetRequiredService<UserManager<ExtendedUser>>();
          
            foreach (var item in entities)
            {
        
                item.IsCustomer = true;
            }
            return  base.WhileInserting(entities);

        }
        protected override Task WhileUpdating(IEnumerable<UserDetails> entities)
        {
            foreach (var item in entities)
            {
                item.IsCustomer = true;
            }
            return base.WhileUpdating(entities);
        }
        
    }
    public interface ICustomerService : IBaseService<UserDetails, int>
    {

    }
}
