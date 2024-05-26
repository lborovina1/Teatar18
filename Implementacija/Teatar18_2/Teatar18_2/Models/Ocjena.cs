using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teatar18_2.Models;

public class Ocjena
{
	public Ocjena(){}
	[Key] public int ID { get; set; }
	[ForeignKey("Predstava")] public int IDPredstave { get; set; }
	public int ocjena { get; set; }	
	public Predstava? Predstava { get; set; }
}
