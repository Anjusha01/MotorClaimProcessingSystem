<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPages/AdminPage.Master" AutoEventWireup="true" CodeBehind="UserMasterListing.aspx.cs" Inherits="MotorClaimProcessingSystem.Master.UserMasterListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <script src="../Script.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mt-3">
        <div class="float-end m-2">
            <asp:LinkButton ID="btnAddUser" runat="server" CssClass="btn-add" OnClick="btnAddUser_Click">
                <span class="icon"><img src="../Src/Image/add.png" alt="Add" /></span>
            <span class="btn-text">Add</span>
            </asp:LinkButton>
        </div>
        <h2 class="mt-5">User Master Listing</h2>
        <div class="table table-responsive">
            <asp:GridView
                ID="gridViewUserMaster"
                runat="server"
                OnRowDeleting="gridViewUserMaster_RowDeleting" 
                OnRowDataBound="gridViewUserMaster_RowDataBound"
                CssClass="table-bg table table-border"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="User ID">
                        <ItemTemplate>
                            <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("USER_ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="USER_NAME" HeaderText="User Name" />
                    <asp:TemplateField HeaderText="Active">
                        <ItemTemplate>
                            <div class="text-center">
                                <asp:Image ID="imgActiveYes" runat="server" CssClass="status-icon"
                                    ImageUrl="../Src/Image/closed.png"
                                    AlternateText="Yes"
                                    ToolTip="Active"
                                    Visible="false" />
                                <asp:Image ID="imgActiveNo" runat="server" CssClass="status-icon"
                                    ImageUrl="../Src/Image/open.png"
                                    AlternateText="No"
                                    ToolTip="Not Active"
                                    Visible="false" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <div class="text-center">
                                <asp:ImageButton ID="btnEdit"
                                    runat="server"
                                    ImageUrl="../Src/Image/edit.png"
                                    CssClass="status-icon"
                                    AlternateText="Edit" ToolTip="Edit"
                                    CommandArgument='<%# Eval("USER_ID") %>'
                                    OnClick="btnEdit_Click" />
                                <asp:ImageButton ID="lnkDelete"
                                    runat="server" CommandName="Delete"
                                    ImageUrl="../Src/Image/delete.png"
                                    CssClass="status-icon"
                                    AlternateText="Delete" ToolTip="Delete"
                                    OnClientClick="return confirm('Are you sure you want to delete this user?');" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div>
                <asp:Label runat="server" ID="lblMessage" />
            </div>
        </div>
    </div>
</asp:Content>
