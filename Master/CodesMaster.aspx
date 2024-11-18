<%@ Page Title="Codes Master" Language="C#" MasterPageFile="~/SiteMasterPages/AdminPage.Master" AutoEventWireup="true" CodeBehind="CodesMaster.aspx.cs" Inherits="MotorClaimProcessingSystem.Master.CodesMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <script src="../Script.js"></script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= txtCode.ClientID %>').on('input', function () {
                this.value = this.value.toUpperCase();
            });
        });
        $(document).ready(function () {
            $('#<%= txtType.ClientID %>').on('input', function () {
                this.value = this.value.toUpperCase();
            });
        });
        $(document).ready(function () {
            $('#<%= txtParentCode.ClientID %>').on('input', function () {
                this.value = this.value.toUpperCase();
            });
        });
        $(document).ready(function () {

            $('#<%= txtCode.ClientID %>').on('input', function () {
                validateFields();
                checkCodeExists();
            });


            $('#<%= txtType.ClientID %>').on('input', function () {
                validateFields();
            });


            $('#<%= txtDescription.ClientID %>').on('input', function () {
                validateFields();
            });


            $('#<%= txtValue.ClientID %>').on('input', function () {
                validateFields();
            });


            $('#<%= txtParentCode.ClientID %>').on('input', function () {
                validateFields();
            });

            function validateFields() {
                var isValid = true;


                var code = $('#<%= txtCode.ClientID %>').val();
                if (code.length > 12) {
                    $('#lblCodeMessage').text('Code cannot exceed 12 characters').css('color', 'red');
                    isValid = false;
                } else {
                    $('#lblCodeMessage').text('');
                }


                var type = $('#<%= txtType.ClientID %>').val();
                if (type.length > 12) {
                    $('#lblTypeMessage').text('Type cannot exceed 12 characters').css('color', 'red');
                    isValid = false;
                } else {
                    $('#lblTypeMessage').text('');
                }


                var description = $('#<%= txtDescription.ClientID %>').val();
                var byteLength = new Blob([description]).size;
                if (byteLength > 240) {
                    $('#lblDescriptionMessage').text('Description cannot exceed 240 bytes').css('color', 'red');
                    isValid = false;
                } else {
                    $('#lblDescriptionMessage').text('');
                }


                var value = $('#<%= txtValue.ClientID %>').val();
                if (isNaN(value) || value < 0 || value > 999999999) {
                    $('#lblValueMessage').text('Value must be a valid number between 0 and 999999999').css('color', 'red');
                    isValid = false;
                } else {
                    $('#lblValueMessage').text('');
                }


                var parentCode = $('#<%= txtParentCode.ClientID %>').val();
                if (parentCode.length > 12) {
                    $('#lblParentCodeMessage').text('Parent Code cannot exceed 12 characters').css('color', 'red');
                    isValid = false;
                } else {
                    $('#lblParentCodeMessage').text('');
                }


                $('#<%= btnSave.ClientID %>').prop('disabled', !isValid);
                $('#<%= btnUpdate.ClientID %>').prop('disabled', !isValid);
            }

            function checkCodeExists() {
                var code = $('#<%= txtCode.ClientID %>').val();
                if (code.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "CodesMaster.aspx/CheckCodeExists",
                        data: JSON.stringify({ code: code }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d) {
                                $('#lblCodeExistMessage').text('Code already exists.').css('color', 'red');
                                $('#<%= btnSave.ClientID %>').prop('disabled', true);
                            } else {
                                $('#lblCodeExistMessage').text('Available').css('color', 'green');
                                $('#<%= btnSave.ClientID %>').prop('disabled', false);
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error(error);
                        }
                    });
                } else {
                    $('#lblCodeExistMessage').text('');
                }
            }
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server">
        <div class="">
            <div class="float-end">
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn-back" OnClick="btnBack_Click" />
            </div>
            <h2 class="text-center mt-5">Codes Master</h2>
            <div class="form-grid">

                <!-- Code -->
                <div>
                    <span>
                        <asp:Label ID="lblCode" runat="server" AssociatedControlID="txtCode" Text="Code:" CssClass="" /><span style="color: red;">*</span>
                    </span>
                    <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtCode"
                        ErrorMessage="Code is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Save" />
                    <label id="lblCodeMessage" class="small fw-light"></label>
                    <label id="lblCodeExistMessage" class="small fw-light"></label>
                </div>

                <!-- Type -->
                <div>

                    <span>
                        <asp:Label ID="lblType" runat="server" AssociatedControlID="txtType" Text="Type:" CssClass="" /><span style="color: red;">*</span>
                    </span>
                    <asp:TextBox ID="txtType" runat="server" CssClass="form-control" />

                    <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="txtType"
                        ErrorMessage="Type is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Save" />
                    <label id="lblTypeMessage" class="small fw-light"></label>
                </div>

                <!-- Description -->
                <div>
                    <asp:Label ID="lblDescription" runat="server" AssociatedControlID="txtDescription" Text="Description:" CssClass="" />
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    <label id="lblDescriptionMessage" class="small fw-light"></label>
                </div>

                <!-- Value -->
                <div>
                    <asp:Label ID="lblValue" runat="server" AssociatedControlID="txtValue" Text="Value:" CssClass="" />
                    <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" />
                    <label id="lblValueMessage" class="small fw-light"></label>
                </div>

                <!-- Parent Code -->
                <div>
                    <asp:Label ID="lblParentCode" runat="server" AssociatedControlID="txtParentCode" Text="Parent Code:" CssClass="" />
                    <asp:TextBox ID="txtParentCode" runat="server" CssClass="form-control" />
                    <label id="lblParentCodeMessage" class="small fw-light"></label>
                </div>

                <!-- Active (Y/N) -->
                <div>
                    <asp:Label ID="lblActive" runat="server" AssociatedControlID="ddlActive" Text="Active (Y/N):" CssClass="" />
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
