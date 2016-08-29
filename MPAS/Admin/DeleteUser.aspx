<%@ Page Title="Delete User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeleteUser.aspx.cs" Inherits="MPAS.Admin.DeleteUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3> Delete User</h3>
    <hr />
    <div class ="row form-horizontal">
        <div class="container col-md-8 col-xs-12">
            <div class="form-group">
                <div class="col-md-4 col-xs-6">
                    <asp:Label runat="server" ID="StudentNumberDelete_Label" >Student Number:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-6">
                    <asp:TextBox runat="server" ID="StudentNumberDelete_TextBox" Placeholder="XYZABC000" style="text-transform:uppercase" CssClass="form-control"/>
                    <asp:CustomValidator runat="server" ControlToValidate="StudentNumberDelete_TextBox"
                                    CssClass="text-danger" ErrorMessage="Student Number format invalid" 
                                    OnServerValidate="StudentNumberValidate"/>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4 col-xs-6">
                    <asp:Button runat="server" ID="DeleteButton" Text="Delete" OnClick="DeleteButton_Click" CssClass="btn btn-default" style="margin-top:5%"/>
                </div>
                <div class="col-md-8 col-xs-6">
                    <asp:Label runat="server" ID="StatusLabel" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
