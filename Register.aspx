﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebAppSecurity.Register" ValidateRequest="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function finalCheck() {
            var finalmsg = "";
            // fname validation
            var fname = document.getElementById('<%=tb_fname.ClientID%>').value;
            if (fname.search(/[^a-zA-Z0-9]+/) == -1) {
                document.getElementById('<%=lb_fname.ClientID%>').innerHTML = " ";
            }
            else if (fname == "") {
                finalmsg += "fnameErrorBlank";
            }
            else {
                document.getElementById('<%=lb_fname.ClientID%>').innerHTML = "Invalid first name";
                document.getElementById('<%=lb_fname.ClientID%>').style.color = "Red";
                finalmsg += "fnameError";
            }
            // lname validation
            var lname = document.getElementById('<%=tb_lname.ClientID%>').value;
            if (lname.search(/[^a-zA-Z0-9]+/) == -1) {
                document.getElementById('<%=lb_lname.ClientID%>').innerHTML = " ";
            }
            else if (lname == "") {
                finalmsg += "lnameErrorBlank";
            }
            else {
                document.getElementById('<%=lb_lname.ClientID%>').innerHTML = "Invalid last name";
                document.getElementById('<%=lb_lname.ClientID%>').style.color = "Red";
                finalmsg += "lnameError";
            }
            // cc validation
            var cc = document.getElementById('<%=tb_cc.ClientID%>').value;
            if (cc.search(/^[0-9]+$/) == 0) {
                document.getElementById('<%=lb_cc.ClientID%>').innerHTML = " ";
            }
            else if (cc == "") {
                finalmsg += "ccErrorBlank";
            }
            else {
                document.getElementById('<%=lb_cc.ClientID%>').innerHTML = "Invalid Credit Card. Numbers only";
                document.getElementById('<%=lb_cc.ClientID%>').style.color = "Red";
                finalmsg += "ccError";
            }
            // email validation
            var email = document.getElementById('<%=tb_email.ClientID%>').value;
            if (email.search(/^[a-zA-Z0-9]+@[a-zA-Z]+\.[a-zA-Z]+$/) == 0) {
                document.getElementById('<%=lb_email.ClientID%>').innerHTML = " ";
            }
            else if (email == "") {
                finalmsg += "emailErrorBlank";
            }
            else {
                document.getElementById('<%=lb_email.ClientID%>').innerHTML = "Invalid Email Address!";
                document.getElementById('<%=lb_email.ClientID%>').style.color = "Red";
                finalmsg += "emailError";
            }
            // password validation
            var pass = document.getElementById('<%=tb_pwd.ClientID%>').value;
            var msg = "Must have ";
            if (pass.length < 8) {
                msg += "8 Characters ";
            }
            if (pass.search(/[a-z]+/) == -1) {
                msg += "1 lowercase "
            }
            if (pass.search(/[A-Z]+/) == -1) {
                msg += "1 uppercase "
            }
            if (pass.search(/[0-9]+/) == -1) {
                msg += "1 number "
            }
            if (pass.search(/[^a-zA-Z0-9]+/) == -1) {
                msg += "1 symbol "
            }
            if (msg == "Must have ") {
                document.getElementById('<%=lb_pwd.ClientID%>').innerHTML = "";
            }
            else if (pass == "") {
                finalmsg += "passErrorBlank";
            }
            else {
                document.getElementById('<%=lb_pwd.ClientID%>').innerHTML = msg;
                document.getElementById('<%=lb_pwd.ClientID%>').style.color = "Red";
                finalmsg += "passError";
            }
            // confirm password validation
            var pass = document.getElementById('<%=tb_pwd.ClientID%>').value;
            var confirm = document.getElementById('<%=tb_confirmpassword.ClientID%>').value;
            if (confirm == "") {
                finalmsg += "conpassErrorBlank";
            }
            else if (confirm == pass) {
                document.getElementById('<%=lb_confirmpassword.ClientID%>').innerHTML = " ";
            }
            else {
                document.getElementById('<%=lb_confirmpassword.ClientID%>').innerHTML = "Password do not match!";
                document.getElementById('<%=lb_confirmpassword.ClientID%>').style.color = "Red";
                finalmsg += "conpassError";
            }
            // validation success
            if (finalmsg == "") {
                document.getElementById('<%=btnAddUser.ClientID%>').classList.remove("disabled");
            }
            else {
                document.getElementById('<%=btnAddUser.ClientID%>').classList.remove("disabled");
                document.getElementById('<%=btnAddUser.ClientID%>').classList.add("disabled");
            }
            console.log(finalmsg);
        }
        // visible password
        function showPass(para) {
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
            // eye confirm password clicked
            if (para == 'confirm') {
                if (document.getElementById('<%=tb_confirmpassword.ClientID%>').getAttribute('type') == 'password') {
                    document.getElementById('<%=tb_confirmpassword.ClientID%>').setAttribute('type', 'text');
                    document.getElementById('eye_confirmpassword').classList.remove("fa-eye");
                    document.getElementById('eye_confirmpassword').classList.add("fa-eye-slash");
                }
                else {
                    document.getElementById('<%=tb_confirmpassword.ClientID%>').setAttribute('type', 'password');
                    document.getElementById('eye_confirmpassword').classList.remove("fa-eye-slash");
                    document.getElementById('eye_confirmpassword').classList.add("fa-eye");
                }
            }
        }

    </script>
    <div class="container w3-responsive">
        <div class="w3-row">
            <header style="text-align:center;">
                <h1><b>Register Account</b></h1>
                <asp:Label runat="server" Text=" " ForeColor="Red" ID="lb_finalmsg"></asp:Label>
            </header>
        </div>
       
        <div class="w3-row w3-light-gray" style="margin:auto; width:50%;">
            <table>
                <tr >
                    <td>   
                        <asp:Label runat="server" Text="First Name" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox 
                            ID="tb_fname" 
                            runat="server" 
                            OnInput="javascript:finalCheck()"></asp:TextBox>
                        <p><asp:Label 
                            ID="lb_fname" 
                            runat="server" 
                            Font-Size="x-small" 
                            ForeColor="White"
                            Text=""></asp:Label>&nbsp</p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Last Name"></asp:Label>
                    </td>
                    <td >
                        <asp:TextBox 
                            ID="tb_lname" 
                            runat="server" 
                            OnInput="javascript:finalCheck()"></asp:TextBox>
                        <p><asp:Label 
                            ID="lb_lname" 
                            runat="server" 
                            Font-Size="x-small" 
                            ForeColor="White"
                            Text=""></asp:Label>&nbsp</p>
                    </td>
                </tr>
                <tr>
                   <td>
                       <asp:Label runat="server" Text="Email"></asp:Label>
                   </td>
                    <td>
                        <asp:TextBox 
                            ID="tb_email" 
                            runat="server" 
                            OnInput="javascript:finalCheck()"></asp:TextBox>
                        <p><asp:Label 
                            ID="lb_email" 
                            runat="server" 
                            Font-Size="x-small" 
                            ForeColor="White"
                            Text=""></asp:Label>&nbsp</p>
                    </td>
                </tr>
                <tr>
                   <td>
                       <asp:Label runat="server" Text="Date of Birth"></asp:Label>
                   </td>
                    <td>
                        <asp:TextBox 
                            ID="tb_dob" 
                            runat="server" 
                            Enabled="false" Width="128px"></asp:TextBox>
                        <a id="calenderBtn" onserverclick="calendarCall" runat="server" class="btn w3-hover-blue">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </a>
                        <asp:Calendar
                            OnDayRender="calendar_render"
                            OnSelectionChanged="calendar_select"
                            ID="calendar" runat="server" Height="250px" Width="330px" BackColor="White" BorderColor="Black" BorderStyle="Solid" CellSpacing="1" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" NextPrevFormat="ShortMonth">
                            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" Height="8pt" />
                            <DayStyle BackColor="#CCCCCC" />
                            <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
                            <OtherMonthDayStyle ForeColor="#999999"/>
                            <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                            <TitleStyle BackColor="#333399" BorderStyle="Solid" Font-Bold="True" Font-Size="12pt" ForeColor="White" Height="12pt" />
                            <TodayDayStyle BackColor="#999999" ForeColor="White" />
                        </asp:Calendar>
                        <p><asp:Label 
                            ID="lb_dob" 
                            runat="server" 
                            Font-Size="x-small" 
                            ForeColor="White"
                            Text=""></asp:Label>&nbsp</p>
                    </td>
                </tr>
                <tr>
                   <td>
                       <asp:Label runat="server" Text="Credit Card(No space)"></asp:Label>
                   </td>
                    <td>
                        <asp:TextBox 
                            ID="tb_cc" 
                            runat="server" 
                            OnInput="javascript:finalCheck()"></asp:TextBox>
                        <p><asp:Label 
                            ID="lb_cc" 
                            runat="server" 
                            Font-Size="x-small" 
                            ForeColor="White"
                            Text=""></asp:Label>&nbsp</p>
                    </td>
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
                            OnInput="javascript:finalCheck()"></asp:TextBox>
                        <span 
                            runat="server"
                            id="eye_password" 
                            class=" glyphicon glyphicon-eye-open w3-right" 
                            onclick="javascript:showPass('password')" 
                            style="font-size:1vw;"></span>
                        <p><asp:Label 
                            ID="lb_pwd" 
                            runat="server" 
                            Font-Size="x-small" 
                            ForeColor="Blue" 
                            Text="8 characters with 1 Uppercase, Lowercase, Symbol, Number"></asp:Label>&nbsp</p>
                    </td>
                </tr>
                <tr >
                    <td>
                        <asp:Label runat="server" Text="Confirm Password"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox 
                            ID="tb_confirmpassword"
                            runat="server" 
                            OnInput="javascript:finalCheck()"
                            TextMode="Password"></asp:TextBox>
                        <span 
                            runat="server"
                            id="eye_confirmpassword" 
                            class="glyphicon glyphicon-eye-open w3-right" 
                            onclick="javascript:showPass('confirm')" 
                            style="font-size:1vw;"></span>
                        <p><asp:Label 
                            ID="lb_confirmpassword" 
                            runat="server" 
                            Font-Size="X-Small"
                            ForeColor="Blue" 
                            Text="Must match password"></asp:Label>&nbsp</p>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div class="w3-row w3-bar" style="width:50%;margin:auto;">
            <a runat="server" href="~/" class="btn w3-red w3-hover-blue w3-left w3-bar-item">
                
                <span class="nodeco w3-right w3-margin-left">CANCEL</span>
            </a>
            
            <a id="btnAddUser" onserverclick="addUserClick" runat="server" class="btn disabled w3-green w3-hover-blue w3-right w3-bar-item">
                
                <span class="nodeco w3-right w3-margin-left">REGISTER</span>
            </a>
        </div>
    </div>
</asp:Content>
