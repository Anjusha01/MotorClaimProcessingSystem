<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPages/AdminPage.Master" AutoEventWireup="true" CodeBehind="CodeMasterListing.aspx.cs" Inherits="MotorClaimProcessingSystem.Master.CodeMasterListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <script src="../Script.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mt-3">
        <div class="float-end m-2">
            <asp:LinkButton ID="btnAddCode" runat="server" CssClass="btn-add" OnClick="btnAddCode_Click">
            <span class="icon"><img src="../Src/Image/add.png" alt="Add" /></span>
            <span class="btn-text">Add</span>
            </asp:LinkButton>
        </div>
        <h2 class="mt-5">Code Master Listing</h2>
        <div class="table table-responsive">
            <asp:GridView
                ID="gridViewCodeMaster"
                runat="server"
                OnRowDataBound="gridViewCodeMaster_RowDataBound"
                OnRowDeleting="gridViewCodeMaster_RowDeleting"
                CssClass="table-bg table table-border"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="Code">
                        <ItemTemplate>
                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("CM_CODE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Type">
                        <ItemTemplate>
                            <asp:Label ID="lblType" runat="server" Text='<%# Eval("CM_TYPE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="CM_DESC" HeaderText="Description" />
                    <asp:BoundField DataField="CM_VALUE" HeaderText="Value" ItemStyle-CssClass="text-right" />
                    <asp:BoundField DataField="CM_PARENT_CODE" HeaderText="Parent Code" />
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
                                <asp:ImageButton ID="btnAction" runat="server" ImageUrl="../Src/Image/edit.png" CssClass="status-icon"
                                    CommandArgument='<%# Eval("CM_CODE") + "," + Eval("CM_TYPE") %>' OnClick="btnAction_Click"
                                    AlternateText="Edit" ToolTip="Edit" />

                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="../Src/Image/delete.png" CssClass="status-icon"
                                    CommandName="Delete" AlternateText="Delete" ToolTip="Delete"
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
