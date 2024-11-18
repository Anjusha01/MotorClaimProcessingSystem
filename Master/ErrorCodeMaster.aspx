<%@ Page Title="Error Code Master" Language="C#" MasterPageFile="~/SiteMasterPages/AdminPage.Master" AutoEventWireup="true" CodeBehind="ErrorCodeMaster.aspx.cs" Inherits="MotorClaimProcessingSystem.Master.ErrorCodeMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <script src="../Script.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
           
            $('#<%= txtErrorCode.ClientID %>').on('input', function () {
                validateFields();
                checkErrorCodeExists();
            });

         
            $('#<%= txtErrorType.ClientID %>').on('input', function () {
                validateFields();
            });

          
            $('#<%= txtErrorDescription.ClientID %>').on('input', function () {
                validateFields();
            });

            function validateFields() {
                var isValid = true;

               
                var errorCode = $('#<%= txtErrorCode.ClientID %>').val();
                if (errorCode.length > 12) {
                    $('#lblErrorCodeMessage').text('Error Code cannot exceed 12 characters').css('color', 'red');
                    isValid = false;
                } else {
                    $('#lblErrorCodeMessage').text('');
                }

               
                var errorType = $('#<%= txtErrorType.ClientID %>').val();
                if (errorType.length > 12) {
                    $('#lblErrorTypeMessage').text('Error Type cannot exceed 12 characters').css('color', 'red');
                    isValid = false;
                } else {
                    $('#lblErrorTypeMessage').text('');
                }

                var errorDescription = $('#<%= txtErrorDescription.ClientID %>').val();
                var byteLength = new Blob([errorDescription]).size;
                if (byteLength > 100) {
                    $('#lblErrorDescriptionMessage').text('Error Description cannot exceed 240 bytes').css('color', 'red');
                    isValid = false;
                } else {
                    $('#lblErrorDescriptionMessage').text('');
                }

                $('#<%= btnSave.ClientID %>').prop('disabled', !isValid);
                $('#<%= btnUpdate.ClientID %>').prop('disabled', !isValid);
            }

            function checkErrorCodeExists() {
                var errorCode = $('#<%= txtErrorCode.ClientID %>').val();
                if (errorCode.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ErrorCodeMaster.aspx/CheckErrorCodeExists",
                        data: JSON.stringify({ code: errorCode }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d) { 
                                $('#lblErrorCodeExistsMessage').text('Error Code already exists.').css('color', 'red');
                                $('#<%= btnSave.ClientID %>').prop('disabled', true);
                            } else {
                                $('#lblErrorCodeExistsMessage').text('Avialable').css('color', 'green');
                                $('#<%= btnSave.ClientID %>').prop('disabled', false);
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error(error);
                        }
                    });
                } else {
                    $('#lblErrorCodeExistsMessage').text('');
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
            <h2 class="text-center mt-5">Error Code Master</h2>
            <div class="form-grid">
                <!-- Error Code -->
                <div>
                    <span>
                        <asp:Label ID="lblErrorCode" runat="server" AssociatedControlID="txtErrorCode" Text="Error Code:" CssClass="" /><span style="color: red;">*</span>
                    </span>
                    <asp:TextBox ID="txtErrorCode" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvErrorCode" runat="server" ControlToValidate="txtErrorCode"
                        ErrorMessage="Error Code is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Save" />
                    <label id="lblErrorCodeMessage" class="small fw-light"></label>
                    <label id="lblErrorCodeExistsMessage" class="small fw-light"></label>
                </div>

                <!-- Error Type -->
                <div>
                    <span>
                        <asp:Label ID="lblErrorType" runat="server" AssociatedControlID="txtErrorType" Text="Error Type:" CssClass="" /><span style="color: red;">*</span>
                    </span>
                    <asp:TextBox ID="txtErrorType" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvErrorType" runat="server" ControlToValidate="txtErrorType"
                        ErrorMessage="Error Type is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Save" />
                    <label id="lblErrorTypeMessage" class="small fw-light"></label>
                </div>

                <!-- Error Description -->
                <div>
                    <asp:Label ID="lblErrorDescription" runat="server" AssociatedControlID="txtErrorDescription" Text="Error Description:" CssClass="" />
                    <asp:TextBox ID="txtErrorDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    <label id="lblErrorDescriptionMessage" class="small fw-light"></label>
                </div>
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
