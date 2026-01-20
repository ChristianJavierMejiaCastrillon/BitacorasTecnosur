<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BitacorasWeb.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <div class="form-container mt-4">
        <h2 class="mb-4">Login</h2>

        <div class="mb-3">
            <label for="txtUsuarioLogin" class="form-label">Usuario</label>
            <asp:TextBox ID="txtUsuarioLogin" runat="server" CssClass="form-control" />
        </div>

        <div class="mb-3">
            <label for="txtPass" class="form-label">Contraseña</label>
            <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" />
        </div>

        <asp:Button ID="btnIngresar" runat="server" CssClass="btn btn-primary" 
            Text="Ingresar" OnClick="btnIngresar_Click" />
    </div>
</asp:Content>
