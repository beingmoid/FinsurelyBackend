using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using static NukesLab.Core.Common.Constants;

namespace NukesLab.Core.Common
{
    public class QueryParamLookup
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class QueryParamsModel
    {
        public QueryParamsModel()
        {
            filter = new List<QueryParamLookup>();
            sort = new List<QueryParamLookup>();
            pageIndex = 1;
            pageSize = 10;
            isFilter = false;
        }
        public List<QueryParamLookup> filter { get; set; }
        public string filterValue { get; set; }
        public List<QueryParamLookup> sort { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public bool isFilter { get; set; }
    }
      
    
    public class Utils
    {
        public static IConfigurationRoot _config { get; set; }
        public static string NewAccessToken { get; set; }

        public static string GetUserId(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.User.GetUserId();
        }

        public static byte[] GetTenantId(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.User.GetTenantId();
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

        public static object GetQueryParamsModel(HttpRequest request)
        {
            QueryParamsModel result = new QueryParamsModel();
            if (!string.IsNullOrEmpty(request.QueryString.Value))
            {
                string q = request.QueryString.Value.Split("=")[1];
                string qp = HttpUtility.UrlDecode(q);
                var res = JsonConvert.DeserializeObject(qp);
                result = JsonConvert.DeserializeObject<QueryParamsModel>(qp);
            }
            return result;
        }

        public static List<string> GetClaimTypes()
        {
            List<string> list = new List<string>();
            list.Add(ClaimType.Contacts);
            list.Add(ClaimType.Task);
            list.Add(ClaimType.Calendar);
            list.Add(ClaimType.Documents);
            list.Add(ClaimType.Accounting);
            list.Add(ClaimType.Cases);
            //list.Add(ClaimType.Reports);
            list.Add(ClaimType.Settings);
            list.Add(ClaimType.Team);
            //list.Add(ClaimType.Analytics);
            //list.Add(ClaimType.History_Case);
            //list.Add(ClaimType.History_Event);
            //list.Add(ClaimType.History_Task);
            //list.Add(ClaimType.History_Contact);
            //list.Add(ClaimType.History_Accounting);
            //list.Add(ClaimType.History_Commission);
            //list.Add(ClaimType.History_Documents);
            return list;
        }
    }
}
