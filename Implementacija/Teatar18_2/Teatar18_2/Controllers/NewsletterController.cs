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

namespace Teatar18_2.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsletterController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        //metoda koja salje mailove svim subscribed Korisnicima
        public IActionResult saljiNewsletter(string subject="Novi repertoar", string message= "Na web stranici je postavljen repertoar za nadolazeci mjesec.\nPogledajte ga i rezervisite svoje karte na vrijeme.")
        {
            var subscribedUsers = _context.Korisnik.Where(k => k.newsletter).ToList();

            if (!subscribedUsers.Any())
            {
                return Ok("Nema korniska koji su pretplaceni na newsletter.");
            }

            var newsletter = new Newsletter
            {   
                informacija = message,
                datumSlanja = DateTime.Now
            };

            _context.Newsletter.Add(newsletter);
            _context.SaveChanges();

            foreach(var user in subscribedUsers)
            {
                SendEmail(user.Email, subject, newsletter.informacija);
            }

            return Ok("Emailovi su uspjesno poslani.");
        }

        private void SendEmail(string toEmail, string subject, string body)
        {
            var fromEmail = "teatar18.5@gmail.com";
            var fromPassword = "Teata5R18!OoaD";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
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
        } 
    }
}
