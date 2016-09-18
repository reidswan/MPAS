<%@ Page Title="Group Assignment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GroupDisplay.aspx.cs" Inherits="MPAS.Admin.GroupDisplay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Group Assignments</h3>
    <asp:Table runat="server" ID="GroupsTable">
        <asp:TableHeaderRow>
            <asp:TableCell>
                Group Number
            </asp:TableCell>
            <asp:TableCell>
                Name
            </asp:TableCell>
            <asp:TableCell>
                Student Number
            </asp:TableCell>
        </asp:TableHeaderRow>
    </asp:Table>
</asp:Content>
