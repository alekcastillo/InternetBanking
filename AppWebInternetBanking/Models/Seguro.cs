using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public class Seguro
    {
        public int Codigo { get; set; }
        public int CodigoCuenta { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraVencimiento { get; set; }
        public decimal Prima { get; set; }
        public string Descripcion { get; set; }
    }
}