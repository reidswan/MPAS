<%@ Page Title="Group Chat" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GroupChat.aspx.cs" Inherits="MPAS.Chatroom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Group Chatroom</h3>
    <div class="row">
        <div class="col-md-2" />
        <div class="col-md-8 col-xs-12">
            <asp:UpdatePanel runat="server" ID="ChatroomPanel">
                <ContentTemplate>
                    <div class="row" style="border:solid 2px black; overflow-x:hidden; overflow-y:auto">
                        <asp:Label runat="server" ID="ChatroomLabel" Text=""/>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-2" />
    </div>

    <div class="row">
        <div class="col-md-2" />
        <div class="col-md-8 col-xs-12">
            <asp:UpdatePanel runat="server" ID="MessagePanel">
                <ContentTemplate>
                    <div class="row">
                        <asp:TextBox class="form-control" runat="server" TextMode="MultiLine" ID="MessageBox" />
                    </div>
                    <div class="row">
                        <asp:Button CssClass="button" runat="server" ID="SendButton" OnClick="SendButtonClick" Text="Send" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-2" />

    </div>
</asp:Content>
