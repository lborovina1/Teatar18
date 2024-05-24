﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teatar18_2.Models
{
    public class Rezervacija
    {
        public Rezervacija(){}
        [Key] public int ID { get; set; }
        [ForeignKey("Korisnik")] public int IDKorisnika { get; set; }
        public bool kupovina { get; set; }
        public int brojKarata { get; set; }
        public double popust { get; set; } = 0;
        public Korisnik? Korisnik { get; set; }
    }
}