using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public class Prestamo
    {
        public int Codigo { get; set; }
        public int CodigoCuenta { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraVencimiento { get; set; }
        public decimal Monto { get; set; }
        public decimal Intereses { get; set; }
    }
}