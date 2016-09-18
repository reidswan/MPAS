<%@ Page Title="Inbox" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inbox.aspx.cs" Inherits="MPAS.Inbox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-xs-8"><h3>Inbox</h3></div>
            <div class="col-xs-4">
                <a runat="server" id="NewMessageButton"  class="btn btn-default" style="margin-top:5%" href="NewMessage">
                    New Message
                </a>
            </div>
        </div>
        <div class="row">
            <asp:Label runat="server" ID="NoMessages">Nothing to display</asp:Label>
            <asp:Table ID="MessageThreadTable" runat="server" Visible="false" EnableTheming="true" CssClass="table" style="width:100%">
                <asp:TableHeaderRow>
                    <asp:TableCell><h3>From</h3></asp:TableCell>
                    <asp:TableCell><h3>Content</h3></asp:TableCell>
                    <asp:TableCell><h3>Date</h3></asp:TableCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>
