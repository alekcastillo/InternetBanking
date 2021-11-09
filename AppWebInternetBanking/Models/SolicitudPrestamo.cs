using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class SolicitudPrestamo
    {
        public int Codigo { get; set; }
        public int CodigoCuenta { get; set; }
        public string CondicionLaboral { get; set; }
        public string Profesion { get; set; }
        public decimal IngresoMensual { get; set; }
        public string DestinoPrestamo { get; set; }
        public decimal MontoDeseado { get; set; }
    }
}