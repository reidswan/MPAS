<%@ Page Title="Delete User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeleteUser.aspx.cs" Inherits="MPAS.Admin.DeleteUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1> Delete User</h1>
    <hr />
    
    <div class ="row form-horizontal">
        <div class="container col-md-12 col-xs-12">
            <div class="form-group">
                <div class="col-xs-4 col-md-4 ">
                    <asp:Label runat="server" ID="StudentNumberDelete_Label" >Student Number:</asp:Label>
                </div>
                <div class="col-xs-8 col-md-8 ">
                    <asp:TextBox runat="server" ID="StudentNumberDelete_TextBox" Placeholder="XYZABC000" style="text-transform:uppercase" CssClass="form-control"/>
                    <asp:CustomValidator runat="server" ControlToValidate="StudentNumberDelete_TextBox"
                                    CssClass="text-danger" ErrorMessage="Student Number format invalid" 
                                    OnServerValidate="StudentNumberValidate"/>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Button runat="server" ID="DeleteButton" Text="Delete" OnClick="DeleteButton_Click" CssClass="btn btn-default" style="margin-top:5%"/>
                </div>
                <div class="col-md- col-xs-8">
                    <asp:Label runat="server" ID="StatusLabel" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <asp:GridView ID="GridView1" runat ="server" DataKeyNames="Studentnumber" AutoGenerateColumns="False" 
        CssClass="table"
        >
        
        <RowStyle  HorizontalAlign="Center" />

            <Columns>
                <asp:BoundField HeaderText=" Student Number " DataField="StudentNumber"></asp:BoundField>
                <asp:BoundField HeaderText=" First Name " DataField="FirstName"></asp:BoundField>
                <asp:BoundField HeaderText=" Surname " DataField="Surname"></asp:BoundField>
                <asp:BoundField HeaderText=" Date Of Birth " DataField="DateOfBirth"></asp:BoundField>
                <asp:BoundField HeaderText=" Role " DataField="Role"></asp:BoundField>
                <asp:BoundField HeaderText=" Group Number " DataField="GroupNumber"></asp:BoundField>
            </Columns>
   </asp:GridView>
    <br />
    <br />
  
</asp:Content>
