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
    public partial class frmActivo : System.Web.UI.Page
    {
        IEnumerable<Activo> activos = new ObservableCollection<Activo>();
        ActivoManager activoManager = new ActivoManager();
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
                activos = await activoManager.ObtenerActivos(Session["Token"].ToString());
                gvActivos.DataSource = activos.ToList();
                gvActivos.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de activos";
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
                    || string.IsNullOrEmpty(txtTipo.Text)
                    || string.IsNullOrEmpty(txtValor.Text))
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
                else
                {

                    Activo activo = new Activo()
                    {
                        CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                        Tipo = txtTipo.Text,
                        Valor = Convert.ToDecimal(txtValor.Text)
                    };

                    Activo activoIngresado = await activoManager.Ingresar(activo, Session["Token"].ToString());

                    lblResultado.Text = "Activo ingresado con exito";
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
                    || string.IsNullOrEmpty(txtCodigoMant.Text)
                    || string.IsNullOrEmpty(txtTipo.Text)
                    || string.IsNullOrEmpty(txtValor.Text))
                {
                    lblResultado.Text = "Es necesario llenar todos los espacios.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else
                {
                    Activo activo = new Activo()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                        Tipo = txtTipo.Text,
                        Valor = Convert.ToDecimal(txtValor.Text)
                    };

                    Activo activoActualizado = await activoManager.Actualizar(activo, Session["Token"].ToString());

                    lblResultado.Text = "Activo actualizado con exito";
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

                Activo activo = await activoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(activo.Tipo))
                {
                    ltrModalMensaje.Text = "Activo eliminado";
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
                    Vista = "frmActivo.aspx",
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
            ltrTituloMantenimiento.Text = "Nuevo Activo";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;
            txtCodigoCuenta.Visible = true;
            ltrCodigoCuenta.Visible = true;
            txtValor.Visible = true;
            ltrValor.Visible = true;
            txtTipo.Visible = true;
            ltrTipo.Visible = true;
            txtTipo.Text = string.Empty;
            txtValor.Text = string.Empty;
            txtCodigoMant.Text = string.Empty;
            txtCodigoCuenta.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvActivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvActivos.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar activo";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoCuenta.Text = row.Cells[1].Text.Trim();
                    txtTipo.Text = row.Cells[2].Text.Trim();
                    txtValor.Text = row.Cells[3].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el activo?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

    }
}