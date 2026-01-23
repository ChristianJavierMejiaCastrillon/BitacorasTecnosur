<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Maquina.aspx.cs" Inherits="BitacorasWeb.Admin.Maquina" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="card shadow-sm p-4">

            <div class="d-flex justify-content-between align-items-center mb-3">
                <h2 class="mb-0">Gestión de Máquinas</h2>

                <div class="d-flex gap-2 align-items-center">
                    <asp:Button ID="btnNueva" runat="server" Text="Nueva máquina" CssClass="btn btn-primary"
                        OnClick="btnNueva_Click" />

                    <div class="form-check ms-2">
                        <asp:CheckBox ID="chkIncluirInactivas" runat="server" CssClass="form-check-input"
                            AutoPostBack="true" OnCheckedChanged="chkIncluirInactivas_CheckedChanged" />
                        <label class="form-check-label" for="<%= chkIncluirInactivas.ClientID %>">Incluir inactivas</label>
                    </div>
                </div>
            </div>

            <asp:Label ID="lblMensaje" runat="server" EnableViewState="false"></asp:Label>

            <hr />

            <!-- Grilla -->
            <div class="table-responsive">
                <asp:GridView ID="gvMaquinas" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="IdMaquina" CssClass="table table-striped table-bordered align-middle"
                    OnRowCommand="gvMaquinas_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Codigo" HeaderText="Código" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />

                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <div class="d-flex gap-2">
                                    <asp:Button ID="btnEditar" runat="server" Text="Editar"
                                        CssClass="btn btn-sm btn-outline-primary"
                                        CommandName="Editar" CommandArgument='<%# Eval("IdMaquina") %>' />

                                    <asp:Button ID="btnDesactivar" runat="server" Text="Desactivar"
                                        CssClass="btn btn-sm btn-outline-danger"
                                        CommandName="Desactivar" CommandArgument='<%# Eval("IdMaquina") %>' />

                                    <asp:Button ID="btnReactivar" runat="server" Text="Reactivar"
                                        CssClass="btn btn-sm btn-outline-success"
                                        CommandName="Reactivar" CommandArgument='<%# Eval("IdMaquina") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <!-- Formulario Crear/Editar -->
            <asp:Panel ID="pnlFormulario" runat="server" Visible="false" CssClass="mt-4">
                <hr />
                <h4 class="mb-3">
                    <asp:Label ID="lblTituloFormulario" runat="server" Text="Nueva máquina"></asp:Label>
                </h4>

                <asp:HiddenField ID="hfIdMaquina" runat="server" />

                <div class="row g-3">
                    <div class="col-md-4">
                        <label class="form-label">Código</label>
                        <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-md-4">
                        <label class="form-label">Nombre</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-md-4">
                        <label class="form-label">Descripción</label>
                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-12 d-flex gap-2 mt-2">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success"
                            OnClick="btnGuardar_Click" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                            OnClick="btnCancelar_Click" />
                    </div>
                </div>

            </asp:Panel>

        </div>
    </div>

</asp:Content>
