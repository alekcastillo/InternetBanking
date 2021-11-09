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
    public partial class frmPrestamo : System.Web.UI.Page
    {
        IEnumerable<Prestamo> prestamos = new ObservableCollection<Prestamo>();
        PrestamoManager prestamosManager = new PrestamoManager();

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
                prestamos = await prestamosManager.ObtenerPrestamo(Session["Token"].ToString());
                gvprestamo.DataSource = prestamos.ToList();
                gvprestamo.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de seguro";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());

                if (string.IsNullOrEmpty(txtCodigoCuenta.Text) ||
                     string.IsNullOrEmpty(txtFechaHoraInicio.Text) ||
                     string.IsNullOrEmpty(txtFechaHoraVencimiento.Text) ||
                     string.IsNullOrEmpty(txtMonto.Text) ||
                     string.IsNullOrEmpty(txtIntereses.Text) ||
                  (Convert.ToDateTime(txtFechaHoraInicio.Text) > Convert.ToDateTime(txtFechaHoraVencimiento.Text))
                  )
                {
                    lblResultado.Text = "Hubo un error al efectuar la operacion asegurese de llenar bien los espacios";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else if (!cuentas.Any(x => x.Codigo == Int32.Parse(txtCodigoCuenta.Text)))
                {
                    lblResultado.Text = "Digite un codigo de cuenta que exista";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;

                }
                else
                {
                    Prestamo prestamo = new Prestamo()
                    {
                        CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                        FechaHoraInicio = DateTime.Parse(txtFechaHoraInicio.Text),
                        FechaHoraVencimiento = DateTime.Parse(txtFechaHoraVencimiento.Text),
                        Monto = Convert.ToDecimal(txtMonto.Text),
                        Intereses = Convert.ToDecimal(txtIntereses.Text)

                    };

                    Prestamo prestamoIngresado = await prestamosManager.Ingresar(prestamo, Session["Token"].ToString());
                    lblResultado.Text = "Prestamo ingresado con exito";
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


                if (string.IsNullOrEmpty(txtCodigoCuenta.Text) ||
                       string.IsNullOrEmpty(txtFechaHoraInicio.Text) ||
                       string.IsNullOrEmpty(txtFechaHoraVencimiento.Text) ||
                       string.IsNullOrEmpty(txtMonto.Text) ||
                       string.IsNullOrEmpty(txtIntereses.Text) ||
                    (Convert.ToDateTime(txtFechaHoraInicio.Text) > Convert.ToDateTime(txtFechaHoraVencimiento.Text))
                    )
                {
                    lblResultado.Text = "Hubo un error al efectuar la operacion asegurese de llenar bien los espacios";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;


                }
                else if (!cuentas.Any(x => x.Codigo == Int32.Parse(txtCodigoCuenta.Text)))
                {
                    lblResultado.Text = "Digite un codigo de cuenta que exista";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;

                }
                else
                {
                    Prestamo prestamo = new Prestamo()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                        FechaHoraInicio = DateTime.Parse(txtFechaHoraInicio.Text),
                        FechaHoraVencimiento = DateTime.Parse(txtFechaHoraVencimiento.Text),
                        Monto = Convert.ToDecimal(txtMonto.Text),
                        Intereses = Convert.ToDecimal(txtIntereses.Text)
                    };
                    Prestamo prestamoActualizado = await prestamosManager.Actualizar(prestamo, Session["Token"].ToString());

                    lblResultado.Text = "Prestamo actualizado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);


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

                Prestamo prestamo = await prestamosManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(prestamo.Monto.ToString()))
                {
                    ltrModalMensaje.Text = "Prestamo eliminado";
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
                    Vista = "frmPrestamo.aspx",
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
            ltrTituloMantenimiento.Text = "Nuevo Prestamo";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            txtCodigoCuenta.Visible = true;
            ltrCodigoCuenta.Visible = true;
            txtFechaHoraInicio.Visible = true;
            ltrFechaHoraInicio.Visible = true;
            txtFechaHoraVencimiento.Visible = true;
            ltrFechaHoraVencimiento.Visible = true;
            txtMonto.Visible = true;
            ltrMonto.Visible = true;
            txtIntereses.Visible = true;
            ltrIntereses.Visible = true;

            txtCodigoMant.Text = string.Empty;
            txtCodigoCuenta.Text = string.Empty;
            txtFechaHoraInicio.Text = string.Empty;
            txtFechaHoraVencimiento.Text = string.Empty;
            txtMonto.Text = string.Empty;
            txtIntereses.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvPrestamo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvprestamo.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar Prestamo";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoCuenta.Text = row.Cells[1].Text.Trim();
                    txtFechaHoraInicio.Text = row.Cells[2].Text.Trim();
                    txtFechaHoraVencimiento.Text = row.Cells[3].Text.Trim();
                    txtMonto.Text = row.Cells[4].Text.Trim();
                    txtIntereses.Text = row.Cells[5].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el Prestamo?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}