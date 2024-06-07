using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Teatar18_2.Models
{
    public class Predstava
    {
        public Predstava() {}
        [Key] public int ID { get; set; }
        public string naziv { get; set; }
        public string glumci { get; set; }
        public string? scenaristi { get; set; }
        public string? reziseri { get; set; }
        public string? scenografi { get; set; }
        [EnumDataType(typeof(Zanr))] public Zanr zanr { get; set; }
        public string opis { get; set; }
        public string poster { get; set; }
        public int trajanje { get; set; }
        public bool uRepertoaru { get; set; } = false;
        public bool preporucena { get; set; } = false;
    }
}

/*
 @model Teatar18_2.Models.Predstava

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@Model.naziv</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
        }
        .header {
            background-color: #a10000;
            color: white;
            padding: 10px;
            text-align: center;
        }
        .header h1 {
            margin: 0;
        }
        .nav {
            display: flex;
            justify-content: center;
            background-color: #e0e0e0;
            padding: 10px;
        }
        .nav a {
            margin: 0 15px;
            text-decoration: none;
            color: black;
            font-weight: bold;
        }
        .container {
            display: flex;
            justify-content: space-around;
            margin: 20px;
        }
        .details, .description {
            background-color: white;
            padding: 20px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
            width: 45%;
        }
        .details h2, .description h2 {
            margin-top: 0;
        }
        .rating {
            background-color: #a10000;
            color: white;
            padding: 10px;
            text-align: center;
            font-size: 1.2em;
        }
        .reserve {
            display: block;
            background-color: #a10000;
            color: white;
            text-align: center;
            padding: 10px;
            margin-top: 20px;
            text-decoration: none;
            font-weight: bold;
        }
        .poster {
            text-align: center;
        }
        .poster img {
            max-width: 100%;
        }
    </style>
</head>
<body>
    <div class="header">
        <h1>Teatar18</h1>
    </div>
    <div class="nav">
        <a href="#">Početna</a>
        <a href="#">O nama</a>
        <a href="#">Pitanja</a>
        <a href="#">Prijava</a>
        <a href="#">Registracija</a>
    </div>
    <div class="container">
        <div class="details">
            <h2>@Model.naziv</h2>
            <p>@Model.zanr</p>
            <p>@Model.trajanje</p>
            <div class="rating">Ocjena: @Model.oc</div>
            <p>Režiser: @Model.reziseri</p>
            <p>Scenarist: @Model.scenaristi</p>
            <p>Scenograf: @Model.scenografi</p>
            <p>Glumci: @Model.glumci</p>
        </div>
        <div class="description">
            <h2>Opis</h2>
            <p>@Model.opis</p>
            <div class="poster">
                <img src="@Model.poster" alt="@Model.naziv poster">
                <a href="#" class="reserve">Rezerviši</a>
            </div>
        </div>
    </div>
</body>
</html>

 * */