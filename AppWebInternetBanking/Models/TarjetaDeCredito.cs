using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class TarjetaDeCredito
    {
        public int Codigo { get; set; }
        public int CodigoCuenta { get; set; }
        public string NumeroDeTarjeta { get; set; }
        public string Tipo { get; set; }
        public int AnoExpiracion { get; set; }
        public int MesExpiracion { get; set; }
        public int CV2 { get; set; }
    }
}