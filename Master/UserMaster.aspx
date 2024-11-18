<%@ Page Title="User Master" Language="C#" MasterPageFile="~/SiteMasterPages/AdminPage.Master" AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" Inherits="MotorClaimProcessingSystem.Master.UserMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <script src="../Script.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // Validate UserId length
            $('#<%= txtUserId.ClientID %>').on('input', function () {
                validateFields();
                checkUserIdExists();
            });

            // Validate UserName length
            $('#<%= txtUserName.ClientID %>').on('input', function () {
                validateFields();
            });

            // Validate Password length
            $('#<%= txtPassword.ClientID %>').on('input', function () {
                validateFields();
            });

            function validateFields() {
                var isValid = true;

                // UserId validation (max 12 characters)
                var userId = $('#<%= txtUserId.ClientID %>').val();
                if (userId.length > 12) {
                    $('#lblUserIdMessage').text('User ID cannot exceed 12 characters').css('color', 'red');
                    isValid = false;
                } else {
                    $('#lblUserIdMessage').text('');
                }

                // UserName validation (max 30 characters)
                var userName = $('#<%= txtUserName.ClientID %>').val();
                if (userName.length > 30) {
                    $('#lblUserNameMessage').text('User Name cannot exceed 30 characters').css('color', 'red');
                    isValid = false;
                } else {
                    $('#lblUserNameMessage').text('');
                }

                // Password validation (max 24 characters)
                var password = $('#<%= txtPassword.ClientID %>').val();
                if (password.length > 24) {
                    $('#lblPasswordMessage').text('Password cannot exceed 24 characters').css('color', 'red');
                    isValid = false;
                } else {
                    $('#lblPasswordMessage').text('');
                }

                // Disable or enable save button based on validation
                $('#<%= btnSave.ClientID %>').prop('disabled', !isValid);
            }

            function checkUserIdExists() {
                var userId = $('#<%= txtUserId.ClientID %>').val();
                console.log(userId);
                if (userId.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "UserMaster.aspx/CheckUserIdExists",
                        data: JSON.stringify({ userId: userId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d) { // if the UserId exists
                                $('#lblUserIdExistMessage').text('User ID already exists.').css('color', 'red');
                                $('#<%= btnSave.ClientID %>').prop('disabled', true);
                            } else {
                                $('#lblUserIdExistMessage').text('Available').css('color', 'green');
                                $('#<%= btnSave.ClientID %>').prop('disabled', false);
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error(error);
                        }
                    });
                } else {
                    $('#lblUserIdExistMessage').text('io');
                }
            }
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server">
        <div class="container-fluid">
            <div class="float-end">
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn-back" OnClick="btnBack_Click" />
            </div>
            <h2 class="text-center mt-5">User Master</h2>
            <div class="form-grid">

                <!-- User ID -->
                <div>
                    <span>
                        <asp:Label ID="lblUserId" runat="server" AssociatedControlID="txtUserId" Text="User ID:" CssClass="" /><span style="color: red;">*</span>
                    </span>
                    <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvUserId" runat="server" ControlToValidate="txtUserId" ErrorMessage="User ID is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Save" />
                    <label id="lblUserIdMessage" class="small fw-light"></label>
                    <label id="lblUserIdExistMessage" class="small fw-light"></label>
                </div>

                <!-- User Name -->
                <div>
                    <span>
                        <asp:Label ID="lblUserName" runat="server" AssociatedControlID="txtUserName" Text="User Name:" CssClass="" /><span style="color: red;">*</span>
                    </span>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName" ErrorMessage="User Name is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Save" />
                    <label id="lblUserNameMessage" class="small fw-light"></label>
                </div>

                <!-- Password -->
                <div>
                    <!-- Password Field -->
                    <span>
                        <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="Password:" CssClass="" /><span style="color: red;">*</span>
                    </span>

                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="Password is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Save" />
                    <!-- Password Pattern Validation -->
                    <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="Password must be at least 6 characters long and contain uppercase, lowercase, number, and special character."
                        CssClass="text-danger" Display="Dynamic" ValidationGroup="Save"
                        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$" />
                    <label id="lblPasswordMessage" class="small fw-light"></label>
                </div>

                <div>
                    <!-- Confirm Password Field -->
                    <span>
                        <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" Text="Confirm Password:" CssClass="" /><span style="color: red;">*</span>
                    </span>

                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                        ErrorMessage="Confirm Password is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Save" />
                    <!-- Password Match Validator -->
                    <asp:CompareValidator ID="cvPasswords" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword"
                        ErrorMessage="Passwords do not match." CssClass="text-danger" Display="Dynamic" ValidationGroup="Save" />
                </div>


                <!-- Active (Y/N) -->
                <div>
                    <span>
                        <asp:Label ID="lblActive" runat="server" AssociatedControlID="ddlActive" Text="Active (Y/N):" CssClass="" /><span style="color: red;">*</span>
                    </span>
                    <asp:DropDownList ID="ddlActive" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <!-- General Error Message Label -->
            <div>
                <asp:Label ID="lblGeneralError" runat="server" CssClass="text-danger"></asp:Label>
            </div>

            <!-- Save and Update Buttons -->
            <div class="d-flex justify-content-center">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn-save-color button-action" OnClick="btnSave_Click" ValidationGroup="Save" />
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn-update-color button-action" OnClick="btnUpdate_Click" ValidationGroup="Save" Visible="false" />
            </div>

            <!-- Message Label -->
            <div>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
