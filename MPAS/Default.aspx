<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MPAS._Default"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="padding:0%; margin:0%">
        <asp:Image ImageUrl="~/Images/uctgreenmile.jpg" runat="server" cssclass="img-responsive"/>
    </div>

    <div class="row">
        <div class="col-md-4 col-xs-12">
            <h3>Announcements</h3>
            <asp:Table runat="server" ID="AnnouncementTable" CssClass="table">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Title</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Date</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
            <hr />
            <div class="col-offset-xs-2 col-xs-10">
                <a class="btn btn-default" href="AnnouncementList.aspx">View all &raquo;</a>
            </div>
        </div>
        <div class="col-md-4 col-xs-12">
            <h3>Upcoming Meetings</h3>
            <asp:Table runat="server" ID="MeetingsTable" CssClass="table">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Location</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Time</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
            <hr />
            <div class="col-offset-xs-2 col-xs-10">
                <a class="btn btn-default" href="MeetingList.aspx">View all &raquo;</a>
            </div>
        </div>
        <div class="col-md-4 col-xs-12">
            <h3>Recent Messages</h3>
            <asp:Table runat="server" ID="MessageTable" CssClass="table">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Sender</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Content</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
            <hr />
            <div class="col-offset-xs-2 col-xs-10">
                <a class="btn btn-default" href="GroupChat.aspx">View all &raquo;</a>
            </div>
        </div>
    </div>

</asp:Content>
