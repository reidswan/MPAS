<%@ Page Title="Group Chat" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GroupChat.aspx.cs" Inherits="MPAS.GroupChat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Group Chatroom</h1>
    <hr />
    <div class="row">
        <div class="col-md-2"></div>
        <div class="col-md-8 col-xs-12">
            <asp:UpdatePanel runat="server" ID="ChatroomPanel" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ChatTimer"/>
                </Triggers>
                <ContentTemplate>
                    <div class="row" style="border:solid 2px black; overflow-x:hidden; overflow-y:auto; min-height:100px; max-height:500px">
                        <asp:Label runat="server" ID="ChatroomLabel" Text="" ></asp:Label>
                        <asp:Timer runat="server" ID="ChatTimer" Interval="100" OnTick="ChatTimer_Tick"/>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-2"></div>
    </div>

    <div class="row">
        <div class="col-md-2"></div>
        <div class="col-md-8 col-xs-12">
            <asp:UpdatePanel runat="server" ID="MessagePanel" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="SendButton" />
                </Triggers>
                <ContentTemplate>
                    <div class="row">
                        <asp:TextBox class="form-control" runat="server" TextMode="MultiLine" ID="MessageBox" />
                    </div>
                    <div class="row">
                        <asp:Button CssClass="button" runat="server" ID="SendButton" Text="Send" OnClick="SendButtonClick"/>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-2"></div>
    </div>
</asp:Content>
