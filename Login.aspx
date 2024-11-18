<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MotorClaimProcessingSystem.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <script src="../Script.js"></script>
    <link href="Src/Login.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <div class="wrapper">
            <div class="head-wrap">
                <h1>Motor Claim Processing System</h1>
            </div>
            <hr />
            <h2>Login</h2>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

            <div class="input-field">
                <asp:TextBox ID="txtUserName" runat="server" CssClass="" placeholder=" " />
                <label>Enter Your User Id</label>
                <asp:RequiredFieldValidator ID="rfvUserID" runat="server" ControlToValidate="txtUserName"
                    ErrorMessage="User ID is required." ForeColor="Red" ValidationGroup="Login" />
            </div>

            <div class="input-field">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="" placeholder=" " />
                <label>Enter your password</label>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                    ErrorMessage="Password is required." ForeColor="Red" ValidationGroup="Login" />
            </div>
            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Log In" CssClass="login-btn" ValidationGroup="Login" />
        </div>
    </form>

</body>
</html>
