<%@ Page Title="Make Announcement" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MakeAnnouncement.aspx.cs" Inherits="MPAS.MakeAnnouncement" %>
<asp:Content ID="AnnouncementContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Make Announcement</h2>
    <hr />

    <div class="row form-horizontal">
        <div class="container col-md-8">
            <div class="form-group">
                <div class="col-md-4 col-xs-6">
                    <asp:Label runat="server" ID="Title_Label" >Title:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-6">
                    <asp:TextBox placeholder="Eg. First Mentor Meeting" runat="server" ID="Title_Textbox" TextMode="SingleLine" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4 col-xs-6">
                    <asp:Label runat="server" ID="Label1">Content:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-6">
                    <asp:TextBox placeholder="Eg. First meeting has been scheduled" runat="server" ID="Content_Textbox" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                </div>
            </div>

            <div class="form-group" runat="server" visible="false" id="MentorOptions">
                <div class="col-md-4 col-xs-6">
                    <asp:Label runat="server">General Announcement:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-6">
                    <asp:CheckBox runat="server" ID="General_CheckBox"/>
                </div>
            
            </div>
            <div class="form-group" runat="server" visible="false" id="AdminOptions">
                <div class="col-md-4 col-xs-6">
                    <asp:Label runat="server"> Mentor Group:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-6">
                    <asp:DropDownList runat="server" ID="Groups_DropDown" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-12 col-xs-12" style="margin-top:5%">
                    <asp:Button runat="server" ID="Button1" Text="Submit" OnClick="SubmitButton_Click" CssClass="btn btn-default"/>
                </div>
                <div class="col-md-8 col-xs-12">
                    <asp:Label id="Status_Label" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
