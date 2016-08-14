<%@ Page Title="Add User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="MPAS.Admin.AddUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3> Add User </h3>
    <asp:Label runat="server" ID="Warning"></asp:Label>
    <div class="container">
        <div class="row">
            <div class="col-md-4 col-xs-4">
                <asp:Label runat="server" ID="FirstName_Label" >First Name:</asp:Label>
            </div>
            <div class="col-md-4 col-xs-4">
                <asp:TextBox runat="server" ID="FirstName_TextBox" placeholder="John"></asp:TextBox>
            </div>
            <asp:CustomValidator runat="server" ControlToValidate="FirstName_TextBox"
                                CssClass="text-danger" ErrorMessage="First name must be alphabetic and non-empty" 
                                OnServerValidate="NameValidate"/>
        </div>
        
        <div class="row">
            <div class="col-md-4 col-xs-4">
                <asp:Label runat="server" ID="Surname_Label">Surname:</asp:Label>
            </div>
            <div class="col-md-4 col-xs-4">
                <asp:TextBox runat="server" ID="Surname_TextBox" placeholder="Smith"></asp:TextBox>
            </div>
            <asp:CustomValidator runat="server" ControlToValidate="Surname_TextBox"
                                CssClass="text-danger" ErrorMessage="Surname must be alphabetic and non-empty" 
                                OnServerValidate="NameValidate"/>
        </div>

        <div class="row">
            <div class="col-md-4 col-xs-4">
                <asp:Label runat="server" ID="StudentNumber_Label">Student Number:</asp:Label>
            </div>
            <div class="col-md-4 col-xs-4">
                <asp:TextBox runat="server" ID="StudentNumber_TextBox" placeholder="XYZABC000" style="text-transform:uppercase"></asp:TextBox>
                <asp:CustomValidator runat="server" ControlToValidate="StudentNumber_TextBox"
                                CssClass="text-danger" ErrorMessage="Student Number format invalid" 
                                OnServerValidate="StudentNumberValidate"/>
            </div>
            
        </div>
        <div class="row">
            <div class="col-md-4 col-xs-4">
                <asp:Label runat="server" Id="Mentor_Label">Mentor: </asp:Label>
            </div>
            <div class="col-md-4 col-xs-4">
                <asp:CheckBox runat="server" Checked="false" Id="MentorCheckBox"/>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-4">
                <asp:Button runat="server" ID="SubmitButton" Text="Submit" OnClick="SubmitButton_Clicked"/>
            </div>
            <div class="col-md-4">
                <asp:Label id="Status_Label" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
