using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NukesLab.Core.Repository
{
    public interface IRequestInfo
    {
        IConfiguration Configuration { get; }
      //  IHttpContextAccessor HttpContextAccessor { get; }
    }
}
