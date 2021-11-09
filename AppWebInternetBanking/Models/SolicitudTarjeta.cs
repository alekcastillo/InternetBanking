using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class SolicitudTarjeta
    {
        public int Codigo { get; set; }
        public int CodigoCuenta { get; set; }
        public string EstadoCivil { get; set; }
        public string CondicionLaboral { get; set; }
        public decimal IngresoMensual { get; set; }
        public string ProductoDeseado { get; set; }
    }
}