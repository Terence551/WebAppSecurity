<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebAppSecurity._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Stationary Shop</h1>
        <p class="lead">Our shop aims to provide tools and equipment for both Staff and Students of NYP.</p>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="jumbotron">
                <h2>Getting started</h2>
                <p>
                    New to the website? Then Register with us to begin!
                </p>
                <p>
                    <a class="btn btn-primary" href="Register">Register &raquo;</a>
                </p>
            </div>
        </div>
        <div class="col-md-6">
            <div class="jumbotron">
                <h2>Login</h2>
                <p>
                    Have an existing account already? Login to browse!
                </p>
                <p>
                    <a class="btn btn-success" href="Login">Login &raquo;</a>
                </p>
            </div>        </div>
    </div>

</asp:Content>
