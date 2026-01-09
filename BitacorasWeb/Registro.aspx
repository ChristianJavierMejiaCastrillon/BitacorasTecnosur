<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="BitacorasWeb.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-container mt-4" style="max-width: 720px">
        <%-- un poco más ancho que Login --%>
        <h2 class="mb-4">Registro de Novedades</h2>

        <!-- Muestra de errores en bloque -->
        <asp:ValidationSummary ID="vsResumen" runat="server" CssClass="alert alert-danger"
            HeaderText="Por favor corrige los siguientes campos:" DisplayMode="BulletList" />

        <div class="row g-3">
            <!-- Fecha -->
            <div class="col-md-6">
                <label for="txtFecha" class="form-label">Fecha</label>
                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFecha"
                    ErrorMessage="La fecha es obligatoria" CssClass="text-danger" Display="Dynamic" />
            </div>

            <!-- Turno -->
            <div class="col-md-6">
                <label for="ddlTurno" class="form-label">Turno</label>
                <asp:DropDownList ID="ddlTurno" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Seleccione..." Value="" />
                    <asp:ListItem>Turno 1</asp:ListItem>
                    <asp:ListItem>Turno 2</asp:ListItem>
                    <asp:ListItem>Turno 3</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlTurno"
                    InitialValue="" ErrorMessage="Selecciona el turno" CssClass="text-danger" Display="Dynamic" />
            </div>

            <!-- Máquina -->
            <div class="col-md-6">
                <label for="ddlMaquina" class="form-label">Máquina</label>

                <asp:DropDownList ID="ddlMaquina" runat="server" CssClass="form-select">
                </asp:DropDownList>

                <asp:RequiredFieldValidator runat="server" 
                    ControlToValidate="ddlMaquina"
                    InitialValue="0" 
                    ErrorMessage="Selecciona la máquina"
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <!-- Producto -->
            <div class="col-md-6">
                <label for="ddlProducto" class="form-label">Producto</label>
                <asp:DropDownList ID="ddlProducto" runat="server" CssClass="form-select">
                </asp:DropDownList>

                <asp:RequiredFieldValidator runat="server"
                    ControlToValidate="ddlProducto"
                    InitialValue="0" 
                    ErrorMessage="Selecciona el producto" 
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <!-- Operario -->
            <div class="col-md-6">
                <label for="ddlOperario" class="form-label">Operario</label>

                <asp:DropDownList ID="ddlOperario" runat="server" CssClass="form-select">
                </asp:DropDownList>

                <asp:RequiredFieldValidator runat="server" 
                    ControlToValidate="ddlOperario"
                    InitialValue="0"
                    ErrorMessage="El operario es obligatorio" 
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <!-- Tipo de novedad -->
            <div class="col-md-6">
                <label for="ddlTipo" class="form-label">Tipo de novedad</label>
                <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Seleccione..." Value="" />
                    <asp:ListItem>Calidad</asp:ListItem>
                    <asp:ListItem>Producción</asp:ListItem>
                    <asp:ListItem>Mantenimiento</asp:ListItem>
                    <asp:ListItem>Seguridad</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlTipo"
                    InitialValue="" ErrorMessage="Selecciona el tipo de novedad" CssClass="text-danger" Display="Dynamic" />
            </div>

            <!-- Descripción -->
            <div class="col-12">
                <label for="txtDescripcion" class="form-label">Descripción</label>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDescripcion"
                    ErrorMessage="Describe la novedad" CssClass="text-danger" Display="Dynamic" />
            </div>

            <!-- Botones -->
            <div class="col-12 d-flex gap-2">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary"
                    OnClick="btnGuardar_Click" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary"
                    CausesValidation="False" OnClick="btnLimpiar_Click" />
            </div>
        </div>
    </div>
</asp:Content>
