<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPages/TransactionPage.Master" AutoEventWireup="true" CodeBehind="ClaimListing.aspx.cs" Inherits="MotorClaimProcessingSystem.Transaction.ClaimListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <script src="../Script.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pt-4 container-fluid">
        <h3>Claim Listing</h3>
        <div class="mx-auto w-75 d-flex justify-content-center flex-column align-items-center bg-light rounded-3 shadow p-4">
            <div class="form-grid">
                <asp:TextBox ID="txtClaimNo" runat="server" CssClass="form-control" placeholder="Claim Number"></asp:TextBox>
                <asp:TextBox ID="txtPolicyNo" runat="server" CssClass="form-control" placeholder="Policy Number"></asp:TextBox>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                    <asp:ListItem Text="Open" Value="O"></asp:ListItem>
                    <asp:ListItem Text="Closed" Value="C"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-save-color button-action" Text="Search" OnClick="btnSearch_Click" />
        </div>
        <div class="float-end m-2">
            <asp:LinkButton ID="btnAddClaim" runat="server" CssClass="btn-add" OnClick="btnAddClaim_Click">
            <span class="icon"><img src="../Src/Image/add.png" alt="Add" /></span>
            <span class="btn-text">Add</span>
            </asp:LinkButton>
        </div>
        <div>
            <div class="table table-responsive">
                <asp:GridView
                    ID="gridViewClaim"
                    runat="server" OnRowDataBound="gridViewClaim_RowDataBound" OnPageIndexChanging="gridViewClaim_PageIndexChanging" AllowPaging="true" PageSize="8" OnRowCreated="gridViewClaim_RowCreated"
                    CssClass="table-bg table table-border"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="CLM_NO" HeaderText="Claim Number" SortExpression="CLM_NO" />
                        <asp:BoundField DataField="POL_NO" HeaderText="Policy Number" SortExpression="CLM_POL_NO" />
                        <asp:BoundField DataField="POL_FM_DT" HeaderText="From Date" SortExpression="POL_FM_DT" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="POL_TO_DT" HeaderText="To Date" SortExpression="POL_TO_DT" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="CLM_LOSS_DT" HeaderText="Loss Date" SortExpression="CLM_LOSS_DT" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="CLM_INTM_DT" HeaderText="Intimation Date" SortExpression="CLM_INTM_DT" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="POL_ASSR_NAME" HeaderText="Assured Name" SortExpression="POL_ASSR_NAME" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <div class="text-center">
                                    <asp:Image ID="imgClmStatusOpen" runat="server" CssClass="status-icon"
                                        ImageUrl="../Src/Image/open.png"
                                        AlternateText="Open"
                                        ToolTip="Claim is Open"
                                        Visible="false" />
                                    <asp:Image ID="imgClmStatusClose" runat="server" CssClass="status-icon"
                                        ImageUrl="../Src/Image/closed.png"
                                        AlternateText="Open"
                                        ToolTip="Claim is Closed"
                                        Visible="false" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <div class="text-center">
                                    <asp:ImageButton ID="btnEdit" runat="server" CssClass="status-icon"
                                        CommandArgument='<%# Eval("CLM_UID") %>'
                                        OnClick="btnEdit_Click"
                                        ImageUrl="../Src/Image/edit.png"
                                        AlternateText="Edit"
                                        ToolTip="Edit Claim" />

                                    <asp:ImageButton ID="btnView" runat="server" CssClass="status-icon"
                                        CommandArgument='<%# Eval("CLM_UID") %>'
                                        OnClick="btnView_Click"
                                        ImageUrl="../Src/Image/view.png"
                                        AlternateText="View"
                                        ToolTip="View Claim"
                                        Visible="false" />

                                    <asp:ImageButton ID="btnClose" runat="server" CssClass="status-icon"
                                        CommandArgument='<%# Eval("CLM_UID") %>'
                                        OnClick="btnClose_Click"
                                        ImageUrl="../Src/Image/close.png"
                                        AlternateText="Close"
                                        ToolTip="Close Claim"
                                        OnClientClick="return confirm('Are you sure you want to close this claim?');" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center text-dark border-0">
                            <h6>No Records Found</h6>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
