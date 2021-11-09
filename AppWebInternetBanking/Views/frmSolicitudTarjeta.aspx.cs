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
    public partial class frmSolicitudTarjeta : System.Web.UI.Page
    {
        IEnumerable<SolicitudTarjeta> solicitudTarjetas = new ObservableCollection<SolicitudTarjeta>();
        SolicitudTarjetaManager solicitudTarjetaManager = new SolicitudTarjetaManager();

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

                solicitudTarjetas = await solicitudTarjetaManager.ObtenerSolicitudTarjetas(Session["Token"].ToString());

                gvSolicitudTarjetas.DataSource = solicitudTarjetas.ToList();
                gvSolicitudTarjetas.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de solicitudes de tarjetas";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());
                

                if (string.IsNullOrEmpty(txtCodigoCuenta.Text) 
                    || string.IsNullOrEmpty(txtIngresoMensual.Text))
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
                    SolicitudTarjeta solicitudTarjeta = new SolicitudTarjeta()
                    {

                        CodigoCuenta = Int32.Parse(txtCodigoCuenta.Text),
                        EstadoCivil = ddlEstadoCivil.SelectedValue,
                        CondicionLaboral = ddlCondicionLaboral.SelectedValue,
                        IngresoMensual = Convert.ToDecimal(txtIngresoMensual.Text),
                        ProductoDeseado = ddlProductoDeseado.SelectedValue

                    };

                    SolicitudTarjeta solicitudTarjetaIngresado = await solicitudTarjetaManager.Ingresar(solicitudTarjeta, Session["Token"].ToString());

                    lblResultado.Text = "Solicitud de tarjeta ingresada con exito";
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

                if (string.IsNullOrEmpty(txtCodigoMant.Text)
                    || string.IsNullOrEmpty(txtCodigoCuenta.Text)
                    || string.IsNullOrEmpty(txtIngresoMensual.Text))
                {
                    lblResultado.Text = "No pueden existir espacios vacíos.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else if (!cuentas.Any(x => x.Codigo == Int32.Parse(txtCodigoCuenta.Text)))
                {
                    lblResultado.Text = "No existe este código de Cuenta.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else
                {
                    SolicitudTarjeta solicitudTarjeta = new SolicitudTarjeta()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoCuenta = Int32.Parse(txtCodigoCuenta.Text),
                        EstadoCivil = ddlEstadoCivil.SelectedValue,
                        CondicionLaboral = ddlCondicionLaboral.SelectedValue,
                        IngresoMensual = Convert.ToDecimal(txtIngresoMensual.Text),
                        ProductoDeseado = ddlProductoDeseado.SelectedValue
                    };

                    SolicitudTarjeta solicitudTarjetaActualizado = await solicitudTarjetaManager.Actualizar(solicitudTarjeta, Session["Token"].ToString());

                    lblResultado.Text = "Solicitud de tarjeta actualizada con exito";
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

                SolicitudTarjeta solicitudTarjeta = await solicitudTarjetaManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(solicitudTarjeta.CodigoCuenta.ToString())
                    || !string.IsNullOrEmpty(solicitudTarjeta.IngresoMensual.ToString()))
                {
                    ltrModalMensaje.Text = "Solicitud de tarjeta eliminada";
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
                    Vista = "frmSolicitudTarjeta.aspx",
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
            ltrTituloMantenimiento.Text = "Nueva solicitud de tarjeta";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodigoCuenta.Visible = true;
            txtCodigoCuenta.Visible = true;
            ddlEstadoCivil.Enabled = true;
            ddlCondicionLaboral.Enabled = true;
            ltrIngresoMensual.Visible = true;
            txtIngresoMensual.Visible = true;
            ddlProductoDeseado.Enabled = true;

            txtCodigoMant.Text = string.Empty;
            txtCodigoCuenta.Text = string.Empty;
            txtIngresoMensual.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvSolicitudTarjetas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSolicitudTarjetas.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar solicitud";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoCuenta.Text = row.Cells[1].Text.Trim();
                    txtIngresoMensual.Text = row.Cells[4].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar la solicitud de tarjetas?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

        

        //private async void existeLlaveForanea()
        //{
        //    List<SolicitudTarjeta> = await solicitudTarjetaManager.ObtenerSolicitudTarjetas(Session["Token"].ToString());
        //    lblPrueba.Text = solicitudTarjetas.Where(solicitudTarjetas => solicitudTarjetas.Codigo == 3).ToString();
        //}

    }
}