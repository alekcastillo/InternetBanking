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
    public partial class frmTarjetaDeCredito : System.Web.UI.Page
    {
        IEnumerable<TarjetaDeCredito> tarjetaDeCreditos = new ObservableCollection<TarjetaDeCredito>();
        TarjetaDeCreditoManager tarjetaDeCreditoManager = new TarjetaDeCreditoManager();

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

                tarjetaDeCreditos = await tarjetaDeCreditoManager.ObtenerTarjetaDeCreditos(Session["Token"].ToString());

                gvTarjetaDeCreditos.DataSource = tarjetaDeCreditos.ToList();
                gvTarjetaDeCreditos.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de tarjetas de credito";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());
                

                if (string.IsNullOrEmpty(txtCodigoCuenta.Text) 
                    || string.IsNullOrEmpty(txtNumeroDeTarjeta.Text)
                    || string.IsNullOrEmpty(txtMesExpiracion.Text)
                    || string.IsNullOrEmpty(txtAnoExpiracion.Text)
                    || string.IsNullOrEmpty(txtCV2.Text))
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
                else if (txtNumeroDeTarjeta.Text.Length != 19)
                {
                    lblResultado.Text = "El numero de tarjeta debe ser de 19 digitos.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else if (Int32.Parse(txtMesExpiracion.Text) < 0 || Int32.Parse(txtMesExpiracion.Text) > 12)
                {
                    lblResultado.Text = "El mes de expiracion es invalido.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else if (Int32.Parse(txtAnoExpiracion.Text) < 2021 || Int32.Parse(txtAnoExpiracion.Text) > 2050)
                {
                    lblResultado.Text = "El año de expiracion es invalido.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else if (txtCV2.Text.Length < 3 || txtCV2.Text.Length > 4)
                {
                    lblResultado.Text = "El CV2 debe ser de 3 o 4 digitos.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else
                {
                    TarjetaDeCredito tarjetaDeCredito = new TarjetaDeCredito()
                    {

                        CodigoCuenta = Int32.Parse(txtCodigoCuenta.Text),
                        Tipo = ddlTipo.SelectedValue,
                        NumeroDeTarjeta = txtNumeroDeTarjeta.Text,
                        MesExpiracion = Int32.Parse(txtMesExpiracion.Text),
                        AnoExpiracion = Int32.Parse(txtAnoExpiracion.Text),
                        CV2 = Int32.Parse(txtCV2.Text)

                    };

                    TarjetaDeCredito tarjetaDeCreditoIngresado = await tarjetaDeCreditoManager.Ingresar(tarjetaDeCredito, Session["Token"].ToString());

                    lblResultado.Text = "Tarjeta de credito ingresada con exito";
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
                    || string.IsNullOrEmpty(txtNumeroDeTarjeta.Text)
                    || string.IsNullOrEmpty(txtMesExpiracion.Text)
                    || string.IsNullOrEmpty(txtAnoExpiracion.Text)
                    || string.IsNullOrEmpty(txtCV2.Text))
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
                else if (txtNumeroDeTarjeta.Text.Length != 19)
                {
                    lblResultado.Text = "El numero de tarjeta debe ser de 19 digitos.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else if (Int32.Parse(txtMesExpiracion.Text) < 0 || Int32.Parse(txtMesExpiracion.Text) > 12)
                {
                    lblResultado.Text = "El mes de expiracion es invalido.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else if (Int32.Parse(txtAnoExpiracion.Text) < 2021 || Int32.Parse(txtAnoExpiracion.Text) > 2050)
                {
                    lblResultado.Text = "El año de expiracion es invalido.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else if (txtCV2.Text.Length < 3 || txtCV2.Text.Length > 4)
                {
                    lblResultado.Text = "El CV2 debe ser de 3 o 4 digitos.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
                else
                {
                    TarjetaDeCredito tarjetaDeCredito = new TarjetaDeCredito()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoCuenta = Int32.Parse(txtCodigoCuenta.Text),
                        Tipo = ddlTipo.SelectedValue,
                        NumeroDeTarjeta = txtNumeroDeTarjeta.Text,
                        MesExpiracion = Int32.Parse(txtMesExpiracion.Text),
                        AnoExpiracion = Int32.Parse(txtAnoExpiracion.Text),
                        CV2 = Int32.Parse(txtCV2.Text)
                    };

                    TarjetaDeCredito tarjetaDeCreditoActualizado = await tarjetaDeCreditoManager.Actualizar(tarjetaDeCredito, Session["Token"].ToString());

                    lblResultado.Text = "Tarjeta de credito actualizada con exito";
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

                TarjetaDeCredito tarjetaDeCredito = await tarjetaDeCreditoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(tarjetaDeCredito.CodigoCuenta.ToString())
                    || !string.IsNullOrEmpty(tarjetaDeCredito.NumeroDeTarjeta.ToString()))
                {
                    ltrModalMensaje.Text = "Tarjeta de credito eliminada";
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
                    Vista = "frmTarjetaDeCredito.aspx",
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
            ltrTituloMantenimiento.Text = "Nueva Tarjeta de credito";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodigoCuenta.Visible = true;
            txtCodigoCuenta.Visible = true;

            ltrAnoExpiracion.Visible = true;
            txtAnoExpiracion.Visible = true;

            ltrMesExpiracion.Visible = true;
            txtMesExpiracion.Visible = true;

            ltrCV2.Visible = true;
            txtCV2.Visible = true;

            txtCodigoMant.Text = string.Empty;
            txtCodigoCuenta.Text = string.Empty;
            txtAnoExpiracion.Text = string.Empty;
            txtMesExpiracion.Text = string.Empty;
            txtCV2.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvTarjetaDeCreditos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvTarjetaDeCreditos.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar Tarjeta de credito";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoCuenta.Text = row.Cells[1].Text.Trim();
                    txtNumeroDeTarjeta.Text = row.Cells[2].Text.Trim();
                    ddlTipo.SelectedValue = row.Cells[3].Text.Trim();
                    txtMesExpiracion.Text = row.Cells[4].Text.Trim();
                    txtAnoExpiracion.Text = row.Cells[5].Text.Trim();
                    txtCV2.Text = row.Cells[6].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar la Tarjeta de credito?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

        

        //private async void existeLlaveForanea()
        //{
        //    List<TarjetaDeCredito> = await tarjetaDeCreditoManager.ObtenerTarjetaDeCreditos(Session["Token"].ToString());
        //    lblPrueba.Text = tarjetaDeCreditos.Where(tarjetaDeCreditos => tarjetaDeCreditos.Codigo == 3).ToString();
        //}

    }
}