using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teatar18_2.Models;

public class Karta
{
	public Karta(){}
    [Key] public int ID { get; set; }
    [ForeignKey("Izvedba")] public int IDIzvedbe { get; set; }
    public int sjediste { get; set; }
    public double cijena { get; set; }
    public bool placena { get; set; } = false;
    [ForeignKey("Rezervacija")] public int? IDRezervacije { get; set; } 
    public Izvedba? Izvedba { get; set; }
    public Rezervacija? Rezervacija { get; set; }
}
