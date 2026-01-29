<%@ Page Title="Estructura de Máquina" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EstructuraMaquina.aspx.cs" Inherits="BitacorasWeb.Admin.EstructuraMaquina" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <div class="card shadow-sm p-4">

            <div class="d-flex justify-content-between align-items-center mb-3">
                <h2 class="mb-0">Estructura de Máquina</h2>
            </div>

            <asp:Label ID="lblMensaje" runat="server" EnableViewState="false"></asp:Label>

            <div class="row g-3 align-items-end">
                <div class="col-md-6">
                    <label class="form-label">Máquina</label>
                    <asp:DropDownList ID="ddlMaquina" runat="server" CssClass="form-select"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlMaquina_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <div class="col-md-6 d-flex gap-2">
                    <asp:Button ID="btnNuevoModulo" runat="server" Text="Nuevo módulo" CssClass="btn btn-primary"
                        OnClick="btnNuevoModulo_Click" />
                    <asp:CheckBox ID="chkIncluirInactivosModulos" runat="server" CssClass="form-check-input ms-2"
                        AutoPostBack="true" OnCheckedChanged="chkIncluirInactivosModulos_CheckedChanged" />
                    <label class="form-check-label ms-2" for="<%= chkIncluirInactivosModulos.ClientID %>">Incluir inactivos</label>
                </div>
            </div>

            <hr />

            <!-- MODULOS -->
            <h4 class="mb-3">Módulos</h4>

            <div class="table-responsive">
                <asp:GridView ID="gvModulos" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-striped table-bordered align-middle"
                    DataKeyNames="IdMaquinaModulo" OnRowCommand="gvModulos_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <div class="d-flex gap-2">
                                    <asp:Button runat="server" Text="Elementos" CssClass="btn btn-sm btn-outline-secondary"
                                        CommandName="Elementos" CommandArgument='<%# Eval("IdMaquinaModulo") %>' />
                                    <asp:Button runat="server" Text="Editar" CssClass="btn btn-sm btn-outline-primary"
                                        CommandName="Editar" CommandArgument='<%# Eval("IdMaquinaModulo") %>' />
                                    <asp:Button runat="server" Text="Desactivar" CssClass="btn btn-sm btn-outline-danger"
                                        CommandName="Desactivar" CommandArgument='<%# Eval("IdMaquinaModulo") %>' />
                                    <asp:Button runat="server" Text="Reactivar" CssClass="btn btn-sm btn-outline-success"
                                        CommandName="Reactivar" CommandArgument='<%# Eval("IdMaquinaModulo") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <!-- Form Modulo -->
            <asp:Panel ID="pnlModulo" runat="server" Visible="false" CssClass="mt-4">
                <hr />
                <h5 class="mb-3"><asp:Label ID="lblTituloModulo" runat="server" /></h5>
                <asp:HiddenField ID="hfIdMaquinaModulo" runat="server" />

                <div class="row g-3">
                    <div class="col-md-4">
                        <label class="form-label">Nombre</label>
                        <asp:TextBox ID="txtModuloNombre" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-8">
                        <label class="form-label">Descripción</label>
                        <asp:TextBox ID="txtModuloDesc" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-12 d-flex gap-2">
                        <asp:Button ID="btnGuardarModulo" runat="server" Text="Guardar módulo" CssClass="btn btn-success"
                            OnClick="btnGuardarModulo_Click" />
                        <asp:Button ID="btnCancelarModulo" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                            OnClick="btnCancelarModulo_Click" />
                    </div>
                </div>
            </asp:Panel>

            <!-- ELEMENTOS -->
            <asp:Panel ID="pnlElementos" runat="server" Visible="false" CssClass="mt-4">
                <hr />
                <div class="d-flex justify-content-between align-items-center mb-2">
                    <h4 class="mb-0">Elementos del módulo: <asp:Label ID="lblModuloActual" runat="server" /></h4>

                    <div class="d-flex gap-2 align-items-center">
                        <asp:Button ID="btnNuevoElemento" runat="server" Text="Nuevo elemento" CssClass="btn btn-primary"
                            OnClick="btnNuevoElemento_Click" />
                        <asp:CheckBox ID="chkIncluirInactivosElementos" runat="server" CssClass="form-check-input ms-2"
                            AutoPostBack="true" OnCheckedChanged="chkIncluirInactivosElementos_CheckedChanged" />
                        <label class="form-check-label ms-2" for="<%= chkIncluirInactivosElementos.ClientID %>">Incluir inactivos</label>
                    </div>
                </div>

                <asp:HiddenField ID="hfIdModuloElementos" runat="server" />

                <div class="table-responsive">
                    <asp:GridView ID="gvElementos" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-striped table-bordered align-middle"
                        DataKeyNames="IdMaquinaModuloElemento" OnRowCommand="gvElementos_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <div class="d-flex gap-2">
                                        <asp:Button runat="server" Text="Editar" CssClass="btn btn-sm btn-outline-primary"
                                            CommandName="Editar" CommandArgument='<%# Eval("IdMaquinaModuloElemento") %>' />
                                        <asp:Button runat="server" Text="Desactivar" CssClass="btn btn-sm btn-outline-danger"
                                            CommandName="Desactivar" CommandArgument='<%# Eval("IdMaquinaModuloElemento") %>' />
                                        <asp:Button runat="server" Text="Reactivar" CssClass="btn btn-sm btn-outline-success"
                                            CommandName="Reactivar" CommandArgument='<%# Eval("IdMaquinaModuloElemento") %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <!-- Form Elemento -->
                <asp:Panel ID="pnlElementoForm" runat="server" Visible="false" CssClass="mt-3">
                    <h5 class="mb-3"><asp:Label ID="lblTituloElemento" runat="server" /></h5>
                    <asp:HiddenField ID="hfIdElemento" runat="server" />

                    <div class="row g-3">
                        <div class="col-md-4">
                            <label class="form-label">Nombre</label>
                            <asp:TextBox ID="txtElementoNombre" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-8">
                            <label class="form-label">Descripción</label>
                            <asp:TextBox ID="txtElementoDesc" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 d-flex gap-2">
                            <asp:Button ID="btnGuardarElemento" runat="server" Text="Guardar elemento" CssClass="btn btn-success"
                                OnClick="btnGuardarElemento_Click" />
                            <asp:Button ID="btnCancelarElemento" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                                OnClick="btnCancelarElemento_Click" />
                        </div>
                    </div>
                </asp:Panel>

            </asp:Panel>

        </div>
    </div>

</asp:Content>
