<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="shop.aspx.cs" Inherits="WebAppSecurity.shop" ValidateRequest="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    
    
    
    <div class="w3-row w3-light-gray" style="margin:auto; width:50%;">
        <asp:Label runat="server" ID="labelLogged">You are logged in! Browse the shop!</asp:Label>
        <a class="btn btn-danger" id="out" runat="server" onserverclick="logOut" visible="false">Log Out</a>
        <asp:Label runat="server" ID="lb_finalmsg"></asp:Label>
            
    </div>
</asp:Content>
