<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPages/TransactionPage.Master" AutoEventWireup="true" CodeBehind="PolicyListing.aspx.cs" Inherits="MotorClaimProcessingSystem.Transaction.PolicyListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.3/font/bootstrap-icons.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <script src="../Script.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid pt-4">
        <div class="">
            <h4 class="">Policy Listing</h4>



            <div class="mx-auto w-75 d-flex justify-content-center flex-column align-items-center bg-light rounded-3 shadow p-4">
                <div class="form-grid">
                    <div class="">
                        <asp:Label ID="lblPolicyNumber" runat="server" Text="Policy Number:" AssociatedControlID="txtPolicyNumber"></asp:Label>
                        <asp:TextBox ID="txtPolicyNumber" runat="server" CssClass="form-control" />
                    </div>
                    <div class="">
                        <asp:Label ID="lblAssuredName" runat="server" Text="Assured Name:" AssociatedControlID="txtAssuredName"></asp:Label>
                        <asp:TextBox ID="txtAssuredName" runat="server" CssClass="form-control" />
                    </div>
                    <div class="">
                        <asp:Label ID="lblStatus" runat="server" Text="Status:" AssociatedControlID="ddlStatus"></asp:Label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Text="All" Value="All" />
                            <asp:ListItem Text="Pending" Value="P" />
                            <asp:ListItem Text="Approved" Value="A" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="">
                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary mt-4" Text="Search" OnClick="btnSearch_Click" />
                </div>
            </div>

            <div class="float-end m-2">
                <asp:LinkButton ID="btnAddPolicy" runat="server" CssClass="btn-add" OnClick="btnAddPolicy_Click">
            <span class="icon"><img src="../Src/Image/add.png" alt="Add" /></span>
            <span class="btn-text">Add</span>
                </asp:LinkButton>
            </div>
        </div>
        <div class="table table-responsive">
            <asp:GridView
                ID="gridViewPolicy"
                runat="server" AllowPaging="true" PageSize="8"
                OnPageIndexChanging="gridViewPolicy_PageIndexChanging"
                OnRowDataBound="gridViewPolicy_RowDataBound"
                OnRowDeleting="gridViewPolicy_RowDeleting" PagerStyle-CssClass="gridview-pagination" OnRowCreated="gridViewPolicy_RowCreated"
                CssClass="table-bg table table-border"
                AutoGenerateColumns="false">
                <Columns>
                    
                    <asp:BoundField DataField="POL_NO" HeaderText="Policy Number" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="POL_ISS_DT" HeaderText="Issue Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="POL_FM_DT" HeaderText="Start Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="POL_TO_DT" HeaderText="End Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="POL_ASSR_NAME" HeaderText="Name" />
                    <asp:BoundField DataField="POL_ASSR_MOBILE" HeaderText="Mobile" ItemStyle-CssClass="text-right" />
                    <asp:BoundField DataField="POL_NET_LC_PREM" HeaderText="Net LC Premium"
                        DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />
                    <asp:BoundField DataField="POL_VAT_LC_AMT" HeaderText="VAT LC Amount"
                        DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <div class="text-center">
                                <asp:Image ID="imageApproval" runat="server" CssClass="status-icon"
                                    ImageUrl="../Src/Image/check-mark.png" Visible="false" ToolTip="Approved" />
                                <asp:Image ID="imagePending" runat="server" CssClass="status-icon"
                                    ImageUrl="../Src/Image/pending.png" Visible="false" ToolTip="Pending" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("POL_UID") %>' OnClick="btnEdit_Click"
                                ImageUrl="../Src/Image/edit.png" CssClass="status-icon" AlternateText="Edit" ToolTip="Edit" />
                            <asp:ImageButton ID="btnView" runat="server" CommandArgument='<%# Eval("POL_UID") %>' OnClick="btnView_Click"
                                ImageUrl="../Src/Image/view.png" CssClass="status-icon" AlternateText="View" ToolTip="View" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center text-dark border-0">
                            <h6>No Records Found</h6>
                        </div>
                    </EmptyDataTemplate>
            </asp:GridView>
            <div>
                <asp:Label ID="lblMessage" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
