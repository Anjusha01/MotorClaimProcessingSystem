<%@ Page Title="Error Code Master Listing" Language="C#" MasterPageFile="~/SiteMasterPages/AdminPage.Master" AutoEventWireup="true" CodeBehind="ErrorCodeMasterListing.aspx.cs" Inherits="MotorClaimProcessingSystem.Master.ErrorCodeMasterListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <script src="../Script.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mt-3">
        <asp:Panel runat="server">
            <div class="float-end m-2">
                <asp:LinkButton ID="btnAddNew" runat="server" CssClass="btn-add" OnClick="btnAddNew_Click">
                    <span class="icon"><img src="../Src/Image/add.png" alt="Add" /></span>
                    <span class="btn-text">Add</span>
                </asp:LinkButton>
            </div>
            <h2 class="mt-5">Error Code Master Listing</h2>
            <div class="table table-responsive">

                <asp:GridView ID="gvErrorCodeMaster" runat="server" AutoGenerateColumns="False" CssClass="table-bg table table-border" OnRowCommand="gvErrorCodeMaster_RowCommand" OnRowDeleting="gvErrorCodeMaster_RowDeleting">
                    <Columns>
                        <asp:TemplateField HeaderText="Error Code">
                            <ItemTemplate>
                                <asp:Label ID="lblErrorCode" runat="server" Text='<%# Eval("ERR_CODE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ERR_TYPE" HeaderText="Error Type" />
                        <asp:BoundField DataField="ERR_DESC" HeaderText="Error Description" />

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <div class="text-center">
                                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="EditErrorCode" CommandArgument='<%# Eval("ERR_CODE") %>'
                                        ImageUrl="../Src/Image/edit.png" CssClass="status-icon" ToolTip="Edit"
                                        AlternateText="Edit" />

                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ERR_CODE") %>'
                                        ImageUrl="../Src/Image/delete.png" CssClass="status-icon" ToolTip="Delete"
                                        AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this error code?');" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>



                <div>
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
