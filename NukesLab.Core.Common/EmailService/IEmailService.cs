﻿namespace NukesLab.Core.Common.EmailService 
{ 
    public interface IEmailService
    {
        bool SendEmailWithoutTemplate(string to, string subject, string body, bool isBodyHtml = false);
    }
}
