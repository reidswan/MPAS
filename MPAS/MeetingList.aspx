<%@ Page Title="Meetings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeetingList.aspx.cs" Inherits="MPAS.MeetingList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-xs-8"><h3>Meetings</h3></div>
            <div class="col-xs-4">
                <a runat="server" id="MakeMeetingButton" visible="false" class="btn btn-default" style="margin-top:5%" href="MakeMeeting">
                    Make Meeting
                </a>
            </div>
         </div>
        <hr />
    </div>
    <br />
    <asp:Label runat="server" ID="NoMeetings">Nothing to display</asp:Label>
    <asp:Table ID="MeetingTable" runat="server" Visible="false" EnableTheming="true" CssClass="table" style="width:100%">
        <asp:TableHeaderRow>
            <asp:TableCell><h3>Title</h3></asp:TableCell>
            <asp:TableCell><h3>Made by</h3></asp:TableCell>
            <asp:TableCell><h3>Date</h3></asp:TableCell>
            <asp:TableCell><h3>Start time</h3></asp:TableCell>
            <asp:TableCell><h3>End time</h3></asp:TableCell>
        </asp:TableHeaderRow>
    </asp:Table>
</asp:Content>
