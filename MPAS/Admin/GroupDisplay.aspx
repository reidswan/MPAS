<%@ Page Title="Group Assignment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GroupDisplay.aspx.cs" Inherits="MPAS.Admin.GroupDisplay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Group Assignments</h1>
    <div class="row">
        <div class="col-xs-12 col-md-8">
            <asp:Table runat="server" ID="GroupsTable" CssClass="table">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>
                        <h4>Group Number</h4>
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell>
                        <h4>Name</h4>
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell>
                        <h4>Student Number</h4>
                    </asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>
