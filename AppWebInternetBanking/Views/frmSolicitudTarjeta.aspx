<%@ Page Async="true" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="frmSolicitudTarjeta.aspx.cs" Inherits="AppWebInternetBanking.Views.frmSolicitudTarjeta" %>

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
    <h1>Mantenimiento de solicitud de tarjetas</h1>
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvSolicitudTarjetas" runat="server" AutoGenerateColumns="false"
      CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" 
        HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White" 
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvSolicitudTarjetas_RowCommand" >
        <Columns>
            <asp:BoundField HeaderText="Codigo" DataField="Codigo" />
            <asp:BoundField HeaderText="Codigo Cuenta" DataField="CodigoCuenta" />
            <asp:BoundField HeaderText="Estado Civil" DataField="EstadoCivil" />
            <asp:BoundField HeaderText="Condicion Laboral" DataField="CondicionLaboral" />
            <asp:BoundField HeaderText="Ingreso Mensual" DataField="IngresoMensual" />
            <asp:BoundField HeaderText="Producto Deseado" DataField="ProductoDeseado" />
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
                  <td><asp:Literal Text="Estado Civil" runat="server" /></td>
                  <td> <asp:DropDownList ID="ddlEstadoCivil"  CssClass="form-control" runat="server">
                    <asp:ListItem Value="Soltero">Soltero</asp:ListItem>
                    <asp:ListItem Value="Unión Libre">Unión Libre</asp:ListItem>
                    <asp:ListItem Value="Divorciado">Divorciado</asp:ListItem>
                    <asp:ListItem Value="Soltero">Soltero</asp:ListItem>
                    <asp:ListItem Value="Viudo">Viudo</asp:ListItem>
                </asp:DropDownList></td>
              </tr>
              <tr>
                  <td><asp:Literal Text="Condición Laboral" runat="server" /></td>
                  <td> <asp:DropDownList ID="ddlCondicionLaboral"  CssClass="form-control" runat="server">
                    <asp:ListItem Value="Asalariado">Asalariado</asp:ListItem>
                    <asp:ListItem Value="Independiente">Independiente</asp:ListItem>
                    <asp:ListItem Value="Mixto">Mixto</asp:ListItem>
                </asp:DropDownList></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrIngresoMensual" Text="Ingreso Mensual" runat="server" /></td>
                  <td><asp:TextBox ID="txtIngresoMensual" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event)"/></td>
                  <%--<asp:RegularExpressionValidator ID="valIngresoMensual" ControlToValidate="txtIngresoMensual" runat="server" ErrorMessage="Solo se admiten numeros en el espacio de prima" ValidationExpression="^[1-9][.\d]*(,\d+)?$"></asp:RegularExpressionValidator>--%>
              </tr>
              <tr>
                  <td><asp:Literal Text="Producto Deseado" runat="server" /></td>
                  <td> <asp:DropDownList ID="ddlProductoDeseado"  CssClass="form-control" runat="server">
                    <asp:ListItem Value="Cashback">Cashback</asp:ListItem>
                    <asp:ListItem Value="Puntos">Puntos</asp:ListItem>
                    <asp:ListItem Value="Premios">Premios</asp:ListItem>
                    <asp:ListItem Value="PriceSmart">PriceSmart</asp:ListItem>
                    <asp:ListItem Value="Walmart">Walmart</asp:ListItem>
                </asp:DropDownList></td>
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
        <h4 class="modal-title">Mantenimiento de servicios</h4>
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
