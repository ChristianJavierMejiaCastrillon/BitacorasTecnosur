<%@ Page Title="Producto" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductoFormulario.aspx.cs" Inherits="BitacorasWeb.Admin.ProductoFormulario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <h2 id="titulo" runat="server">Producto</h2>

        <asp:HiddenField ID="hfIdProducto" runat="server" />

        <div class="row">
            <div class="col-md-4 mb-3">
                <label class="form-label">Código</label>
                <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvCodigo" runat="server"
                    ControlToValidate="txtCodigo" ErrorMessage="Código requerido"
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="col-md-8 mb-3">
                <label class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                    ControlToValidate="txtNombre" ErrorMessage="Nombre requerido"
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="col-md-12 mb-3">
                <label class="form-label">Descripción</label>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
            </div>
        </div>

        <hr />

        <h5>Máquinas donde se elabora</h5>
        <asp:CheckBoxList ID="cblMaquinas" runat="server" CssClass="form-check">
        </asp:CheckBoxList>

        <div class="mt-4">
            <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnVolver" runat="server" CssClass="btn btn-secondary" Text="Volver" CausesValidation="false" OnClick="btnVolver_Click" />
        </div>

        <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger mt-3 d-block" />
    </div>

</asp:Content>
