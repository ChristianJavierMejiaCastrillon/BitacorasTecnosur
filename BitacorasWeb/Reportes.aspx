<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="BitacorasWeb.Reportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="card shadow-sm p-4">

            <h2 class="mb-4" style="color: #0d6efd;">Reportes de Novedades</h2>

            <asp:Label ID="lblMensaje" runat="server" EnableViewState="false"></asp:Label>

            <div class="row g-3">

                <!-- Fecha -->
                <div class="col-md-4">
                    <label for="txtFecha" class="form-label">Fecha</label>
                    <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>

                <!-- Turno -->
                <div class="col-md-4">
                    <label for="ddlTurno" class="form-label">Turno</label>
                    <asp:DropDownList ID="ddlTurno" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <!-- Máquina -->
                <div class="col-md-4">
                    <label for="ddlMaquina" class="form-label">Máquina</label>
                    <asp:DropDownList ID="ddlMaquina" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <!-- Botones -->
                <div class="col-md-6">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary w-100"
                        OnClick="btnBuscar_Click" />
                </div>

                <div class="col-md-6">
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary w-100"
                        CausesValidation="false" OnClick="btnLimpiar_Click" />
                </div>
            </div>

            <hr class="my-4" />

            <!-- Resultados -->
            <asp:GridView ID="gvNovedades" runat="server" CssClass="table table-striped table-bordered"
                AutoGenerateColumns="false" EmptyDataText="No hay novedades para los filtros seleccionados.">
                <Columns>
                    <asp:BoundField DataField="FechaHoraRegistro" HeaderText="Fecha/Hora"
                        DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Turno" HeaderText="Turno" />
                    <asp:BoundField DataField="Maquina" HeaderText="Máquina" />
                    <asp:BoundField DataField="Producto" HeaderText="Producto" />
                    <asp:BoundField DataField="Operario" HeaderText="Operario" />
                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                </Columns>
            </asp:GridView>

        </div>
    </div>
</asp:Content>

