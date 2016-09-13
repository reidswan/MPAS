<%@ Page Title="Authorisation Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuthError.aspx.cs" Inherits="MPAS.Error.AuthError" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Authorisation Error</h3>
    <hr/>
    <div class="row">
        <div class="col-md-8 col-xs-12">
            You're seeing this page because you attempted to access <asp:Label runat="server" ID="From_Label">page</asp:Label>
            but the server was unable to validate you. Please ensure you are logged in to the correct account.
        </div>
    </div>
</asp:Content>
