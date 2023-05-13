using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using static AIB.Common.Constants;

namespace AIB.Common
{
    public class Utils
    {
        public static IConfigurationRoot _config { get; set; }
        public static string NewAccessToken { get; set; }

        public static string GetUserId(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.User.GetUserId();
        }


        public static string GetRole(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.User.GetRole();
        }

        public static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Guid.NewGuid().ToString()
                      + Path.GetExtension(fileName);
        }

        public static string GetQueryParams(HttpRequest request)
        {
            string result = "";
            if (!string.IsNullOrEmpty(request.QueryString.Value))
            {
                string q = request.QueryString.Value.Split("=")[1];
                result = HttpUtility.UrlDecode(q);
            }
            return result;
        }

        public static List<string> GetClaimTypes()
        {
            List<string> list = new List<string>();
            list.Add(ClaimType.SalesManagement);
            list.Add(ClaimType.BankAccount);
            list.Add(ClaimType.Company);
            list.Add(ClaimType.Outstandings);
            list.Add(ClaimType.ManageReports);
            list.Add(ClaimType.ManageSetups);
            list.Add(ClaimType.Settings);
            list.Add(ClaimType.Team);
 
            return list;
        }
    }
}
