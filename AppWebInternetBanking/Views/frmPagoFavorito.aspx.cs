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
    public partial class frmPagoFavorito : System.Web.UI.Page
    {
        IEnumerable<PagoFavorito> pagoFavoritos = new ObservableCollection<PagoFavorito>();
        PagoFavoritoManager pagoFavoritoManager = new PagoFavoritoManager();

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

                pagoFavoritos = await pagoFavoritoManager.ObtenerPagoFavoritos(Session["Token"].ToString());

                gvPagoFavoritos.DataSource = pagoFavoritos.ToList();
                gvPagoFavoritos.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de pagos favoritos";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());
                

                if (string.IsNullOrEmpty(txtCodigoCuenta.Text) 
                    || string.IsNullOrEmpty(txtNombre.Text)
                    || string.IsNullOrEmpty(txtIBAN.Text))
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
                else if (txtIBAN.Text.Length != 22)
                {
                    lblResultado.Text = "El IBAN debe ser de 22 digitos.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else
                {
                    PagoFavorito pagoFavorito = new PagoFavorito()
                    {

                        CodigoCuenta = Int32.Parse(txtCodigoCuenta.Text),
                        Nombre = txtNombre.Text,
                        IBAN = txtIBAN.Text

                    };

                    PagoFavorito pagoFavoritoIngresado = await pagoFavoritoManager.Ingresar(pagoFavorito, Session["Token"].ToString());

                    lblResultado.Text = "Pago favorito ingresado con exito";
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
                    || string.IsNullOrEmpty(txtNombre.Text)
                    || string.IsNullOrEmpty(txtIBAN.Text))
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
                else if (txtIBAN.Text.Length != 22)
                {
                    lblResultado.Text = "El IBAN debe ser de 22 digitos.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else
                {
                    PagoFavorito pagoFavorito = new PagoFavorito()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoCuenta = Int32.Parse(txtCodigoCuenta.Text),
                        Nombre = txtNombre.Text,
                        IBAN = txtIBAN.Text
                    };

                    PagoFavorito pagoFavoritoActualizado = await pagoFavoritoManager.Actualizar(pagoFavorito, Session["Token"].ToString());

                    lblResultado.Text = "Pago favorito actualizado con exito";
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

                PagoFavorito pagoFavorito = await pagoFavoritoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(pagoFavorito.CodigoCuenta.ToString())
                    || !string.IsNullOrEmpty(pagoFavorito.Nombre.ToString()))
                {
                    ltrModalMensaje.Text = "Pago favorito eliminada";
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
                    Vista = "frmPagoFavorito.aspx",
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
            ltrTituloMantenimiento.Text = "Nuevo pago favorito";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodigoCuenta.Visible = true;
            txtCodigoCuenta.Visible = true;

            ltrNombre.Visible = true;
            txtNombre.Visible = true;

            ltrIBAN.Visible = true;
            txtIBAN.Visible = true;

            txtCodigoMant.Text = string.Empty;
            txtCodigoCuenta.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtIBAN.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvPagoFavoritos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvPagoFavoritos.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar pago favorito";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoCuenta.Text = row.Cells[1].Text.Trim();
                    txtNombre.Text = row.Cells[2].Text.Trim();
                    txtIBAN.Text = row.Cells[3].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el pago favorito?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

        

        //private async void existeLlaveForanea()
        //{
        //    List<PagoFavorito> = await pagoFavoritoManager.ObtenerPagoFavoritos(Session["Token"].ToString());
        //    lblPrueba.Text = pagoFavoritos.Where(pagoFavoritos => pagoFavoritos.Codigo == 3).ToString();
        //}

    }
}