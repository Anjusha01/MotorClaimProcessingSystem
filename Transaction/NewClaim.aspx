<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPages/TransactionPage.Master" AutoEventWireup="true" CodeBehind="NewClaim.aspx.cs" Inherits="MotorClaimProcessingSystem.Transaction.NewClaim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <script src="../Script.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            initializeDatePicker(".date-picker");
            var policyStartDate = $('#<%= txtPolicyStartDate.ClientID %>').val();
            var policyEndDate = $('#<%= txtPolicyEndDate.ClientID %>').val();

            if (policyStartDate && policyEndDate) {
                var startDate = $.datepicker.parseDate('dd-mm-yy', policyStartDate);
                var endDate = $.datepicker.parseDate('dd-mm-yy', policyEndDate);
                intiDatePicker('.intimate-date', startDate, endDate);
            } else {
            }
        });


        function intiDatePicker(selector, startDate, endDate) {
            $(selector).datepicker({
                dateFormat: "dd-mm-yy",
                minDate: startDate,
                maxDate: endDate,
                changeMonth: true,
                changeYear: true,
                yearRange: "c-30:c+30"
            });
        }

        function initializeDatePicker(selector) {
            $(selector).datepicker({
                dateFormat: "dd-mm-yy",
                maxDate: 0,
                changeMonth: true,
                changeYear: true,
                yearRange: "c-30:c+30"
            });
        }



        function validateLossDate(source, args) {

            var lossDateStr = $('#<%= txtLossDate.ClientID %>').val();
            var policyStartDateStr = $('#<%= txtPolicyStartDate.ClientID %>').val();
            var policyEndDateStr = $('#<%= txtPolicyEndDate.ClientID %>').val();

            var lossDate = $.datepicker.parseDate('dd-mm-yy', lossDateStr);
            var policyStartDate = $.datepicker.parseDate('dd-mm-yy', policyStartDateStr);
            var policyEndDate = $.datepicker.parseDate('dd-mm-yy', policyEndDateStr);

            var today = new Date();

            var errorMessage = "";

            if (isNaN(lossDate.getTime()) || lossDate > today || lossDate < policyStartDate || lossDate > policyEndDate || lossDate.getFullYear() < 1900) {
                errorMessage = "Loss Date must be between the policy period (" + policyStartDate.toLocaleDateString() + " - " + policyEndDate.toLocaleDateString() + ") and cannot be a future date.";
                args.IsValid = false;
                $('#errorMessageLoss').text(errorMessage).show();
            } else {
                $('#errorMessageLoss').hide();
            }
        }

        function validateIntimationDate(source, args) {

            var intimationDateStr = $('#<%= txtIntimationDate.ClientID %>').val();
            var policyStartDateStr = $('#<%= txtPolicyStartDate.ClientID %>').val();
            var policyEndDateStr = $('#<%= txtPolicyEndDate.ClientID %>').val();


            var intimationDate = $.datepicker.parseDate('dd-mm-yy', intimationDateStr);
            var policyStartDate = $.datepicker.parseDate('dd-mm-yy', policyStartDateStr);
            var policyEndDate = $.datepicker.parseDate('dd-mm-yy', policyEndDateStr);

            var today = new Date();
            args.IsValid = true;
            var errorMessage = "";
            if (isNaN(intimationDate.getTime()) || intimationDate < policyStartDate || intimationDate > policyEndDate || intimationDate.getFullYear() < 1900) {
                errorMessage = "Intimation Date must be between the policy period (" + policyStartDate.toLocaleDateString() + " - " + policyEndDate.toLocaleDateString() + ").";
                args.IsValid = false;
                $('#errorMessageIntimation').text(errorMessage).show();
            } else {
                $('#errorMessageIntimation').hide();
            }
        }



        function maxCharLimitDescription(source, args) {
            var txtDesc = $('#<%=txtDescription.ClientID %>').val();
            var maxChars = 250;

            if (txtDesc.length > maxChars) {
                $('#errorMessageDescription').text("Maximum character limit of " + maxChars + " exceeded.").show();
                args.IsValid = false;
            } else {
                args.IsValid = true;
                $('#errorMessageDescription').hide();
            }
        }

        function validatePoliceRepNo(source, args) {
            var policeRepNo = $('#<%= txtPoliceRepNo.ClientID %>').val();
            var maxChars = 12;

            if (policeRepNo.length > maxChars) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }
        const maxAmount = 1000000;

        function validateEstimateFcAmount(source, args) {
            const value = parseFloat(args.Value);

            if (!isNaN(value) && value > 0 && value <= maxAmount) {
                args.IsValid = true;
                $('#lblEstimateFcAmountError').hide();
            } else if (isNaN(value) || value <= 0) {
                args.IsValid = false;
                $('#lblEstimateFcAmountError').text("Please enter a valid number greater than 0.").show();
            } else {
                args.IsValid = false;
                $('#lblEstimateFcAmountError').text("Amount cannot exceed " + maxAmount + ".").show();
            }
        }

        function openEditModal(ceUid, currency, estimatedAmount) {
            document.getElementById('<%= ddlEstimateCurrency.ClientID %>').value = currency;
            document.getElementById('<%= txtEstimateFcAmount.ClientID %>').value = estimatedAmount;

            document.getElementById('<%= hfCeUid.ClientID %>').value = ceUid;

            document.getElementById('<%= btnUpdateEstimate.ClientID %>').style.display = 'inline-block';
            document.getElementById('<%= btnSubmitEstimate.ClientID %>').style.display = 'none';
            $('#addEstimateModal').modal('show');
        }
        $('#addEstimateModal').on('hidden.bs.modal', function () {
            document.getElementById('<%= btnUpdateEstimate.ClientID %>').style.display = 'none';
            document.getElementById('<%= btnSubmitEstimate.ClientID %>').style.display = 'inline-block';
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    <div class="pt-4 container-fluid">
        <div class="d-flex justify-content-between">
            <h3>Add Claim Details</h3>
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="btn-back float-end" />
        </div>
        <asp:HiddenField ID="txtPolicyStartDate" runat="server" />
        <asp:HiddenField ID="txtPolicyEndDate" runat="server" />

        <div class="form-grid">
            <!-- Policy Number -->
           <%-- <div>
                <span>
                    <asp:Label ID="lblPolicyNumber" runat="server" Text="Policy Number:" AssociatedControlID="txtPolicyNumber" />
                    <span class="text-danger">*</span>
                </span>

                <asp:TextBox ID="txtPolicyNumber" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtPolicyNumber_TextChanged" />
                <asp:RequiredFieldValidator ID="rfvPolicyNumber" runat="server" ControlToValidate="txtPolicyNumber"
                    ErrorMessage="Policy Number is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
            </div>--%>

            <div>
                <span>
                    <asp:Label ID="lblPolNo" runat="server" Text="Policy Number:" AssociatedControlID="txtClaimNumber" />
                    <span class="text-danger">*</span>
                </span>
                <asp:DropDownList ID="ddlPolNo" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPolNo_SelectedIndexChanged" AutoPostBack="true" />
                <asp:RequiredFieldValidator ID="rfvPolicyNumber" runat="server" ControlToValidate="ddlPolNo"
                   InitialValue="" ErrorMessage="Policy Number is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
            </div>
            <!-- Claim Number -->
            <div>
                <asp:Label ID="lblClaimNumber" runat="server" Text="Claim Number:" AssociatedControlID="txtClaimNumber" />
                <asp:TextBox ID="txtClaimNumber" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvClaimNo" runat="server" ControlToValidate="txtClaimNumber"
                    ErrorMessage="" CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
            </div>
            <!-- Vehicle Make -->
            <div>
                <asp:Label ID="lblVehicleMake" runat="server" Text="Vehicle Make:" AssociatedControlID="txtVehicleMake" />
                <asp:TextBox ID="txtVehicleMake" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtClaimNumber"
                    ErrorMessage="" CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
            </div>

            <!-- Vehicle Model -->
            <div>
                <asp:Label ID="lblVehicleModel" runat="server" Text="Vehicle Model:" AssociatedControlID="txtVehicleModel" />
                <asp:TextBox ID="txtVehicleModel" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvModel" runat="server" ControlToValidate="txtVehicleModel"
                    ErrorMessage="" CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
            </div>

            <!-- Vehicle Body Type -->
            <div>
                <asp:Label ID="lblVehicleBodyType" runat="server" Text="Vehicle Body Type:" AssociatedControlID="txtVehicleBodyType" />
                <asp:TextBox ID="txtVehicleBodyType" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvBody" runat="server" ControlToValidate="txtVehicleBodyType"
                    ErrorMessage="" CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
            </div>

            <!-- Chassis Number -->
            <div>
                <asp:Label ID="lblChassisNumber" runat="server" Text="Chassis Number:" AssociatedControlID="txtChassisNumber" />
                <asp:TextBox ID="txtChassisNumber" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvChasis" runat="server" ControlToValidate="txtChassisNumber"
                    ErrorMessage="" CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />

            </div>
            <!-- Registration Number -->
            <div>
                <asp:Label ID="lblRegistrationNumber" runat="server" Text="Registration Number:" AssociatedControlID="txtRegistrationNumber" />
                <asp:TextBox ID="txtRegistrationNumber" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvRegNo" runat="server" ControlToValidate="txtRegistrationNumber"
                    ErrorMessage="" CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
            </div>

            <!-- Loss Date -->
            <div>
                <span>
                    <asp:Label ID="lblLossDate" runat="server" Text="Loss Date:" AssociatedControlID="txtLossDate" />
                    <span class="text-danger">*</span>
                </span>
                <asp:TextBox ID="txtLossDate" runat="server" CssClass="form-control date-picker" />
                <asp:RequiredFieldValidator ID="rfvLossDate" runat="server" ControlToValidate="txtLossDate"
                    ErrorMessage="Loss Date is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                <asp:CustomValidator ID="cvLossDate" runat="server" ControlToValidate="txtLossDate"
                    ClientValidationFunction="validateLossDate"
                    CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                <span id="errorMessageLoss" class="text-danger"></span>
            </div>

            <!-- Loss Type -->
            <div>
                <span>
                    <asp:Label ID="lblLossType" runat="server" Text="Loss Type:" AssociatedControlID="ddlLossType" />
                    <span class="text-danger">*</span>
                </span>
                <asp:DropDownList ID="ddlLossType" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvLossType" runat="server" ControlToValidate="ddlLossType"
                    InitialValue="" ErrorMessage="Loss Type is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
            </div>

            <!-- Fault Type -->
            <div>
                <span>
                    <asp:Label ID="lblFaultType" runat="server" Text="Fault Type:" AssociatedControlID="ddlFaultType" />
                    <span class="text-danger">*</span>
                </span>
                <asp:DropDownList runat="server" ID="ddlFaultType" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvFaultType" runat="server" ControlToValidate="ddlFaultType"
                    InitialValue="" ErrorMessage="Fault Type is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
            </div>

            <!-- Accident Location -->
            <div>
                <span>
                    <asp:Label ID="lblAccidentLocation" runat="server" Text="Accident Location:" AssociatedControlID="ddlAccidentLocation" />
                    <span class="text-danger">*</span>
                </span>
                <asp:DropDownList runat="server" ID="ddlAccidentLocation" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvAccidentLocation" runat="server" ControlToValidate="ddlAccidentLocation"
                    InitialValue="" ErrorMessage="Accident location is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
            </div>

            <!-- Intimation Date -->
            <div>
                <span>
                    <asp:Label ID="lblIntimationDate" runat="server" Text="Intimation Date:" AssociatedControlID="txtIntimationDate" />
                    <span class="text-danger">*</span>
                </span>
                <asp:TextBox ID="txtIntimationDate" runat="server" CssClass="form-control intimate-date" />
                <asp:RequiredFieldValidator ID="rfvIntimationDate" runat="server" ControlToValidate="txtIntimationDate"
                    ErrorMessage="Intimation Date is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                <asp:CustomValidator ID="cvIntimationDate" runat="server" ControlToValidate="txtIntimationDate"
                    ClientValidationFunction="validateIntimationDate"
                    CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                <span id="errorMessageIntimation" class="text-danger"></span>
            </div>

            <!-- Registration Date -->
            <div>
                <span>
                    <asp:Label ID="lblRegistrationDate" runat="server" Text="Registration Date:" AssociatedControlID="txtRegistrationDate" />
                    <span class="text-danger">*</span>
                </span>
                <asp:TextBox ID="txtRegistrationDate" runat="server" CssClass="form-control date-picker" />
                <asp:RequiredFieldValidator ID="rfvRegistrationDate" runat="server" ControlToValidate="txtRegistrationDate"
                    ErrorMessage="Registration Date is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
            </div>

            <%-- Police Report No --%>
            <div>
                <span>
                    <asp:Label ID="lblPoliceRepNo" runat="server" Text="Police Report No:" AssociatedControlID="txtPoliceRepNo" />
                    <span class="text-danger">*</span>
                </span>
                <asp:TextBox ID="txtPoliceRepNo" runat="server" CssClass="form-control" />


                <asp:RequiredFieldValidator
                    ID="rfvPoliceRepNo"
                    runat="server"
                    ControlToValidate="txtPoliceRepNo"
                    ErrorMessage="Police Report Number required."
                    CssClass="text-danger"
                    Display="Dynamic"
                    ValidationGroup="save" />


                <asp:CustomValidator
                    ID="cvPoliceRepNo"
                    runat="server"
                    ControlToValidate="txtPoliceRepNo"
                    ClientValidationFunction="validatePoliceRepNo"
                    ErrorMessage="Police Report Number cannot exceed 12 characters."
                    CssClass="text-danger"
                    Display="Dynamic"
                    ValidationGroup="save" />
            </div>

            <!-- Description -->
            <div>
                <asp:Label ID="lblDescription" runat="server" Text="Description:" AssociatedControlID="txtDescription" />
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                <asp:CustomValidator ID="cvDescription" runat="server" ControlToValidate="txtDescription"
                    ClientValidationFunction="maxCharLimitDescription"
                    CssClass="text-danger" Display="Dynamic" ValidationGroup="save" />
                <span id="errorMessageDescription" class="text-danger" style="display: none;"></span>
            </div>

        </div>

        <!-- Save Claim Button -->
        <div class="d-flex justify-content-center">
            <asp:Button ID="btnSaveClaim" runat="server" Text="Save" CssClass="button-action btn-save-color" OnClick="btnSaveClaim_Click" ValidationGroup="save" />
            <asp:Button ID="btnUpdateClaim" runat="server" Text="Update" CssClass="button-action btn-update-color" OnClick="btnUpdateClaim_Click" Visible="false" ValidationGroup="save" />
            <asp:Button ID="btnCloseClaim" runat="server" Text="Close" CssClass="button-action btn-save-color" OnClick="btnCloseClaim_Click" Visible="false" ValidationGroup="save" />
            <asp:Button ID="btnAddClaimEstimate" runat="server" Text="Add Claim Estimate" CssClass="button-action btn-add-color"
                OnClientClick="$('#addEstimateModal').modal('show'); return false;" Visible="false" />

        </div>

        <div>
            <asp:Label ID="lblMessage" runat="server" CssClass="text-success"></asp:Label>
        </div>
    </div>
    <div class="container-fluid">
        <%--<h3>ClaimEstimation</h3>--%>
        <div class="table table-responsive">
            <asp:Label ID="ltCaption" runat="server" Text="Claim Estimates" CssClass=" h4 font-weight-bold" />
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvClaimEstimates" runat="server" AutoGenerateColumns="False" CssClass="table-bg table table-border" OnRowDataBound="gvClaimEstimates_RowDataBound" OnRowCommand="gvClaimEstimates_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="CE_CURR" HeaderText="Currency" SortExpression="CE_CURR" />
                            <asp:BoundField DataField="CE_EST_FC_AMT" HeaderText="Estimated FC" ItemStyle-CssClass="text-right" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="CE_EST_VAT_FC_AMT" HeaderText="VAT FC" ItemStyle-CssClass="text-right" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="CE_EST_LC_AMT" HeaderText="Estimated LC" ItemStyle-CssClass="text-right" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="CE_EST_VAL_LC_AMT" HeaderText="VAT LC" ItemStyle-CssClass="text-right" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="CE_CR_DT" HeaderText="Estimate Date" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="CE_PAID_FC_AMT" HeaderText="Paid FC" ItemStyle-CssClass="text-right" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="CE_PAID_LC_AMT" HeaderText="Paid LC" ItemStyle-CssClass="text-right" DataFormatString="{0:N2}" />
                            <asp:TemplateField HeaderText="Status" SortExpression="CE_STATUS">
                                <ItemTemplate>
                                    <div class="text-center">
                                        <asp:Image ID="imgEstimateOpen" runat="server"
                                            ImageUrl="../Src/Image/open.png"
                                            AlternateText="Open"
                                            CssClass="status-icon"
                                            ToolTip="Estimate is Open"
                                            Visible="false" />
                                        <asp:Image ID="imgEstimateApproved" runat="server"
                                            ImageUrl="../Src/Image/check-mark.png"
                                            AlternateText="Open"
                                            CssClass="status-icon"
                                            ToolTip="Estimate is Approved"
                                            Visible="false" />
                                        <asp:Image ID="imgEstimateClosed" runat="server"
                                            ImageUrl="../Src/Image/closedestimate.png"
                                            AlternateText="Open"
                                            CssClass="status-icon"
                                            ToolTip="Estimate is Closed"
                                            Visible="false" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <div class="text-center">
                                        <asp:ImageButton
                                            ID="btnEdit"
                                            runat="server"
                                            CommandName="EditEstimate"
                                            CommandArgument='<%# Eval("CE_UID") + "," + Eval("CE_CURR") + "," + Eval("CE_EST_FC_AMT") %>'
                                            CssClass="status-icon"
                                            ImageUrl="../Src/Image/edit.png"
                                            AlternateText="Edit"
                                            ToolTip="Edit Estimate"
                                            OnClientClick='<%# "openEditModal(\"" + Eval("CE_UID") + "\", \"" + Eval("CE_CURR") + "\", \"" + Eval("CE_EST_FC_AMT") + "\");return false;" %>' />


                                        <%--OnClientClick="openEditModal();return false;" />--%>
                                        <%--OnClientClick="openEditModal('<%# Eval("CE_UID") %>', '<%# Eval("CE_CURR") %>', '<%# Eval("CE_EST_FC_AMT") %>');" />--%>


                                        <asp:ImageButton ID="btnApprove" runat="server" CommandName="ApproveEstimate"
                                            CommandArgument='<%# Eval("CE_UID") + "," + Eval("CE_CURR") + "," + Eval("CE_EST_FC_AMT") + "," + Eval("CE_EST_LC_AMT") + "," + Eval("CE_EST_VAT_FC_AMT") + "," + Eval("CE_EST_VAL_LC_AMT") %>'
                                            CssClass="status-icon"
                                            ImageUrl="../Src/Image/approve.png"
                                            AlternateText="Approve"
                                            ToolTip="Approve Estimate"
                                            OnClientClick="return confirm('Are you sure you want to approve this estimate?');" />
                                        <asp:ImageButton ID="btnClose" runat="server" CommandName="CloseEstimate"
                                            CommandArgument='<%# Eval("CE_UID") %>'
                                            CssClass="status-icon"
                                            ImageUrl="../Src/Image/close.png"
                                            AlternateText="Close"
                                            ToolTip="Close Estimate"
                                            OnClientClick="return confirm('Are you sure you want to close this estimate?');" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div>
    <!--Claim Estimate Modal-->
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="modal fade" id="addEstimateModal" tabindex="-1" role="dialog" aria-labelledby="addEstimateModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="addEstimateModalLabel">Add Claim Estimate</h5>
                            <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="hfCeUid" runat="server" />
                            <div class="form-group">
                                <label for="txtEstimateCurrency">Currency:</label>
                                <asp:DropDownList ID="ddlEstimateCurrency" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvEstimateCurrency" runat="server" ControlToValidate="ddlEstimateCurrency"
                                    ErrorMessage="Currency is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="add" />
                            </div>
                            <div class="form-group">
                                <label for="txtEstimateFcAmount">Estimated Amount:</label>
                                <asp:TextBox ID="txtEstimateFcAmount" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvEstimateFcAmount" runat="server" ControlToValidate="txtEstimateFcAmount"
                                    ErrorMessage="Amount is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="add" />

                                <asp:CustomValidator ID="cvEstimateFcAmount" runat="server" ControlToValidate="txtEstimateFcAmount"
                                    CssClass="text-danger" Display="Dynamic" ValidationGroup="add"
                                    ClientValidationFunction="validateEstimateFcAmount" />

                                <label id="lblEstimateFcAmountError" class="text-danger" style="display: none;"></label>
                            </div>

                        </div>
                        <div class="modal-footer">
                            <div class="d-flex justify-content-center">
                                <button type="button" class="btn-close-color button-action" data-bs-dismiss="modal">Close</button>
                                <asp:Button ID="btnSubmitEstimate" runat="server" Text="Submit" CssClass="button-action btn-save-color"
                                    OnClick="btnSaveEstimate_Click" ValidationGroup="add" />
                                <asp:Button ID="btnUpdateEstimate" runat="server" Text="Update" CssClass="button-action btn-update-color"
                                    OnClick="btnUpdateEstimate_Click" ValidationGroup="add" />
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
