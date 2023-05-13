namespace AIB.Common.EmailService 
{
    public interface IEmailService
    {
        //bool SendEmailWithoutTemplate(string to, string subject, string body, bool isBodyHtml = false);
        bool SendEmailWithoutTemplate(string ToEmail, string Name, string subject, string body, bool isBodyHtml = false);
    }
       
}
