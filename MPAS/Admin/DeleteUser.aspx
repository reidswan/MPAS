<%@ Page Title="Delete User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeleteUser.aspx.cs" Inherits="MPAS.Admin.DeleteUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3> Delete User</h3>
    <hr />
    <div class ="container">
        <div class="row">
            <div class="col-md-4 col-xs-4">
                <asp:Label runat="server" ID="StudentNumberDelete_Label">Student Number:</asp:Label>
            </div><div class="col-md-4 col-xs-4">
                <asp:TextBox runat="server" ID="StudentNumberDelete_TextBox" Placeholder="XYZABC000" style="text-transform:uppercase"/>
                <asp:CustomValidator runat="server" ControlToValidate="StudentNumberDelete_TextBox"
                                CssClass="text-danger" ErrorMessage="Student Number format invalid" 
                                OnServerValidate="StudentNumberValidate"/>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4 col-xs-4">
                <asp:Button runat="server" ID="DeleteButton" Text="Delete" OnClick="DeleteButton_Click"/>
            </div>
            <div class="col-md-4 col-xs-4">
                <asp:Label runat="server" ID="StatusLabel" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
