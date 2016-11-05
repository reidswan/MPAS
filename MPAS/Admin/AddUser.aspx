<%@ Page Title="Add User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="MPAS.Admin.AddUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1> Add User </h1>
    <hr />
    <asp:Label runat="server" ID="Warning"></asp:Label>
    <div class="row form-horizontal">
        <div class="col-md-8 col-xs-12">
            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="FirstName_Label" >First Name:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:TextBox runat="server" ID="FirstName_TextBox" placeholder="John" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:CustomValidator runat="server" ControlToValidate="FirstName_TextBox"
                                    CssClass="text-danger" ErrorMessage="First name must be alphabetic and non-empty" 
                                    OnServerValidate="NameValidate"/>
            </div>
        
            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="Surname_Label">Surname:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:TextBox runat="server" ID="Surname_TextBox" placeholder="Smith" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:CustomValidator runat="server" ControlToValidate="Surname_TextBox"
                                    CssClass="text-danger" ErrorMessage="Surname must be alphabetic and non-empty" 
                                    OnServerValidate="NameValidate"/>
            </div>

            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="StudentNumber_Label">Student Number:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:TextBox runat="server" ID="StudentNumber_TextBox" placeholder="XYZABC000" style="text-transform:uppercase" CssClass="form-control"></asp:TextBox>
                    <asp:CustomValidator runat="server" ControlToValidate="StudentNumber_TextBox"
                                    CssClass="text-danger" ErrorMessage="Student Number format invalid" 
                                    OnServerValidate="StudentNumberValidate"/>
                </div>
            
            </div>
            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" Id="Mentor_Label">Mentor: </asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:CheckBox runat="server" Checked="false" Id="MentorCheckBox" CssClass="checkbox"/>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-12 col-xs-12" style="margin-top:5%">
                    <asp:Button runat="server" ID="SubmitButton" Text="Submit" OnClick="SubmitButton_Clicked" CssClass="btn btn-default"/>
                </div>
                <div class="col-md-12 col-xs-12">
                    <asp:Label id="Status_Label" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
