using System;
using System.ComponentModel.DataAnnotations;

public class Karta
{
	public Karta(){}
    [Key] public int ID { get; set; }
    [ForeignKey("Izvedba")] public int IDIzvedbe { get; set; }
    public int sjediste { get; set; }
    public double cijena { get; set; }
    public bool placena { get; set; } = false;
    [ForeignKey("Rezervacija")] public int IDRezervacije { get; set; }  

}
