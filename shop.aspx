<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="shop.aspx.cs" Inherits="WebAppSecurity.shop" ValidateRequest="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" ID="labelLogged">You are logged in! Browse the shop!</asp:Label>
    <a class="btn btn-danger" id="out" runat="server" onserverclick="logOut" visible="false">Log Out</a>
</asp:Content>
