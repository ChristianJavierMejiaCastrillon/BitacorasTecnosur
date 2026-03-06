<%@ Page Title="Tipos de Novedad" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TipoNovedad.aspx.cs" Inherits="BitacorasWeb.Admin.TiposNovedad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="mb-3">Administración - Tipos de Novedad</h2>

    <asp:Label ID="lblMensaje" runat="server" EnableViewState="false"></asp:Label>

    <div class="card mb-4">
        <div class="card-body">

            <asp:GridView ID="gvTipos" runat="server"
                CssClass="table table-striped table-bordered"
                AutoGenerateColumns="False"
                DataKeyNames="IdTipoNovedad"
                OnRowCommand="gvTipos_RowCommand">

                <Columns>

                    <asp:BoundField DataField="IdTipoNovedad" HeaderText="ID" />

                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />

                    <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />

                    <asp:BoundField DataField="FechaCreacion"
                        HeaderText="Fecha"
                        DataFormatString="{0:yyyy-MM-dd HH:mm}" />

                     <%-- ACCIONES --%>
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>

                            <!-- EDITAR -->
                            <asp:LinkButton
                                runat="server"
                                CssClass="btn btn-sm btn-warning me-2"
                                CommandName="Editar"
                                CommandArgument='<%# Eval("IdTipoNovedad") %>'
                                Text="Editar"
                                CausesValidation="false" />

                            <!-- ACTIVAR / DESACTIVAR -->
                            <asp:LinkButton
                                runat="server"
                                CssClass='<%# (Convert.ToBoolean(Eval("Activo")) ? "btn btn-sm btn-danger" : "btn btn-sm btn-success") %>'
                                CommandName="Toggle"
                                CommandArgument='<%# Eval("IdTipoNovedad") %>'
                                Text='<%# (Convert.ToBoolean(Eval("Activo")) ? "Desactivar" : "Activar") %>'
                                CausesValidation="false"
                                OnClientClick='<%# (Convert.ToBoolean(Eval("Activo")) ? "return confirm(\"¿Seguro que deseas desactivar este tipo de novedad?\");" : "") %>' />

                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

        </div>
    </div>


    <!-- FORMULARIO CREAR / EDITAR -->

    <div class="card">
        <div class="card-body">

            <h5 class="mb-3">Crear / Editar Tipo</h5>

            <asp:HiddenField ID="hfIdTipoNovedad" runat="server" Value="0" />

            <div class="row g-3">

                <div class="col-md-6">

                    <label class="form-label">Nombre</label>

                    <asp:TextBox
                        ID="txtNombre"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="50">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="txtNombre"
                        ErrorMessage="El nombre es obligatorio"
                        CssClass="text-danger"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                </div>

                <div class="col-12 d-flex gap-2 mt-2">

                    <asp:Button
                        ID="btnGuardar"
                        runat="server"
                        Text="Guardar"
                        CssClass="btn btn-primary"
                        OnClick="btnGuardar_Click" />

                    <asp:Button
                        ID="btnLimpiar"
                        runat="server"
                        Text="Limpiar"
                        CssClass="btn btn-outline-secondary"
                        CausesValidation="false"
                        OnClick="btnLimpiar_Click" />

                </div>

            </div>

        </div>
    </div>
</asp:Content>