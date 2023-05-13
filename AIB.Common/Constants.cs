using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Common
{
   public class Constants
    {
        public static class JWTConfiguration
        {
            public static readonly string JWTIssuer = Utils._config["Jwt:Issuer"];
            public static readonly string JWTAudience = Utils._config["Jwt:Audience"];
            public static readonly string JWTKey = Utils._config["Jwt:Key"];
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

        public static class LogoConfigurations
        {
            public static readonly string PortalFrontEndLogo = Utils._config["LogoConfiguration:PortalFrontEndLogo"];
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
            public static readonly string SalesManagement = "SalesManagment";
            public static readonly string BankAccount = "BankAccount";
            public static readonly string Company = "Company";
            public static readonly string Outstandings = "Outstandings";
            public static readonly string Accounting = "Accounting";
            public static readonly string ManageReports = "ManageReports";
            public static readonly string ManageSetups = "ManageSetups";
            public static readonly string Settings = "Settings";
            public static readonly string Transactions = "Transactions";
            public static readonly string Team = "Team";
            public static readonly string Agent = "Agent";
            public static readonly string VehicleType = "VehicleType";
            public static readonly string MotorType = "MotorType";
            public static readonly string Expenses = "Expenses";
        }

        public static class Blob
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
    }
}
