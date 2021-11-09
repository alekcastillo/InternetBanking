<%@ Page Async="true" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="frmTarjetaDeCredito.aspx.cs" Inherits="AppWebInternetBanking.Views.frmTarjetaDeCredito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        
       function openModal() {
                 $('#myModal').modal('show'); //ventana de mensajes
        }

        function openModalMantenimiento() {
            $('#myModalMantenimiento').modal('show'); //ventana de mantenimiento
        }    

        function CloseModal() {
            $('#myModal').modal('hide');//cierra ventana de mensajes
        }

        function CloseMantenimiento() {
            $('#myModalMantenimiento').modal('hide'); //cierra ventana de mantenimiento
        }

        $(document).ready(function () { //filtrar el datagridview
            $("#myInput").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#MainContent_gvServicios tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <asp:Label ID="lblPrueba" ForeColor="Black" runat="server" Visible="true" />
    <h1>Mantenimiento de tarjetas de credito</h1>
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvTarjetaDeCreditos" runat="server" AutoGenerateColumns="false"
      CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" 
        HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White" 
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvTarjetaDeCreditos_RowCommand" >
        <Columns>
            <asp:BoundField HeaderText="Codigo" DataField="Codigo" />
            <asp:BoundField HeaderText="Codigo Cuenta" DataField="CodigoCuenta" />
            <asp:BoundField HeaderText="Numero de Tarjeta" DataField="NumeroDeTarjeta" />
            <asp:BoundField HeaderText="Tipo de Tarjeta" DataField="Tipo" />
            <asp:BoundField HeaderText="Mes de expiracion" DataField="MesExpiracion" />
            <asp:BoundField HeaderText="Año de expiracion" DataField="AnoExpiracion" />
            <asp:BoundField HeaderText="CV2" DataField="CV2" />
            <asp:ButtonField HeaderText="Modificar" CommandName="Modificar" 
                ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" />
            <asp:ButtonField HeaderText="Eliminar" CommandName="Eliminar"
                ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" />
        </Columns>
    </asp:GridView>
    <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
      Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo"    />
    <br />
    <asp:Label ID="lblStatus" ForeColor="Maroon" runat="server" Visible="false" />


      <!--VENTANA DE MANTENIMIENTO -->
  <div id="myModalMantenimiento" class="modal fade" role="dialog">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title"><asp:Literal ID="ltrTituloMantenimiento" runat="server"></asp:Literal></h4>
      </div>
      <div class="modal-body">
          <table style="width: 100%;">
              <tr>
                  <td><asp:Literal ID="ltrCodigoMant" Text="Codigo" runat="server" /></td>
                  <td><asp:TextBox ID="txtCodigoMant" runat="server" Enabled="false" CssClass="form-control" /></td>
              </tr>
              <tr>
                  
                  <td><asp:Literal ID="ltrCodigoCuenta" Text="Codigo Cuenta" runat="server" /></td>
                  <td><asp:TextBox ID="txtCodigoCuenta" runat="server" Enabled="true" CssClass="form-control" onkeypress="return isNumberKey(event)"/></td>
                  <%--<asp:RegularExpressionValidator ID="valCodigoCuenta" ControlToValidate="txtCodigoCuenta" runat="server" ErrorMessage="Solo se permiten espacios en el espacio de código" ValidationExpression="\d+"></asp:RegularExpressionValidator>--%>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrNumeroDeTarjeta" Text="Numero De Tarjeta" runat="server" /></td>
                  <td><asp:TextBox ID="txtNumeroDeTarjeta" runat="server" Enabled="true" CssClass="form-control" /></td>
              </tr>
              <tr>
                  <td><asp:Literal Text="Tipo" runat="server" /></td>
                  <td> <asp:DropDownList ID="ddlTipo"  CssClass="form-control" runat="server">
                    <asp:ListItem Value="Visa">Visa</asp:ListItem>
                    <asp:ListItem Value="MasterCard">MasterCard</asp:ListItem>
                    <asp:ListItem Value="Amex">Amex</asp:ListItem>
                </asp:DropDownList></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrMesExpiracion" Text="Mes de Expiracion" runat="server" /></td>
                  <td><asp:TextBox ID="txtMesExpiracion" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event)"/></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrAnoExpiracion" Text="Año de Expiracion" runat="server" /></td>
                  <td><asp:TextBox ID="txtAnoExpiracion" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event)"/></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrCV2" Text="CV2" runat="server" /></td>
                  <td><asp:TextBox ID="txtCV2" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event)"/></td>
              </tr>
          </table>
          <asp:Label ID="lblResultado" ForeColor="Maroon" Visible="False" runat="server" />
      </div>
      <div class="modal-footer">
        <asp:LinkButton type="button" OnClick="btnAceptarMant_Click" CssClass="btn btn-success" ID="btnAceptarMant" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
         <asp:LinkButton type="button" OnClick="btnCancelarMant_Click"  CssClass="btn btn-danger" ID="btnCancelarMant"  runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
      </div>
    </div>
  </div>
</div>

    
     <!-- VENTANA MODAL -->
  <div id="myModal" class="modal fade" role="dialog">
  <div class="modal-dialog modal-sm">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Mantenimiento de tarjetas de credito</h4>
      </div>
      <div class="modal-body">
        <p><asp:Literal ID="ltrModalMensaje" runat="server" /></p>
      </div>
      <div class="modal-footer">
         <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarModal" OnClick="btnAceptarModal_Click"  runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
         <asp:LinkButton type="button"  CssClass="btn btn-danger" ID="btnCancelarModal" OnClick="btnCancelarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
      </div>
    </div>
  </div>
</div>
</asp:Content>
