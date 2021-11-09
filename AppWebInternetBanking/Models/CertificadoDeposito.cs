using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;

    public partial class CertificadosDepositos
    {

        public int Codigo { get; set; }
        public int CodigoCuenta { get; set; }
        public System.DateTime FechaOperacion { get; set; }
        public System.DateTime FechaVencimiento { get; set; }
        public int Plazo { get; set; }
        public string Monto { get; set; }
        public decimal Interes { get; set; }

    }
}