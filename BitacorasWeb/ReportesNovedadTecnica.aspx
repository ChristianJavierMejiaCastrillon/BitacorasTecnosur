<%@ Page Title="Reportes de Novedades Técnicas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportesNovedadTecnica.aspx.cs" Inherits="BitacorasWeb.ReportesNovedadTecnica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <div class="card shadow-sm">
            <div class="card-body">

                <h2 class="text-primary mb-4">Reportes de Novedades Técnicas</h2>

                <asp:Label ID="lblMensaje" runat="server" EnableViewState="false"></asp:Label>

                <div class="row g-3">

                    <!-- Fecha desde -->
                    <div class="col-md-4">
                        <label for="txtFechaDesde" class="form-label">Fecha desde</label>
                        <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>

                    <!-- Fecha hasta -->
                    <div class="col-md-4">
                        <label for="txtFechaHasta" class="form-label">Fecha hasta</label>
                        <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>

                    <!-- Turno -->
                    <div class="col-md-4">
                        <label for="ddlTurno" class="form-label">Turno</label>
                        <asp:DropDownList ID="ddlTurno" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>

                    <!-- Máquina -->
                    <div class="col-md-4">
                        <label for="ddlMaquina" class="form-label">Máquina</label>
                        <asp:DropDownList ID="ddlMaquina" runat="server" CssClass="form-select"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlMaquina_SelectedIndexChanged">
                        </asp:DropDownList>
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

                    <!-- Tipo de novedad -->
                    <div class="col-md-4">
                        <label for="ddlTipoNovedadTecnica" class="form-label">Tipo de novedad</label>
                        <asp:DropDownList ID="ddlTipoNovedadTecnica" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>

                    <!-- Tipo de mantenimiento -->
                    <div class="col-md-4">
                        <label for="ddlTipoMantenimiento" class="form-label">Tipo de mantenimiento</label>
                        <asp:DropDownList ID="ddlTipoMantenimiento" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Todos" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Correctivo" Value="Correctivo"></asp:ListItem>
                            <asp:ListItem Text="Preventivo" Value="Preventivo"></asp:ListItem>
                            <asp:ListItem Text="Predictivo" Value="Predictivo"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Técnico responsable -->
                    <div class="col-md-4">
                        <label for="ddlUsuarioResponsable" class="form-label">Técnico responsable</label>
                        <asp:DropDownList ID="ddlUsuarioResponsable" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>

                    <!-- Tiempo perdido mínimo -->
                    <div class="col-md-6">
                        <label for="txtTiempoMinimo" class="form-label">Tiempo perdido mínimo (min)</label>
                        <asp:TextBox ID="txtTiempoMinimo" runat="server" CssClass="form-control" placeholder="Ej: 10"></asp:TextBox>
                    </div>

                    <!-- Tiempo perdido máximo -->
                    <div class="col-md-6">
                        <label for="txtTiempoMaximo" class="form-label">Tiempo perdido máximo (min)</label>
                        <asp:TextBox ID="txtTiempoMaximo" runat="server" CssClass="form-control" placeholder="Ej: 120"></asp:TextBox>
                    </div>

                    <!-- Botones -->
                    <div class="col-md-6 d-grid">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                            CssClass="btn btn-primary"
                            OnClick="btnBuscar_Click" />
                    </div>

                    <div class="col-md-6 d-grid">
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar"
                            CssClass="btn btn-outline-secondary"
                            CausesValidation="false"
                            OnClick="btnLimpiar_Click" />
                    </div>

                </div>

                <hr class="my-4" />

                <div class="table-responsive">
                    <asp:GridView
                        ID="gvReportesNovedadTecnica"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered table-striped text-center"
                        HeaderStyle-CssClass="text-center align-middle"
                        RowStyle-CssClass="align-middle text-center"
                        OnRowCommand="gvReportesNovedadTecnica_RowCommand">

                        <Columns>

                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}"
                                HeaderStyle-Wrap="false"
                                ItemStyle-Wrap="false" />

                            <asp:BoundField DataField="Turno" HeaderText="Turno" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="Maquina" HeaderText="Máquina" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="Modulo" HeaderText="Módulo" />
                            <asp:BoundField DataField="Elemento" HeaderText="Elemento" />
                            <asp:BoundField DataField="TipoNovedad" HeaderText="Tipo de novedad" />
                            <asp:BoundField DataField="TipoMantenimiento" HeaderText="Tipo mantenimiento" />
                            <asp:BoundField DataField="TecnicoResponsable" HeaderText="Técnico responsable" HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="TiempoPerdidoMinutos" HeaderText="Tiempo perdido (min)" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                            <asp:BoundField DataField="Diagnostico" HeaderText="Diagnóstico" />
                            <asp:BoundField DataField="SolucionAplicada" HeaderText="Solución aplicada" />

                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:Button
                                        ID="btnEditar"
                                        runat="server"
                                        Text="Editar"
                                        CssClass="btn btn-warning btn-sm me-1"
                                        CommandName="Editar"
                                        CommandArgument='<%# Eval("IdNovedadTecnica") %>' />

                                    <asp:Button
                                        ID="btnEliminar"
                                        runat="server"
                                        Text="Eliminar"
                                        CssClass="btn btn-danger btn-sm"
                                        CommandName="Eliminar"
                                        CommandArgument='<%# Eval("IdNovedadTecnica") %>'
                                        OnClientClick="return confirm('¿Seguro que desea eliminar esta novedad técnica?');" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>
                </div>

            </div>
        </div>
    </div>

</asp:Content>
