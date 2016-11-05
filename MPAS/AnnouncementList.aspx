<%@ Page Title="Announcements" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnnouncementList.aspx.cs" Inherits="MPAS.AnnouncementList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-xs-8"><h1>Announcements</h1></div>
            <div class="col-xs-4">
                <a runat="server" id="MakeAnnouncementButton" visible="false" class="btn btn-default" style="margin-top:5%" href="MakeAnnouncement">
                    Make Announcement
                </a>
            </div>
         </div>
    </div>
    <br />
    <asp:Label runat="server" ID="NoAnnouncements">Nothing to display</asp:Label>
    <asp:Table ID="AnnouncementTable" runat="server" Visible="false" EnableTheming="true" CssClass="table" style="width:100%">
        <asp:TableHeaderRow>
            <asp:TableCell><h3>Title</h3></asp:TableCell>
            <asp:TableCell><h3>Made by</h3></asp:TableCell>
            <asp:TableCell><h3>Date</h3></asp:TableCell>
        </asp:TableHeaderRow>
    </asp:Table>
</asp:Content>


