<%@ Page Title="Asignaciones" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Asignaciones.aspx.cs" Inherits="BitacorasWeb.Admin.Asignaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <h3 class="mb-3">Asignaciones Usuario ↔ Máquina</h3>

        <!-- Mensajes -->
        <asp:Label ID="lblMsg" runat="server" EnableViewState="false" />

        <div class="row mt-3">
            <div class="col-md-4">
                <label class="form-label">Usuario</label>
                <asp:DropDownList ID="ddlUsuarios" runat="server"
                    CssClass="form-select"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlUsuarios_SelectedIndexChanged" />
            </div>

            <div class="col-md-4">
                <label class="form-label">Máquina</label>
                <asp:DropDownList ID="ddlMaquinas" runat="server" CssClass="form-select" />
            </div>

            <div class="col-md-4">
                <label class="form-label">Tipo asignación</label>
                <asp:DropDownList ID="ddlTipoAsignacion" runat="server" CssClass="form-select" />
            </div>
        </div>

        <div class="row mt-3 align-items-end">
            <div class="col-md-4">
                <label class="form-label">Fecha inicio</label>
                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" TextMode="Date" />
                <div class="form-text">Si lo dejas vacío, se tomará hoy.</div>
            </div>

            <div class="col-md-4">
                <asp:Button ID="btnAsignar" runat="server" Text="Asignar"
                    CssClass="btn btn-primary w-100"
                    OnClick="btnAsignar_Click" />
            </div>

            <div class="col-md-4">
                <asp:Button ID="btnRefrescar" runat="server" Text="Refrescar lista"
                    CssClass="btn btn-outline-secondary w-100"
                    OnClick="btnRefrescar_Click" />
            </div>
        </div>

        <hr class="mt-4" />

        <div class="d-flex align-items-center justify-content-between">
            <h5 class="mb-0">Asignaciones del usuario seleccionado</h5>
        </div>

        <asp:GridView ID="gvAsignaciones" runat="server"
            CssClass="table table-striped table-bordered mt-2"
            AutoGenerateColumns="false"
            OnRowCommand="gvAsignaciones_RowCommand"
            DataKeyNames="IdUsuarioMaquina"
            EmptyDataText="No hay asignaciones registradas para este usuario.">

            <Columns>
                <asp:BoundField DataField="Maquina" HeaderText="Máquina" />

                <asp:BoundField DataField="TipoCodigo" HeaderText="Tipo (Código)" />
                <asp:BoundField DataField="TipoNombre" HeaderText="Tipo (Nombre)" />

                <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />

                <asp:BoundField DataField="FechaInicio" HeaderText="Inicio" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="FechaFin" HeaderText="Fin" DataFormatString="{0:yyyy-MM-dd}" />

                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:Button runat="server"
                            Text="Finalizar"
                            CssClass="btn btn-sm btn-danger me-1"
                            CommandName="FINALIZAR"
                            CommandArgument='<%# Eval("IdUsuarioMaquina") %>'
                            OnClientClick="return confirm('¿Finalizar esta asignación?');"
                            Visible='<%# Convert.ToBoolean(Eval("Activo")) %>' />

                        <asp:Button runat="server"
                            Text="Reactivar"
                            CssClass="btn btn-sm btn-success"
                            CommandName="REACTIVAR"
                            CommandArgument='<%# Eval("IdUsuarioMaquina") %>'
                            OnClientClick="return confirm('¿Reactivar esta asignación?');"
                            Visible='<%# !Convert.ToBoolean(Eval("Activo")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

    </div>

</asp:Content>
