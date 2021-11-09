<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmSeguro.aspx.cs" Inherits="AppWebInternetBanking.Views.frmSeguro" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

   <script type="text/javascript">
        
       function openModal() {
                 $('#myModal').modal('show'); 
        }

        function openModalMantenimiento() {
            $('#myModalMantenimiento').modal('show'); 
        }    

        function CloseModal() {
            $('#myModal').modal('hide');
        }

        function CloseMantenimiento() {
            $('#myModalMantenimiento').modal('hide'); 
        }

        $(document).ready(function () { 
            $("#myInput").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#MainContent_gvSeguro tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
   </script> 
    

    <h1>Mantenimiento de Seguros</h1>
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />

    <asp:GridView ID="gvseguros" runat="server" AutoGenerateColumns="false"
      CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" 
        HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White" 
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvSeguro_RowCommand" >

        <Columns>
            <asp:BoundField HeaderText="Codigo" DataField="Codigo" />
            <asp:BoundField HeaderText="CodigoCuenta" DataField="CodigoCuenta" />
            <asp:BoundField HeaderText="FechaHoraInicio" DataField="FechaHoraInicio" />
            <asp:BoundField HeaderText="FechaHoraVencimiento" DataField="FechaHoraVencimiento" />
            <asp:BoundField HeaderText="Prima" DataField="Prima" />
            <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" />
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
                  <td><asp:Literal ID="ltrCodigoCuenta" Text="CodigoCuenta" runat="server" /></td>
                  <td><asp:TextBox ID="txtCodigoCuenta" runat="server" Enabled="true" CssClass="form-control" /></td>
                   <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtCodigoCuenta" runat="server" ErrorMessage="Solo se admiten codigos validos en el espacio Codigo Cuenta" ValidationExpression="\d+">
                       </asp:RegularExpressionValidator>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrFechaHoraInicio" Text="Fecha/HoraInicio" runat="server" /></td>
                  <td><asp:TextBox ID="txtFechaHoraInicio" TextMode="Date" runat="server" CssClass="form-control" /></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrFechaHoraVencimiento" Text="Fecha/HoraVencimiento" runat="server" /></td>
                  <td><asp:TextBox ID="txtFechaHoraVencimiento" TextMode="Date" runat="server" CssClass="form-control" /></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrPrima" Text="Prima" runat="server" /></td>
                  <td><asp:TextBox ID="txtPrima" TextMode= "Number" runat="server" CssClass="form-control" /></td>
                   <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtPrima" runat="server" ErrorMessage="Solo se admiten numeros en el espacio de prima" ValidationExpression="^[1-9][\.\d]*(,\d+)?$">
                       </asp:RegularExpressionValidator>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrDescripcion" Text="Descripcion" runat="server" /></td>
                  <td><asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" /></td>
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
        <h4 class="modal-title">Mantenimiento de Seguro</h4>
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


