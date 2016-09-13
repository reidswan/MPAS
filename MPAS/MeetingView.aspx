<%@ Page Title="View Meeting" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeetingView.aspx.cs" Inherits="MPAS.MeetingView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Meeting</h3>
    <hr />
    <div class="row">
        <div class="col-md-8 col-xs-12">
            <div class="row">
                <div class="col-md-12 col-xs-12">
                    <h3><asp:Label runat="server" ID="MeetingTitle_Label">Lorem Ipsum</asp:Label></h3>
                </div>
                <div class="col-md-6 col-xs-6">
                    <h5><asp:Label runat="server" ID="MeetingGroup_Label">General Meeting</asp:Label></h5>
                </div>
                <div class="col-md-6 col-xs-6">
                    <h5><asp:Label runat="server" ID="Location_Label">General Meeting</asp:Label></h5>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 col-xs-4">
                    <h5><asp:Label runat="server" ID="MeetingDate_Label">10/10/2010</asp:Label></h5>
                </div>
                <div class="col-md-8 col-xs-8">
                    <h5><asp:Label runat="server" ID="MeetingTime_Label">10:00 - 12:00</asp:Label></h5>
                </div>
            </div>
            <div class="row ">
                <div class="col-md-12 col-xs-12">
                    <p><asp:Label runat="server" ID="Agenda_Label">"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."</asp:Label></p>
                </div>
            </div>
        </div>
    </div>
    <asp:Label runat="server" ID="Label1"></asp:Label>
</asp:Content>
