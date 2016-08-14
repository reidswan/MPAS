<%@ Page Title="Make Announcement" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MakeAnnouncement.aspx.cs" Inherits="MPAS.MakeAnnouncement" %>
<asp:Content ID="AnnouncementContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Make Announcement</h1>
    <hr />
    <div class="container">
        <div class="row">
            <div class="col-xs-4">
                <asp:Label runat="server" ID="Title_Label">Title:</asp:Label>
            </div>
            <div class="col-xs-6">
                <asp:TextBox placeholder="Eg. First Mentor Meeting" runat="server" ID="Title_Textbox" TextMode="SingleLine"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-4">
                <asp:Label runat="server" ID="Label1">Content:</asp:Label>
            </div>
            <div class="col-xs-6">
                <asp:TextBox placeholder="Eg. First meeting has been scheduled" runat="server" ID="Content_Textbox" TextMode="MultiLine" style="width:100%"></asp:TextBox>
            </div>
        </div>
        <div class="row" runat="server" visible="false" id="MentorOptions">
            <div class="col-xs-4">
                <asp:Label runat="server">General Announcement:</asp:Label>
            </div>
            <div class="col-xs-6">
                <asp:CheckBox runat="server" ID="General_CheckBox"/>
            </div>
        </div>
        <div class="row" runat="server" visible="false" id="AdminOptions">
            <div class="col-xs-4">
                <asp:Label runat="server"> Mentor Group:</asp:Label>
            </div>
            <div class="col-xs-6">
                <asp:DropDownList runat="server" ID="Groups_DropDown">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-4">
                <asp:Button runat="server" ID="SubmitButton" Text="Submit" OnClick="SubmitButton_Click"/>
            </div>
        </div>

    </div>

</asp:Content>
