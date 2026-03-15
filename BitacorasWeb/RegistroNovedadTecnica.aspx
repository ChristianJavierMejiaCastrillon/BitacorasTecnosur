<%@ Page Title="Registro Novedad Técnica" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistroNovedadTecnica.aspx.cs" Inherits="BitacorasWeb.RegistroNovedadTecnica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <div class="card shadow-sm">
            <div class="card-body">

                <h2 class="text-primary mb-4">Registro de Novedad Técnica</h2>

                <asp:HiddenField ID="hfIdNovedadTecnica" runat="server" />

                <asp:Label ID="lblMensaje" runat="server" EnableViewState="false"></asp:Label>

                <div class="row g-3">

                    <!-- Fecha -->
                    <div class="col-md-6">
                        <label for="txtFecha" class="form-label">Fecha</label>
                        <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFecha" runat="server"
                            ControlToValidate="txtFecha"
                            ErrorMessage="La fecha es obligatoria."
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <!-- Turno -->
                    <div class="col-md-6">
                        <label for="ddlTurno" class="form-label">Turno</label>
                        <asp:DropDownList ID="ddlTurno" runat="server" CssClass="form-select"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTurno" runat="server"
                            ControlToValidate="ddlTurno"
                            InitialValue="0"
                            ErrorMessage="Seleccione un turno."
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <!-- Máquina -->
                    <div class="col-md-4">
                        <label for="ddlMaquina" class="form-label">Máquina</label>
                        <asp:DropDownList ID="ddlMaquina" runat="server" CssClass="form-select"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlMaquina_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvMaquina" runat="server"
                            ControlToValidate="ddlMaquina"
                            InitialValue="0"
                            ErrorMessage="Seleccione una máquina."
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <!-- Módulo -->
                    <div class="col-md-4">
                        <label for="ddlModulo" class="form-label">Módulo</label>
                        <asp:DropDownList ID="ddlModulo" runat="server" CssClass="form-select"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlModulo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>

                    <!-- Elemento -->
                    <div class="col-md-4">
                        <label for="ddlElemento" class="form-label">Elemento</label>
                        <asp:DropDownList ID="ddlElemento" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>

                    <!-- Tipo novedad técnica -->
                    <div class="col-md-6">
                        <label for="ddlTipoNovedadTecnica" class="form-label">Tipo de novedad técnica</label>
                        <asp:DropDownList ID="ddlTipoNovedadTecnica" runat="server" CssClass="form-select"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTipoNovedadTecnica" runat="server"
                            ControlToValidate="ddlTipoNovedadTecnica"
                            InitialValue="0"
                            ErrorMessage="Seleccione el tipo de novedad técnica."
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <!-- Tipo mantenimiento -->
                    <div class="col-md-6">
                        <label for="ddlTipoMantenimiento" class="form-label">Tipo de mantenimiento</label>
                        <asp:DropDownList ID="ddlTipoMantenimiento" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Seleccione..." Value="0"></asp:ListItem>
                            <asp:ListItem Text="Correctivo" Value="Correctivo"></asp:ListItem>
                            <asp:ListItem Text="Preventivo" Value="Preventivo"></asp:ListItem>
                            <asp:ListItem Text="Predictivo" Value="Predictivo"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTipoMantenimiento" runat="server"
                            ControlToValidate="ddlTipoMantenimiento"
                            InitialValue="0"
                            ErrorMessage="Seleccione el tipo de mantenimiento."
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <!-- Tiempo perdido -->
                    <div class="col-md-6">
                        <label for="txtTiempoPerdido" class="form-label">Tiempo perdido (min)</label>
                        <asp:TextBox ID="txtTiempoPerdido" runat="server" CssClass="form-control" placeholder="Ej: 15, 30, 60"></asp:TextBox>
                    </div>

                    <!-- Usuario responsable -->
                    <div class="col-md-6">
                        <label for="ddlUsuarioResponsable" class="form-label">Responsable técnico</label>
                        <asp:DropDownList ID="ddlUsuarioResponsable" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>

                    <!-- Descripción -->
                    <div class="col-12">
                        <label for="txtDescripcion" class="form-label">Descripción</label>
                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server"
                            ControlToValidate="txtDescripcion"
                            ErrorMessage="La descripción es obligatoria."
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <!-- Diagnóstico -->
                    <div class="col-12">
                        <label for="txtDiagnostico" class="form-label">Diagnóstico</label>
                        <asp:TextBox ID="txtDiagnostico" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                    </div>

                    <!-- Solución -->
                    <div class="col-12">
                        <label for="txtSolucionAplicada" class="form-label">Solución aplicada</label>
                        <asp:TextBox ID="txtSolucionAplicada" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                    </div>

                    <!-- Botones -->
                    <div class="col-md-6 d-grid">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar"
                            CssClass="btn btn-primary"
                            OnClick="btnGuardar_Click" />
                    </div>

                    <div class="col-md-6 d-grid">
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar"
                            CssClass="btn btn-outline-secondary"
                            CausesValidation="false"
                            OnClick="btnLimpiar_Click" />
                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>