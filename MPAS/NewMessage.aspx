<%@ Page Title="New Message" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewMessage.aspx.cs" Inherits="MPAS.NewMessage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>New Message</h3>
    <div class="row form-horizontal">
        <div class="container col-md-8 col-xs-12">
            <asp:UpdatePanel runat="server" ID="Search_Panel" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Search_Button"/>
                </Triggers>
                <ContentTemplate>
                    <div class="form-group">
                        <div class="col-md-4 col-xs-6">
                            <asp:Label runat="server" ID="Search_Label">Search for recipient</asp:Label>
                        </div>
                        <div class="col-md-8 col-xs-6">
                            <asp:TextBox runat="server" ID="Search_TextBox" CssClass="form-control"></asp:TextBox>
                            <asp:Button runat="server" ID="Search_Button" OnClick="Search_Button_Click" Text="Search" />
                            <asp:Table runat="server" ID="SearchResult_Table" Visible="true"></asp:Table>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-4 col-xs-6">
                            <asp:Label runat="server" ID="Recipient_Label" >Recipient:</asp:Label>
                        </div>
                        <div class="col-md-8 col-xs-6">
                            <asp:TextBox runat="server" ID="Recipient_TextBox" placeholder="XYZABC000" CssClass="form-control"></asp:TextBox>
                        </div>
                        
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="form-group">
                <div class="col-md-4 col-xs-6">
                    <asp:Label runat="server" ID="Content_Label">Content:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-6">
                    <asp:TextBox runat="server" ID="Content_TextBox" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-12 col-xs-12" style="margin-top:5%">
                    <asp:Button runat="server" ID="SendButton" Text="Send" OnClick="SendButton_Clicked" CssClass="btn btn-default"/>
                </div>
                <div class="col-md-8 col-xs-12">
                    <asp:Label id="Status_Label" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
