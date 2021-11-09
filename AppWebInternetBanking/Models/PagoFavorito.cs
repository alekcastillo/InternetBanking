using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class PagoFavorito
    {
        public int Codigo { get; set; }
        public int CodigoCuenta { get; set; }
        public string Nombre { get; set; }
        public string IBAN { get; set; }
    }
}