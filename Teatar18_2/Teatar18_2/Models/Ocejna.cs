using System;
using System.ComponentModel.DataAnnotations;

public class Ocjena
{
	public Ocjena(){}
	[Key] public int ID { get; set; }
    [ForeignKey("Predstava")] public int IDPredstave { get; set; }
	public int ocjena {  get; set; }	
}
