<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPages/TransactionPage.Master" AutoEventWireup="true" CodeBehind="AddNewPolicy.aspx.cs" Inherits="MotorClaimProcessingSystem.AddNewPolicy" EnableViewState="true" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <script src="../Script.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            var maxChars = 100;

            $("#<%= txtToDate.ClientID %>").datepicker("destroy");
            $("#<%= txtToDate.ClientID %>").attr("readonly", "readonly");

            initializeDatePicker(".date-picker");

            handleCharacterLimit('#<%= txtAssuredAddress.ClientID %>', '#lblAssuredAddressMessage', 100);
            handleCharacterLimit('#<%= txtAssuredName.ClientID %>', '#lblAssuredNameMessage', 50);
            handleCharacterLimit('#<%= txtRegistrationNumber.ClientID %>', '#lblRegistrationNumberMessage', 10);
            handleDigitLimit('#<%= txtChassisNumber.ClientID %>', '#lblChassisNumberMessage', 17);
        });
        function handleCharacterLimit(inputSelector, messageSelector, maxChars) {
            $(inputSelector).on('input', function () {
                var currentLength = $(this).val().length;
                if (currentLength > maxChars) {
                    $(messageSelector).text("Maximum character limit of " + maxChars + " exceeded.").show();
                    //$(this).val($(this).val().substring(0, maxChars + 1));
                    $('#btnSave').prop('disabled', true);
                    $('#btnUpdate').prop('disabled', true);
                } else {
                    $(messageSelector).hide();
                }
            });
        }
        function handleDigitLimit(inputSelector, messageSelector, maxDigits) {
            $(inputSelector).on('input', function () {
                var currentValue = $(this).val();

                var isNumber = /^\d*$/.test(currentValue);

                if (!isNumber) {
                    $(messageSelector).text("Only numeric characters are allowed.").show();
                    $(this).val(currentValue.substring(0, maxDigits));
                    $('#btnSave').prop('disabled', true);
                    $('#btnUpdate').prop('disabled', true);
                    return;
                }
                var currentLength = currentValue.length;

                if (currentLength === maxDigits) {
                    $(messageSelector).hide();
                    $('#btnSave').prop('disabled', false);
                    $('#btnUpdate').prop('disabled', false);
                } else if (currentLength > maxDigits) {
                    $(this).val(currentValue.substring(0, maxDigits));
                    $(messageSelector).hide();
                    $('#btnSave').prop('disabled', false);
                    $('#btnUpdate').prop('disabled', false);
                } else {
                    $(messageSelector).text("This field must be exactly " + maxDigits + " digits.").show();
                    $('#btnSave').prop('disabled', true);
                    $('#btnUpdate').prop('disabled', true);
                }
            });
        }
        function initializeDatePicker(selector) {
            $(selector).datepicker({
                dateFormat: "dd-mm-yy",
                changeMonth: true,
                changeYear: true,
                yearRange: "c-30:c+30"
            });
        }

        function updatePremium() {
            var netFcPremium = parseFloat($('#<%= txtNetFcPremium.ClientID %>').val()) || 0;
            $.ajax({
                type: "POST",
                url: "../CurrencyExchange.asmx/CalculatePremiumAndVAT",
                data: JSON.stringify({
                    netFcPremium: netFcPremium
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#<%= txtNetLcPremium.ClientID %>').val(response.d.NetLcPremium.toFixed(2));
                    $('#<%= txtVatFcAmount.ClientID %>').val(response.d.VatFcAmount.toFixed(2));
                    $('#<%= txtVatLcAmount.ClientID %>').val(response.d.VatLcAmount.toFixed(2));

                },
                error: function (error) {
                    console.error("Error: ", error);
                }
            });
        }

        function calculateEndDate() {
            var startDateString = $('#<%= txtFromDate.ClientID %>').val();

            var dateParts = startDateString.split('-');

            if (dateParts.length === 3) {
                var day = parseInt(dateParts[0], 10);
                var month = parseInt(dateParts[1], 10) - 1;
                var year = parseInt(dateParts[2], 10);

                var startDate = new Date(year, month, day);

                if (!isNaN(startDate.getTime())) {

                    startDate.setDate(startDate.getDate() + 365);

                    var endDay = ("0" + startDate.getDate()).slice(-2);
                    var endMonth = ("0" + (startDate.getMonth() + 1)).slice(-2);
                    var endYear = startDate.getFullYear();
                    var formattedEndDate = endDay + '-' + endMonth + '-' + endYear;
                    $('#<%= txtToDate.ClientID %>').val(formattedEndDate);
                }
            }
        }


        function validateDateOfBirth(source, args) {
            var dob = new Date($('#<%= txtAssuredAge.ClientID %>').val());
            var today = new Date();

            if (isNaN(dob.getTime()) || dob > today || dob.getFullYear() < 1900) {
                args.IsValid = false;
                return;
            }
            var age = today.getFullYear() - dob.getFullYear();
            var monthDiff = today.getMonth() - dob.getMonth();

            if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < dob.getDate())) {
                age--;
            }
            if (age < 18 || age > 65) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }

        function validateNetFcPremium() {
            var netFcPremium = document.getElementById('<%= txtNetFcPremium.ClientID %>').value;
            var messageSpan = document.getElementById('lblNetFcPremiumMessage');


            if (isNaN(netFcPremium) || netFcPremium < 1 || netFcPremium > 9999999) {
                messageSpan.textContent = 'Net FC Premium must be a number between 1 and 9,999,999.';
            } else {
                messageSpan.textContent = '';
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" CssClass="pt-4">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <div class="container-fluid">
            <div class="d-flex justify-content-between">
                <h3>Add Policy Details</h3>
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="btn-back float-end" />
            </div>


            <div class="form-grid">
                <div>
                    <asp:Label ID="lblPolicyNumber" runat="server" Text="Policy Number:" AssociatedControlID="txtPolicyNumber" />
                    <asp:TextBox ID="txtPolicyNumber" runat="server" CssClass="form-control" />
                </div>
                <!-- Policy Issue Date (Readonly) -->
                <div>
                    <asp:Label ID="lblIssueDate" runat="server" Text="Policy Issue Date:" AssociatedControlID="txtIssueDate" />
                    <asp:TextBox ID="txtIssueDate" runat="server" CssClass="form-control" />
                </div>
                <div>
                    <span>
                        <asp:Label ID="lblAssuredName" runat="server" Text="Assured Name:" AssociatedControlID="txtAssuredName" /><span style="color: red;">*</span>
                    </span>
                    <asp:TextBox ID="txtAssuredName" runat="server" CssClass="form-control" />

                    <asp:RequiredFieldValidator ID="rfvAssuredName" runat="server" ControlToValidate="txtAssuredName"
                        ErrorMessage="Assured Name is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                    <span id="lblAssuredNameMessage" class="text-danger"></span>
                </div>
                <div>
                    <span>
                        <asp:Label ID="lblAssuredAddress" runat="server" Text="Assured Address:" AssociatedControlID="txtAssuredAddress" /><span style="color: red;">*</span></span>
                    <asp:TextBox ID="txtAssuredAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />

                    <asp:RequiredFieldValidator ID="rfvAssuredAddress" runat="server" ControlToValidate="txtAssuredAddress"
                        ErrorMessage="Assured Address is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                    <span id="lblAssuredAddressMessage" class="text-danger"></span>
                </div>



                <!-- Assured Date of Birth -->
                <div>
                    <span>
                        <asp:Label ID="lblAssuredAge" runat="server" Text="Assured Date of Birth:" AssociatedControlID="txtAssuredAge" /><span style="color: red;">*</span></span>
                    <asp:TextBox ID="txtAssuredAge" runat="server" CssClass="form-control date-picker" />
                    <asp:CustomValidator ID="cvDateOfBirth" runat="server" ControlToValidate="txtAssuredAge"
                        ErrorMessage="Age must be between 18 and 65 years." ClientValidationFunction="validateDateOfBirth"
                        CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                    <asp:RequiredFieldValidator ID="rfvAssuredAge" runat="server" ControlToValidate="txtAssuredAge"
                        ErrorMessage="Date of Birth is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                </div>

                <!-- Assured Mobile -->
                <div>
                    <span>
                        <asp:Label ID="lblAssuredMobile" runat="server" Text="Assured Mobile:" AssociatedControlID="txtAssuredMobile" /><span style="color: red;">*</span></span>
                    <asp:TextBox ID="txtAssuredMobile" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvAssuredMobile" runat="server" ControlToValidate="txtAssuredMobile"
                        ErrorMessage="Assured Mobile is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                    <asp:RegularExpressionValidator ID="revMobile" runat="server"
                        ControlToValidate="txtAssuredMobile"
                        ErrorMessage="Invalid mobile number format."
                        ForeColor="Red"
                        ValidationExpression="^\d{10}$" ValidationGroup="save" />
                </div>

                <!-- Assured Email -->
                <div>
                    <span>
                        <asp:Label ID="lblAssuredEmail" runat="server" Text="Assured Email:" AssociatedControlID="txtAssuredEmail" /><span style="color: red;">*</span></span>
                    <asp:TextBox ID="txtAssuredEmail" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvAssuredEmail" runat="server" ControlToValidate="txtAssuredEmail"
                        ErrorMessage="Assured Email is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server"
                        ControlToValidate="txtAssuredEmail"
                        ErrorMessage="Invalid email format."
                        ForeColor="Red"
                        ValidationExpression="\w+([-+.'']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="save" />
                </div>

                <!-- Vehicle Make -->
                <div>
                    <span>
                        <asp:Label ID="lblVehicleMake" runat="server" Text="Vehicle Make:" AssociatedControlID="ddlVehicleMake" /><span style="color: red;">*</span></span>
                    <asp:DropDownList ID="ddlVehicleMake" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlVehicleMake_SelectedIndexChanged" />
                    <asp:RequiredFieldValidator ID="rfvVehicleMake" runat="server" ControlToValidate="ddlVehicleMake"
                        InitialValue="" ErrorMessage="Vehicle Make is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                </div>

                <!-- Vehicle Model -->
                <div>
                    <span>
                        <asp:Label ID="lblVehicleModel" runat="server" Text="Vehicle Model:" AssociatedControlID="ddlVehicleModel" /><span style="color: red;">*</span></span>
                    <asp:DropDownList ID="ddlVehicleModel" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlVehicleModel_SelectedIndexChanged" />
                    <asp:RequiredFieldValidator ID="rfvVehicleModel" runat="server" ControlToValidate="ddlVehicleModel"
                        InitialValue="" ErrorMessage="Vehicle Model is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                </div>

                <!-- Vehicle Body Type -->
                <div>
                    <span>
                        <asp:Label ID="lblVehicleBodyType" runat="server" Text="Vehicle Body Type:" AssociatedControlID="ddlVehicleBodyType" /><span style="color: red;">*</span></span>
                    <asp:DropDownList ID="ddlVehicleBodyType" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvVehicleBodyType" runat="server" ControlToValidate="ddlVehicleBodyType"
                        InitialValue="" ErrorMessage="Vehicle Body Type is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                </div>

                <!-- Chassis Number -->
                <div>
                    <span>
                        <asp:Label ID="lblChassisNumber" runat="server" Text="Chassis Number:" AssociatedControlID="txtChassisNumber" /><span style="color: red;">*</span></span>
                    <asp:TextBox ID="txtChassisNumber" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvChassisNumber" runat="server" ControlToValidate="txtChassisNumber"
                        ErrorMessage="Chassis Number is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                    <label id="lblChassisNumberMessage" class="text-danger"></label>

                </div>

                <!-- Registration Number -->
                <div>
                    <span>
                        <asp:Label ID="lblRegistrationNumber" runat="server" Text="Registration Number:" AssociatedControlID="txtRegistrationNumber" /><span style="color: red;">*</span></span>
                    <asp:TextBox ID="txtRegistrationNumber" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvRegistrationNumber" runat="server" ControlToValidate="txtRegistrationNumber"
                        ErrorMessage="Registration Number is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                    <label id="lblRegistrationNumberMessage" class="text-danger"></label>
                </div>

                <!-- Start Date -->
                <div>
                    <span>
                        <asp:Label ID="lblFromDate" runat="server" Text="Start Date:" AssociatedControlID="txtFromDate" /><span style="color: red;">*</span></span>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date-picker" onchange="calculateEndDate()" />
                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"
                        ErrorMessage="Start Date is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />

                </div>

                <!-- End Date (Readonly, Calculated) -->
                <div>

                    <asp:Label ID="lblToDate" runat="server" Text="End Date:" AssociatedControlID="txtToDate" />
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date-picker" />

                </div>
                <!-- Net FC Premium -->
                <div>
                    <span>
                        <asp:Label ID="lblNetFcPremium" runat="server" Text="Net FC Premium:" AssociatedControlID="txtNetFcPremium" /><span style="color: red;">*</span></span>
                    <asp:TextBox ID="txtNetFcPremium" runat="server" CssClass="form-control text-right" onchange="updatePremium()" onkeyup="validateNetFcPremium()"  />

                    <asp:RequiredFieldValidator ID="rfvNetFcPremium" runat="server" ControlToValidate="txtNetFcPremium"
                        ErrorMessage="Net FC Premium is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />

                    <span id="lblNetFcPremiumMessage" class="text-danger"></span>
                    <!-- Span for validation message -->
                </div>
                <div>
                    <asp:Label ID="lblNetLcPremium" runat="server" Text="Net LC Premium:" AssociatedControlID="txtNetLcPremium" />
                    <asp:TextBox ID="txtNetLcPremium" runat="server" CssClass="form-control" />
                </div>
                <div>
                    <asp:Label ID="lblVatFcAmount" runat="server" Text="Vat FC Premium:" AssociatedControlID="txtVatFcAmount" />
                    <asp:TextBox ID="txtVatFcAmount" runat="server" CssClass="form-control" />
                </div>
                <div>
                    <asp:Label ID="lblVatLcAmount" runat="server" Text="Vat LC Premium:" AssociatedControlID="txtVatLcAmount" />
                    <asp:TextBox ID="txtVatLcAmount" runat="server" CssClass="form-control" />
                </div>

            </div>
            <!-- Save Policy Button -->

            <div class="d-flex justify-content-center">
            <div class="btn-container">
                <asp:Button ID="btnSavePolicy" runat="server" Text="Save Policy" CssClass="button-action btn-save-color" OnClick="btnSavePolicy_Click" ValidationGroup="save" />
            </div>
                <div class="btn-container">
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="button-action btn-update-color" OnClick="btnUpdate_Click" Visible="false" ValidationGroup="save" />
                </div>
                <div class="btn-container">
                    <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="button-action btn-approve-color" OnClick="btnApprove_Click" Visible="false" ValidationGroup="save" />
                </div>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
