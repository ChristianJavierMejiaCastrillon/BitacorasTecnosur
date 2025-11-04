<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="BitacorasWeb.Reportes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="form-container mt-4" style="max-width:980px">
    <h2 class="mb-4">Reportes de Novedades</h2>

    <!-- Filtros -->
    <div class="row g-3 mb-3">
      <div class="col-md-4">
        <label class="form-label">Fecha</label>
        <asp:TextBox ID="txtFiltroFecha" runat="server" CssClass="form-control" TextMode="Date" />
      </div>
      <div class="col-md-4">
        <label class="form-label">Turno</label>
        <asp:DropDownList ID="ddlFiltroTurno" runat="server" CssClass="form-select">
          <asp:ListItem Text="Todos" Value="" />
          <asp:ListItem>Turno 1</asp:ListItem>
          <asp:ListItem>Turno 2</asp:ListItem>
          <asp:ListItem>Turno 3</asp:ListItem>
        </asp:DropDownList>
      </div>
      <div class="col-md-4">
        <label class="form-label">Máquina</label>
        <asp:DropDownList ID="ddlFiltroMaquina" runat="server" CssClass="form-select">
          <asp:ListItem Text="Todas" Value="" />
          <asp:ListItem>Laminadora 1</asp:ListItem>
          <asp:ListItem>Empacadora MK-200</asp:ListItem>
          <asp:ListItem>Selladora ZX-5</asp:ListItem>
        </asp:DropDownList>
      </div>
      <div class="col-12 d-flex gap-2">
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary"
          OnClick="btnBuscar_Click" />
        <asp:Button ID="btnLimpiarFiltros" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary"
          CausesValidation="False" OnClick="btnLimpiarFiltros_Click" />
      </div>
    </div>

    <!-- Resultados -->
    <asp:GridView ID="gvNovedades" runat="server" AutoGenerateColumns="False"
        CssClass="table table-striped table-hover"
        HeaderStyle-CssClass="table-primary"
        EmptyDataText="No se encontraron resultados."
        AllowPaging="True" PageSize="8" OnPageIndexChanging="gvNovedades_PageIndexChanging">
      <Columns>
        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:BoundField DataField="Turno" HeaderText="Turno" />
        <asp:BoundField DataField="Maquina" HeaderText="Máquina" />
        <asp:BoundField DataField="Producto" HeaderText="Producto" />
        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
      </Columns>
    </asp:GridView>
  </div>
</asp:Content>

