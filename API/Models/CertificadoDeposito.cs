//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CertificadoDeposito
    {
        public int Codigo { get; set; }
        public int CodigoCuenta { get; set; }
        public System.DateTime FechaOperacion { get; set; }
        public System.DateTime FechaVencimiento { get; set; }
        public int Plazo { get; set; }
        public string Monto { get; set; }
        public decimal Interes { get; set; }
    
        public virtual Cuenta Cuenta { get; set; }
    }
}