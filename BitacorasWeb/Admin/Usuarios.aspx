<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Usuarios.aspx.cs" Inherits="BitacorasWeb.Admin.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <h2 class="mb-3">Gestión de Usuarios</h2>

        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" />

        <div class="card mb-4">
            <div class="card-body">

                <asp:HiddenField ID="hfIdUsuario" runat="server" />

                <div class="row g-3">

                    <div class="col-md-3">
                        <label class="form-label">UsuarioLogin</label>
                        <asp:TextBox ID="txtUsuarioLogin" runat="server" CssClass="form-control" />
                    </div>

                    <div class="col-md-3">
                        <label class="form-label">Código trabajador (contraseña inicial)</label>
                        <asp:TextBox ID="txtCodigoTrabajador" runat="server" CssClass="form-control" />
                    </div>

                    <div class="col-md-3">
                        <label class="form-label">Nombres</label>
                        <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control" />
                    </div>

                    <div class="col-md-3">
                        <label class="form-label">Apellidos</label>
                        <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control" />
                    </div>

                    <div class="col-md-3">
                        <label class="form-label">Rol</label>
                        <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlRol_SelectedIndexChanged" />
                    </div>

                    <div class="col-md-3">
                        <label class="form-label">Activo</label>
                        <asp:DropDownList ID="ddlActivo" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Sí" Value="1" />
                            <asp:ListItem Text="No" Value="0" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-3" id="divTurnero" runat="server" visible="false">
                        <label class="form-label">Es Turnero</label>
                        <asp:DropDownList ID="ddlEsTurnero" runat="server" CssClass="form-select">
                            <asp:ListItem Text="No" Value="0" />
                            <asp:ListItem Text="Sí" Value="1" />
                        </asp:DropDownList>
                    </div>

                </div>

                <div class="mt-3 d-flex gap-2">
                    <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary"
                        Text="Guardar" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" CssClass="btn btn-outline-secondary"
                        Text="Limpiar" OnClick="btnLimpiar_Click" />
                </div>

            </div>
        </div>

        <div class="card">
            <div class="card-body">
                <asp:GridView ID="gvUsuarios" runat="server" CssClass="table table-bordered table-hover"
                    AutoGenerateColumns="false" DataKeyNames="IdUsuario"
                    OnRowCommand="gvUsuarios_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="IdUsuario" HeaderText="Id" />
                        <asp:BoundField DataField="UsuarioLogin" HeaderText="UsuarioLogin" />
                        <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre" />
                        <asp:BoundField DataField="Rol" HeaderText="Rol" />
                        <asp:BoundField DataField="ActivoTexto" HeaderText="Activo" />
                        <asp:BoundField DataField="EsTurneroTexto" HeaderText="Turnero" />

                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CssClass="btn btn-sm btn-outline-primary me-1"
                                    CommandName="EDITAR" CommandArgument='<%# Eval("IdUsuario") %>' Text="Editar" />

                                <asp:LinkButton runat="server" CssClass="btn btn-sm btn-outline-warning me-1"
                                    CommandName="RESET" CommandArgument='<%# Eval("IdUsuario") %>' Text="Reset Pass" />

                                <!-- Sin expresión aquí para evitar romper el parser -->
                                <asp:LinkButton runat="server" CssClass="btn btn-sm btn-outline-danger"
                                    CommandName="TOGGLE" CommandArgument='<%# Eval("IdUsuario") %>' Text="Cambiar estado" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>

    </div>

</asp:Content>

