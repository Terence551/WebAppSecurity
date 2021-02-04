<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebAppSecurity.Login" ValidateRequest="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        // visible password
        function show(para) {
            if (para == 'password') {
                if (document.getElementById('<%=tb_pwd.ClientID%>').getAttribute('type') == 'password') {
                    document.getElementById('<%=tb_pwd.ClientID%>').setAttribute('type', 'text');
                    document.getElementById('eye_password').classList.remove("glyphicon-eye-open");
                    document.getElementById('eye_password').classList.add("glyphicon-eye-close");
                }
                else {
                    document.getElementById('<%=tb_pwd.ClientID%>').setAttribute('type', 'password');
                    document.getElementById('eye_password').classList.remove("glyphicon-eye-close");
                    document.getElementById('eye_password').classList.add("glyphicon-eye-open");
                }
            }
        }
    </script>
    <div class="container w3-responsive">
        <div class="w3-row">
            <header style="text-align:center;">
                <h1><b>Login</b></h1>
                <asp:Label runat="server" Text=" " ForeColor="Red" ID="lb_finalmsg"></asp:Label>
            </header>
        </div>
       
        <div class="w3-row w3-light-gray" style="margin:auto; width:50%;">
            <table>
                <tr>
                   <td>
                       <asp:Label runat="server" Text="Email"></asp:Label>
                   </td>
                    <td>
                        <asp:TextBox 
                            ID="tb_email" 
                            runat="server" 
                            ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                   <td>
                       <asp:Label runat="server" Text="Password"></asp:Label>
                   </td>
                    <td>
                        <asp:TextBox 
                            ID="tb_pwd" 
                             TextMode="Password"
                            runat="server" 
                            ></asp:TextBox>
                        <span 
                            id="eye_password" 
                            class=" glyphicon glyphicon-eye-open w3-right" 
                            onclick="javascript:show('password')" 
                            style="font-size:1vw;"></span>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div class="w3-row w3-bar" style="width:50%;margin:auto;">
            <a runat="server" href="~/" class="btn w3-red w3-hover-blue w3-left w3-bar-item">
                
                <span class="nodeco w3-right w3-margin-left">CANCEL</span>
            </a>
            
            <a id="btnLogin" onserverclick="LoginClick" runat="server" class="btn w3-green w3-hover-blue w3-right w3-bar-item">
                
                <span class="nodeco w3-right w3-margin-left">LOGIN</span>
            </a>
        </div>
        
        <asp:Label runat="server" ID="lb_gscore" ForeColor="Blue" Text="" ></asp:Label>
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
    </div>
</asp:Content>
