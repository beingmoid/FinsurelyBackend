using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace NukesLab.Core.Common
{
    public class Constants
    {
        public static class JWTConfiguration
        {
            public static readonly string JWTIssuer = Utils._config["Jwt:Issuer"];
            public static readonly string JWTAudience = Utils._config["Jwt:Audience"];
            public static readonly string JWTKey = Utils._config["Jwt:Key"];
        }
        public static class ElasticSearchConnection
        {
            public static readonly string Connection = Utils._config["http://localhost:9200/:url"];
        }
        public class MappingConfiguration
        {
            public static List<ExcelMapper> Mapper => new List<ExcelMapper>() {
                 new ExcelMapper(){
                    Id=0,
                    ColumnName="none"
                },
                new ExcelMapper(){
                    Id=1,
                    ColumnName="TransactionType"
                },
                                new ExcelMapper(){
                    Id=2,
                    ColumnName="InvoiceNumber"
                },
                                                   new ExcelMapper(){
                    Id=3,
                    ColumnName="PolicyNumber"
                },
               new ExcelMapper(){
                    Id=4,
                    ColumnName="InvoiceDate"
                },
                                                                                   
                new ExcelMapper(){
                    Id=5,
                    ColumnName="InsuranceType"
                },
                     new ExcelMapper(){
                    Id=6,
                    ColumnName="Amount"
                },
                          new ExcelMapper(){
                    Id=7,
                    ColumnName="Debit"
                },
                               new ExcelMapper(){
                    Id=8,
                    ColumnName="Credit"
                }
                               ,
                               new ExcelMapper(){
                    Id=9,
                    ColumnName="Balance"
                },
                                                  new ExcelMapper(){
                    Id=10,
                    ColumnName="Vehicle"
                },
                                                                                 new ExcelMapper(){
                    Id=11,
                    ColumnName="Name"
                },
    new ExcelMapper()
                        {
        Id=12,
        ColumnName="TransactionRefNumber"
      },
    new ExcelMapper()
      {
        Id=13,
        ColumnName="Memo"
      },
    new ExcelMapper()
      {
        Id=14,
       ColumnName="Account Name"
      }
            };
        }
        public class ExcelMapper
        {
            public int Id { get; set; }
            public string ColumnName { get; set; }

        }
        public static class TokenManager
        {
            public static List<TokenDTO> Tokens = new List<TokenDTO>();
        }

        public static class SwaggerConfiguration
        {
            public static readonly string SwaggerEndPointURL = Utils._config["SwaggerConfigurations:SwaggerEndPointURL"];
            public static readonly string SwaggerEndPointName = Utils._config["SwaggerConfigurations:SwaggerEndPointName"];
        }

        public static class DomainConfiguration
        {
            public static readonly string Domain = Utils._config["DomainConfiguration:Domain"];
            public static readonly string PortalAppDomain = Utils._config["DomainConfiguration:PortalApp"];
            public static readonly string SecureAppDomain = Utils._config["DomainConfiguration:SecureApp"];
            public static readonly string SuperAdminAppDomain = Utils._config["DomainConfiguration:SuperAdminApp"];
        }

        public static class EmailTemplateConfiguration
        {
            public static readonly string VerifyEmailDescription = Utils._config["EmailTemplateConfiguration:VerifyEmail:Description"];
            public static readonly string VerifyEmailButtonTitle = Utils._config["EmailTemplateConfiguration:VerifyEmail:ButtonTitle"];
            public static readonly string VerifyEmailMessage = Utils._config["EmailTemplateConfiguration:VerifyEmail:Message"];
            public static readonly string VerifyEmailAddress = Utils._config["EmailTemplateConfiguration:VerifyEmail:Address"];

            public static readonly string ResetEmailDescription = Utils._config["EmailTemplateConfiguration:ResetEmail:Description"];
            public static readonly string ResetEmailButtonTitle = Utils._config["EmailTemplateConfiguration:ResetEmail:ButtonTitle"];
            public static readonly string ResetEmailMessage = Utils._config["EmailTemplateConfiguration:ResetEmail:Message"];
            public static readonly string ResetEmailAddress = Utils._config["EmailTemplateConfiguration:ResetEmail:Address"];
        }

        public static class BillingPlans
        {
            public static readonly string Free = "Free";
            public static readonly string Monthly = "Monthly";
            public static readonly string Yearly = "Yearly";
        }

        public static class Statuses
        {
            public static readonly string Pending = "Pending";
            public static readonly string Completed = "Completed";
        }
        public static class ElasticPool
        {
            public static readonly string Server = Utils._config["PoolConfiguration:Server"];
            public static readonly string ElasticPoolName = Utils._config["PoolConfiguration:ElasticPoolName"];
            public static readonly string Edition = Utils._config["PoolConfiguration:Edition"];

            public static readonly string Dtu = Utils._config["PoolConfiguration:Dtu"];
            public static readonly string DatabaseDtuMax = Utils._config["PoolConfiguration:DatabaseDtuMax"];
            public static readonly string DatabaseDtuMin = Utils._config["PoolConfiguration:DatabaseDtuMin"];

            public static readonly string StorageMB = Utils._config["PoolConfiguration:StorageMB"];


        }
        public static class LogoConfigurations
        {
            public static readonly string PortalFrontEndLogo = Utils._config["LogoConfiguration:PortalFrontEndLogo"];
        }
        public static class DbConfig
        {
            public static readonly string DummyConnectionString = Utils._config["DatabaseConfig:DummyConnectionString"];
        }
        public static class EmailConfiguration
        {
            public static readonly string Email = Utils._config["EmailConfigurations:Email"];
            public static readonly string Password = Utils._config["EmailConfigurations:Password"];
            public static readonly string DisplayName = Utils._config["EmailConfigurations:DisplayName"];
            public static readonly string Host = Utils._config["EmailConfigurations:Host"];
            public static readonly string Port = Utils._config["EmailConfigurations:Port"];
        }


        public static class OtherConstants
        {
            public static string messageType = MessageType.Success;
            public static string responseMsg = "";
            public static bool isSuccessful;

            public static void Clear()
            {
                messageType = "";
                responseMsg = "";
                isSuccessful = false;
            }
        }

        public static class MessageType
        {
            public static readonly string Success = "success";
            public static readonly string Error = "error";
            public static readonly string Warning = "warning";
            public static readonly string Info = "info";
        }

        public static class CustomClaims
        {
            public static readonly string TenantId = "TenantId";
            public static readonly string UserId = "UserId";
            public static readonly string Role = "Role";
        }

        public static class Roles
        {
            public static readonly string Admin = "Admin";
            public static readonly string CompanyAdmin = "CompanyAdmin";
            public static readonly string User = "User";
        }

        public static class ClaimValue
        {
            public static readonly string View = "View";
            public static readonly string Create = "Create";
            public static readonly string Edit = "Edit";
            public static readonly string Delete = "Delete";
        }

        public static class ClaimType
        {
            public static readonly string Contacts = "Contacts";
            public static readonly string Task = "Tasks";
            public static readonly string Calendar = "Calendar";
            public static readonly string Documents = "Documents";
            public static readonly string Accounting = "Accounting";
            public static readonly string Cases = "Cases";
            public static readonly string Reports = "Reports";
            public static readonly string Settings = "Settings";
            public static readonly string Team = "Team";
            public static readonly string Analytics = "Analytics";
            public static readonly string History = "History";
        }

        public static class Blobs
        {
            public static readonly string ConnectionString = Utils._config["Blob:ConnectionString"];
            public static readonly string ImagesContainerName = Utils._config["Blob:ImagesContainerName"];
            public static readonly string FilesContainerName = Utils._config["Blob:FilesContainerName"];
        }

        public static class AuditType
        {
            public static readonly string Create = "Create";
            public static readonly string Update = "Update";
            public static readonly string Delete = "Delete";
        }
        public static class ShopifyKeys
        {
            public static string SHOPIFY_API_KEY { get; set; }
            public static string PASSWORD { get; set; }
            public static string SHARED_KEY { get; set; }
            public static string SHOPIFY_BASE_URL { get; set; }
            public static string SHOPIFY_ORDER_HOOK_KEY { get; set; }
        }
    }
}
