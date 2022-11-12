using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NukesLab.Core.Repository
{
	public class RequestScope
	{
		public RequestScope(IServiceProvider serviceProvider, ILogger logger, IMapper mapper)
		{
			this.ServiceProvider = serviceProvider;
		
			this.Mapper = mapper;
			this.Logger = logger;
		}

		public IServiceProvider ServiceProvider { get; }

		public IMapper Mapper { get; }
		public ILogger Logger { get; }
	}
	public class RequestScope<Context>:RequestScope 
	
		where Context:IdentityDbContext
	{
		public RequestScope(IServiceProvider serviceProvider, Context context, ILogger logger, IMapper mapper)
			: base(serviceProvider, logger, mapper)
		{
			this.DbContext = context;
		}

		public Context DbContext { get; }
	}
public class GenericContext<TUser, TRole> where TUser : IdentityUser

	where TRole
	:IdentityRole

{
    public IdentityDbContext<TUser,TRole,string> IdentityContext { get;  }
}

}
