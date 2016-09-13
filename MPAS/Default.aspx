<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MPAS._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="padding:0%; margin:0%">
        <asp:Image ImageUrl="~/Images/uctgreenmile.jpg" runat="server" cssclass="img-responsive"/>
    </div>

    <div class="row">
        <div class="col-md-4 col-xs-12s">
            <h3>Announcements</h3>
            <div class="row">
                <div class="col-md-7 col-xs-6">
                    <h4>Title</h4>
                </div>
                <div class="col-md-5 col-xs-6">
                    <h4>Date</h4>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7 col-xs-6">
                    <asp:Label runat="server" ID="AnnouncementTitle1_Label"></asp:Label>
                </div>
                <div class="col-md-5 col-xs-6">
                    <asp:Label runat="server" ID="AnnouncementDate1_Label"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7 col-xs-6">
                    <asp:Label runat="server" ID="AnnouncementTitle2_Label"></asp:Label>
                </div>
                <div class="col-md-5 col-xs-6">
                    <asp:Label runat="server" ID="AnnouncementDate2_Label"></asp:Label>
                </div>
            </div>
            <hr />
            <div class="col-offset-xs-2 col-xs-10">
                <a class="btn btn-default" href="AnnouncementList.aspx">View all &raquo;</a>
            </div>
        </div>
        <div class="col-md-4 col-xs-12">
            <h3>Upcoming Meetings</h3>
            <div class="row">
                <div class="col-md-7 col-xs-6">
                    <h4>Location</h4>
                </div>
                <div class="col-md-5 col-xs-6">
                    <h4>Time</h4>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7 col-xs-6">
                    <asp:Label runat="server" ID="MeetingLocation1_Label"></asp:Label>
                </div>
                <div class="col-md-5 col-xs-6">
                    <asp:Label runat="server" ID="MeetingDate1_Label"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7 col-xs-6">
                    <asp:Label runat="server" ID="MeetingLocation2_Label"></asp:Label>
                </div>
                <div class="col-md-5 col-xs-6">
                    <asp:Label runat="server" ID="MeetingDate2_Label"></asp:Label>
                </div>
            </div>
            <hr />
            <div class="col-offset-xs-2 col-xs-10">
                <a class="btn btn-default" href="AnnouncementList.aspx">View all &raquo;</a>
            </div>
        </div>
        <div class="col-md-4">
            
        </div>
    </div>

</asp:Content>
