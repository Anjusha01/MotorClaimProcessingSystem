<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPages/TransactionPage.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="MotorClaimProcessingSystem.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <script>
        $(function () {

            $("#<%= txtStartDate.ClientID %>").datepicker({
            dateFormat: "dd-mm-yy",
            maxDate: 0,
            changeMonth: true,
            changeYear: true,
            yearRange: "c-30:c+30",
            onSelect: function (selectedDate) {

                $("#<%= txtEndDate.ClientID %>").datepicker("option", "minDate", selectedDate);
            }
        });

        $("#<%= txtEndDate.ClientID %>").datepicker({
            dateFormat: "dd-mm-yy",
            maxDate: 0,
            changeMonth: true,
            changeYear: true,
            yearRange: "c-30:c+30",
            onSelect: function (selectedDate) {

                $("#<%= txtStartDate.ClientID %>").datepicker("option", "maxDate", selectedDate);
            }
        });
    });
    </script>
    <link href="Src/CssStyles.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="report-heading">Claim Estimate Report</h2>
    <div class="w-25 mx-auto">
        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control report-input" placeholder="Start Date"></asp:TextBox>
        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control report-input" placeholder="End Date"></asp:TextBox>
        <asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" OnClick="btnGenerateReport_Click" CssClass="button-action btn-update-color w-100" />
    </div>
</asp:Content>
