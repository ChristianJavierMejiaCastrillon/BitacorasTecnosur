<%@ Page Title="Gestión de Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="BitacorasWeb.Admin.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <h2>Gestión de Productos</h2>

        <asp:Label ID="lblMsg" runat="server" CssClass="alert alert-success d-block" Visible="false"></asp:Label>


        <asp:Button ID="btnNuevo" runat="server"
            CssClass="btn btn-primary mb-3"
            Text="Nuevo Producto"
            OnClick="btnNuevo_Click" />

        <div class="form-check">
        <asp:CheckBox ID="chkIncluirInactivos" runat="server"
            CssClass="form-check-input"
            AutoPostBack="true"
            OnCheckedChanged="chkIncluirInactivos_CheckedChanged" />
        <label class="form-check-label" for="<%= chkIncluirInactivos.ClientID %>">
            Incluir inactivos
        </label>
        </div>

        <asp:GridView ID="gvProductos"
            runat="server"
            AutoGenerateColumns="false"
            CssClass="table table-bordered table-hover"
            OnRowCommand="gvProductos_RowCommand">

            <Columns>

                <asp:BoundField DataField="IdProducto" HeaderText="ID" />
                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />

                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>

                        <!-- Reactivar SOLO si está inactivo -->
                        <asp:LinkButton ID="btnReactivar" runat="server"
                            CommandName="Reactivar"
                            CommandArgument='<%# Eval("IdProducto") %>'
                            CssClass="btn btn-sm btn-success"
                            Visible='<%# !(Convert.ToBoolean(Eval("Activo"))) %>'>
                            Reactivar
                        </asp:LinkButton>

                        <!-- Editar -->
                        <asp:LinkButton ID="btnEditar" runat="server"
                            CommandName="Editar"
                            CommandArgument='<%# Eval("IdProducto") %>'
                            CssClass="btn btn-sm btn-warning">
                            Editar
                        </asp:LinkButton>

                        <!-- Desactivar SOLO si está activo -->
                        <asp:LinkButton ID="btnDesactivar" runat="server"
                            CommandName="Desactivar"
                            CommandArgument='<%# Eval("IdProducto") %>'
                            CssClass="btn btn-sm btn-danger"
                            Visible='<%# Convert.ToBoolean(Eval("Activo")) %>'
                            OnClientClick="return confirm('¿Desactivar producto?');">
                            Desactivar
                        </asp:LinkButton>

                    </ItemTemplate>
                </asp:TemplateField>


            </Columns>
        </asp:GridView>

    </div>

</asp:Content>
