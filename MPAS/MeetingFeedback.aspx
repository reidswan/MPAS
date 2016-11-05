<%@ Page Title="Meeting Feedback" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeetingFeedback.aspx.cs" Inherits="MPAS.MeetingFeedback" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Meeting Feedback</h1>
    <div class="container">
        <div class="row">
            <div class="col-xs-12 col-md-8">
                <asp:Table runat="server" CssClass="table" ID="AttendanceTable">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell>
                            <h4>Student</h4>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                            <h4 style="text-align:center">Attended</h4>
                        </asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-md-8">
                <h4><asp:Label runat="server">Additional Comments:</asp:Label></h4>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-md-8">
                <asp:TextBox runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12 col-md-8">
                <asp:Button runat="server" ID="SubmitButton" OnClick="SubmitButton_Click" CssClass="btn btn-default" Text="Submit"></asp:Button>
            </div>
        </div>
    </div>
</asp:Content>
