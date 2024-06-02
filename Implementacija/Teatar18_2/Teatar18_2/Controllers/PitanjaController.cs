using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Data;
using Teatar18_2.Models;

namespace Teatar18_2.Controllers
{
    public class PitanjaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PitanjaController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Otvara pogled za postavljanje pitanja
        [HttpGet]
        public IActionResult PostavljanjePitanja()
        {
            return View();
        }

        //submit pitanja
        [HttpPost]
        public IActionResult PostavljanjePitanja(Pitanje pitanje)
        {
            pitanje.datumPostavljanja = DateTime.Now;
            pitanje.odgovoreno = false;

            if (User.Identity.IsAuthenticated)
            {
                var korisnik = _context.Users.SingleOrDefault(k => k.Email == User.Identity.Name);
                pitanje.IDKorisnika = korisnik;
            }
            else
            {
                pitanje.IDKorisnika = null;
            }

            _context.Pitanje.Add(pitanje);
            _context.SaveChanges();

            return RedirectToAction("PostavljanjePitanja");
        }

        //generisanje pogleda za korniska
        [HttpGet]
        public IActionResult OdgovaranjeNaPitanja()
        {
            var pitanja = _context.Pitanje.Where(p => !p.odgovoreno).ToList();
            return View(pitanja);
        }

        //odgovaranje na pitanja
        [HttpPost]
        public IActionResult OdgovaranjeNaPitanja(int pitanjeId, string odgovor)
        {
            var pitanje = _context.Pitanje.SingleOrDefault(p => p.ID == pitanjeId);

            if (pitanje == null)
            {
                return NotFound();
            }

            pitanje.odgovoreno = true;
            pitanje.datumOdgovora = DateTime.Now;

            // IDZaposlenika je onaj od trenutno ulogovanog zaposlenika
            pitanje.IDZaposlenika = _context.Users.SingleOrDefault(k => k.Email == User.Identity.Name);

            _context.SaveChanges();

            if (pitanje.IDKorisnika != null)
            {
                SendEmail(pitanje.IDKorisnika.Email, "Odgovor na vaše pitanje", odgovor);
            }

            return RedirectToAction("OdgovaranjeNaPitanja");
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
                Console.WriteLine($"Exception caught in SendEmail(): {ex}");
            }
        }
    }
}