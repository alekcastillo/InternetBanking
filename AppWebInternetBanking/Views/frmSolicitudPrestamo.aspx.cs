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
    public partial class frmSolicitudPrestamo : System.Web.UI.Page
    {
        IEnumerable<SolicitudPrestamo> solicitudPrestamos = new ObservableCollection<SolicitudPrestamo>();
        SolicitudPrestamoManager solicitudPrestamoManager = new SolicitudPrestamoManager();

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
                solicitudPrestamos = await solicitudPrestamoManager.ObtenerSolicitudPrestamos(Session["Token"].ToString());
                
                gvSolicitudPrestamos.DataSource = solicitudPrestamos.ToList();
                gvSolicitudPrestamos.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de solicitudes de prestamos";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());

                if (string.IsNullOrEmpty(txtCodigoCuenta.Text)
                    || string.IsNullOrEmpty(txtProfesion.Text)
                    || string.IsNullOrEmpty(txtIngresoMensual.Text)
                    || string.IsNullOrEmpty(txtMontoDeseado.Text))
                {
                    lblResultado.Text = "Es necesario llenar todos los espacios.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else if(!cuentas.Any(x => x.Codigo == Int32.Parse(txtCodigoCuenta.Text)))
                {
                    lblResultado.Text = "No existe este código de Cuenta.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else
                {

                    SolicitudPrestamo solicitudPrestamo = new SolicitudPrestamo()
                    {
                        //Descripcion = txtDescripcion.Text,
                        //Estado = ddlEstadoMant.SelectedValue
                        CodigoCuenta = Int32.Parse(txtCodigoCuenta.Text),
                        CondicionLaboral = ddlCondicionLaboral.SelectedValue,
                        Profesion = txtProfesion.Text,
                        IngresoMensual = Int32.Parse(txtIngresoMensual.Text),
                        DestinoPrestamo = ddlDestinoPrestamo.SelectedValue,
                        MontoDeseado = Int32.Parse(txtMontoDeseado.Text)

                    };

                    SolicitudPrestamo solicitudPrestamoIngresado = await solicitudPrestamoManager.Ingresar(solicitudPrestamo, Session["Token"].ToString());
                    lblResultado.Text = "Solicitud de prestamo ingresada con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    //Correo correo = new Correo();
                    //correo.Enviar("Nuevo servicio incluido", servicioIngresado.Descripcion, "svillagra07@gmail.com",
                    //    Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    
                }
            }
            else // modificar
            {
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());

                if (string.IsNullOrEmpty(txtCodigoCuenta.Text)
                    || string.IsNullOrEmpty(txtProfesion.Text)
                    || string.IsNullOrEmpty(txtIngresoMensual.Text)
                    || string.IsNullOrEmpty(txtMontoDeseado.Text))
                {
                    lblResultado.Text = "Es necesario llenar todos los espacios.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else
                {
                    SolicitudPrestamo solicitudPrestamo = new SolicitudPrestamo()
                    {
                        Codigo = Int32.Parse(txtCodigoMant.Text),
                        CodigoCuenta = Int32.Parse(txtCodigoCuenta.Text),
                        CondicionLaboral = ddlCondicionLaboral.SelectedValue,
                        Profesion = txtProfesion.Text,
                        IngresoMensual = Int32.Parse(txtIngresoMensual.Text),
                        DestinoPrestamo = ddlDestinoPrestamo.SelectedValue,
                        MontoDeseado = Int32.Parse(txtMontoDeseado.Text)
                    };

                    SolicitudPrestamo solicitudPresamoActualizado = await solicitudPrestamoManager.Actualizar(solicitudPrestamo, Session["Token"].ToString());

                    lblResultado.Text = "Solicitud de prestamo actualizada con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    //Correo correo = new Correo();
                    //correo.Enviar("Servicio actualizado con exito", servicioActualizado.Descripcion, "svillagra07@gmail.com",
                    //    Convert.ToInt32(Session["CodigoUsuario"].ToString()));

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

                SolicitudPrestamo solicitudPrestamo = await solicitudPrestamoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(solicitudPrestamo.CodigoCuenta.ToString())
                    || !string.IsNullOrEmpty(solicitudPrestamo.Profesion.ToString())
                    || !string.IsNullOrEmpty(solicitudPrestamo.IngresoMensual.ToString())
                    || !string.IsNullOrEmpty(solicitudPrestamo.MontoDeseado.ToString()))
                {
                    ltrModalMensaje.Text = "Solicitud de prestamo eliminada";
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
                    Vista = "frmSolicitudPrestamo.aspx",
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
            ltrTituloMantenimiento.Text = "Nueva solicitud de prestamo";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodigoCuenta.Visible = true;
            txtCodigoCuenta.Visible = true;
            ddlCondicionLaboral.Enabled = true;
            ltrProfesion.Visible = true;
            txtProfesion.Visible = true;
            ltrIngresoMensual.Visible = true;
            txtIngresoMensual.Visible = true;
            ddlDestinoPrestamo.Enabled = true;
            ltrMontoDeseado.Visible = true;
            txtMontoDeseado.Visible = true;

            txtCodigoMant.Text = string.Empty;
            txtCodigoCuenta.Text = string.Empty;
            txtProfesion.Text = string.Empty;
            txtIngresoMensual.Text = string.Empty;
            txtMontoDeseado.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvSolicitudPrestamos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSolicitudPrestamos.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar solicitud de prestamo";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoCuenta.Text = row.Cells[1].Text.Trim();
                    txtProfesion.Text = row.Cells[3].Text.Trim();
                    txtIngresoMensual.Text = row.Cells[4].Text.Trim();
                    txtMontoDeseado.Text = row.Cells[6].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar la solicitud de prestamos?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }

}