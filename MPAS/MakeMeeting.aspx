<%@ Page Title="Make Meeting" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MakeMeeting.aspx.cs" Inherits="MPAS.MakeMeeting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Make Meeting</h1>
    <hr />

    <div class="row form-horizontal">
        <div class="container col-md-8">
            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="Title_Label" >Title:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:TextBox placeholder="Eg. First Meeting" runat="server" ID="Title_Textbox" TextMode="SingleLine" CssClass="form-control"></asp:TextBox>
                    <asp:CustomValidator runat="server" ID="ContentValidator" ControlToValidate="Title_TextBox" OnServerValidate="Validate" CssClass="text-danger" ErrorMessage="Title cannot contain HTML"></asp:CustomValidator>

                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="Label5" >Location:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:TextBox placeholder="Eg. Jameson Hall" runat="server" ID="Location_Textbox" TextMode="SingleLine" CssClass="form-control"></asp:TextBox>
                    <asp:CustomValidator runat="server" ID="CustomValidator1" ControlToValidate="Location_TextBox" OnServerValidate="Validate" CssClass="text-danger" ErrorMessage="Title cannot contain HTML"></asp:CustomValidator>

                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="Label1">Agenda:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:TextBox placeholder="Eg. Introductions" runat="server" ID="Agenda_Textbox" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    <asp:CustomValidator runat="server" ID="CustomValidator2" ControlToValidate="Agenda_TextBox" OnServerValidate="Validate" CssClass="text-danger" ErrorMessage="Title cannot contain HTML"></asp:CustomValidator>

                </div>
            </div>

            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="Label2">Date:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:Calendar runat="server" ID="DatePicker" OnSelectionChanged="DateSelectionChanged"></asp:Calendar>
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="Label3">Start Time:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:DropDownList runat="server" ID="StartHour"></asp:DropDownList>
                    <asp:DropDownList runat="server" ID="StartMinute"></asp:DropDownList>
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="Label4">End Time:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:DropDownList runat="server" ID="EndHour"></asp:DropDownList>
                    <asp:DropDownList runat="server" ID="EndMinute"></asp:DropDownList>
                </div>
            </div>

            <div class="form-group" runat="server" visible="false" id="AdminOptions">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server"> Mentor Group:</asp:Label>
                </div>
                <div class="col-md-2 col-xs-2">
                    <asp:DropDownList runat="server" ID="Groups_DropDown" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-6 col-xs-6">

                </div>
            </div>
            <div class="form-group">
                <div class="col-md-12 col-xs-12" style="margin-top:5%">
                    <asp:Button runat="server" ID="SubmitButton" Text="Submit" OnClick="SubmitButton_Click" CssClass="btn btn-default"/>
                </div>
                <div class="col-md-12 col-xs-12">
                    <asp:Label id="Status_Label" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
