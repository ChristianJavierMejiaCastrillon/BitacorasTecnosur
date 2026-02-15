<%@ Page Title="Editar Novedad" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditarNovedad.aspx.cs" Inherits="BitacorasWeb.Admin.EditarNovedad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <div class="card shadow-sm p-4">

            <h2 class="mb-4">Editar Novedad (Administrador)</h2>

            <asp:Label ID="lblMensaje" runat="server"></asp:Label>

            <asp:HiddenField ID="hfIdNovedad" runat="server" />

            <div class="row g-3">

                <div class="col-md-6">
                    <label class="form-label">Tipo</label>
                    <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <div class="col-md-6">
                    <label class="form-label">Producto</label>
                    <asp:DropDownList ID="ddlProducto" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <div class="col-md-6">
                    <label class="form-label">Tiempo perdido (min)</label>
                    <asp:TextBox ID="txtTiempo" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-12">
                    <label class="form-label">Descripción</label>
                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control"
                        TextMode="MultiLine" Rows="4"></asp:TextBox>
                </div>

                <div class="col-12">
                    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar"
                        CssClass="btn btn-primary"
                        OnClick="btnActualizar_Click" />
                </div>

            </div>

        </div>
    </div>

</asp:Content>
