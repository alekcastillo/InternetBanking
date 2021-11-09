using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Activo
    {

        public int Codigo { get; set; }
        public int CodigoCuenta { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }

    }
}