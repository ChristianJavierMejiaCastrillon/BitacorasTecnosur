<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NoAutorizado.aspx.cs" Inherits="BitacorasWeb.NoAutorizado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>No autorizado</h2>
        <p>No tienes permisos para acceder a esta página.</p>
        <a href="Home.aspx" class="btn btn-primary">Volver al inicio</a>
    </div>
</asp:Content>
