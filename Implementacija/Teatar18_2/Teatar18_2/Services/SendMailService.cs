using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using Teatar18_2.Data;
using Teatar18_2.Models;
using Microsoft.AspNetCore.Identity;

namespace Teatar18_2.Services
{
    public class SendMailService
    {
        private readonly ApplicationDbContext _context;

        public SendMailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SendEmail(string toEmail, string subject, string body)
        {
            var fromEmail = "teatar18.5@gmail.com";
            //var fromPassword = "Teata5R18!OoaD";
            var appPassword = "snlpwlgwwnbzudvc"; //snlp wlgw wnbz udvc

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, appPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, etc.)
                Console.WriteLine($"Exception caught in SendEmail(): {ex.ToString()}");
            }

            return true;
        }

    }
}
