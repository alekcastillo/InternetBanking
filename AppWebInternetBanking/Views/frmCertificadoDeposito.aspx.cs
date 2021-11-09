using AppWebInternetBanking.Controllers;
using AppWebInternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppWebInternetBanking.Views
{
    public partial class frmCertificadoDeposito : System.Web.UI.Page
    {

        IEnumerable<CertificadosDepositos> certificadosDepositos = new ObservableCollection<CertificadosDepositos>();
        CertificadosDepositosManager certificadosDepositosManager = new CertificadosDepositosManager();
        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();
        CuentaManager cuentaManager = new CuentaManager();
        static string _codigo = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                    InicializarControles();
            }
        }

        private async void InicializarControles()
        {
            try
            {
                certificadosDepositos = await certificadosDepositosManager.ObtenerCertificados(Session["Token"].ToString());
                gvCertificado.DataSource = certificadosDepositos.ToList();
                gvCertificado.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de certificados de depositos";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());

                if (string.IsNullOrEmpty(txtCodigoCuenta.Text)
                    || string.IsNullOrEmpty(txtCodigoCuenta.Text)
                    || string.IsNullOrEmpty(txtFechaOperacion.Text)
                    || string.IsNullOrEmpty(txtFechaVencimiento.Text)
                    || string.IsNullOrEmpty(txtPlazo.Text)
                    || string.IsNullOrEmpty(txtMonto.Text)
                    || string.IsNullOrEmpty(txtInteres.Text))
                {
                    lblResultado.Text = "Es necesario llenar todos los espacios.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else if (!cuentas.Any(x => x.Codigo == Int32.Parse(txtCodigoCuenta.Text)))
                {
                    lblResultado.Text = "No existe este codigo de Cuenta.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else if (Convert.ToDateTime(txtFechaOperacion.Text) > Convert.ToDateTime(txtFechaVencimiento.Text))
                {
                    lblResultado.Text = "La fecha de operacion no puede ser mayor a la fecha de vencimiento.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else
                {
                    CertificadosDepositos certificadosDepositos = new CertificadosDepositos()
                    {
                        CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                        FechaOperacion = Convert.ToDateTime(txtFechaOperacion.Text),
                        FechaVencimiento = Convert.ToDateTime(txtFechaVencimiento.Text),
                        Plazo = Convert.ToInt32(txtPlazo.Text),
                        Monto = txtMonto.Text,
                        Interes = Convert.ToDecimal(txtInteres.Text)
                    };

                    CertificadosDepositos certificadosDepositosIngresado = await certificadosDepositosManager.Ingresar(certificadosDepositos, Session["Token"].ToString());

                    lblResultado.Text = "Certificado ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);

                }
            }
            else // modificar
            {
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());

                if (string.IsNullOrEmpty(txtCodigoCuenta.Text)
                    || string.IsNullOrEmpty(txtCodigoCuenta.Text)
                    || string.IsNullOrEmpty(txtFechaOperacion.Text)
                    || string.IsNullOrEmpty(txtFechaVencimiento.Text)
                    || string.IsNullOrEmpty(txtPlazo.Text)
                    || string.IsNullOrEmpty(txtMonto.Text)
                    || string.IsNullOrEmpty(txtInteres.Text))
                {
                    lblResultado.Text = "Es necesario llenar todos los espacios.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else if (Convert.ToDateTime(txtFechaOperacion.Text) > Convert.ToDateTime(txtFechaVencimiento.Text))
                {
                    lblResultado.Text = "La fecha de operacion no puede ser mayor a la fecha de vencimiento.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else
                {
                    CertificadosDepositos certificadosDepositos = new CertificadosDepositos()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                        FechaOperacion = Convert.ToDateTime(txtFechaOperacion.Text),
                        FechaVencimiento = Convert.ToDateTime(txtFechaVencimiento.Text),
                        Plazo = Convert.ToInt32(txtPlazo.Text),
                        Monto = txtMonto.Text,
                        Interes = Convert.ToDecimal(txtInteres.Text)
                    };

                    CertificadosDepositos certificadosDepositosActualizado = await certificadosDepositosManager.Actualizar(certificadosDepositos, Session["Token"].ToString());

                    lblResultado.Text = "Certificado actualizado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);


                }
            }
        }

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
        }

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {
            try
            {

                CertificadosDepositos certificadosDepositos = await certificadosDepositosManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(certificadosDepositos.Monto))
                {
                    ltrModalMensaje.Text = "Certificado eliminado";
                    btnAceptarModal.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModal(); });", true);
                    InicializarControles();
                }
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario =
                    Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    FechaHora = DateTime.Now,
                    Vista = "frmCertificadoDeposito.aspx",
                    Accion = "btnAceptarModal_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult,
                    Descripcion = ex.Message
                };
                Error errorIngresado = await errorManager.Ingresar(error);
            }
        }

        protected void btnCancelarModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ltrTituloMantenimiento.Text = "Nuevo Certificado";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;
            txtCodigoCuenta.Visible = true;
            ltrCodigoCuenta.Visible = true;
            txtFechaOperacion.Visible = true;
            ltrFechaOperacion.Visible = true;
            txtFechaVencimiento.Visible = true;
            ltrFechaVencimiento.Visible = true;
            txtPlazo.Visible = true;
            ltrPlazo.Visible = true;
            txtMonto.Visible = true;
            ltrMonto.Visible = true;
            txtInteres.Visible = true;
            ltrInteres.Visible = true;
            txtInteres.Text = string.Empty;
            txtMonto.Text = string.Empty;
            txtPlazo.Text = string.Empty;
            txtFechaOperacion.Text = string.Empty;
            txtFechaVencimiento.Text = string.Empty;
            txtCodigoMant.Text = string.Empty;
            txtCodigoCuenta.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvCertificado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCertificado.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar certificado";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoCuenta.Text = row.Cells[1].Text.Trim();
                    txtFechaOperacion.Text = row.Cells[2].Text.Trim();
                    txtFechaVencimiento.Text = row.Cells[3].Text.Trim();
                    txtPlazo.Text = row.Cells[4].Text.Trim();
                    txtMonto.Text = row.Cells[5].Text.Trim();
                    txtInteres.Text = row.Cells[6].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el certificado?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}