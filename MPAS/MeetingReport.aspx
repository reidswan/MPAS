<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeetingReport.aspx.cs" Inherits="MPAS.MeetingReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Meetings Report</h1>
    <hr />
    <asp:UpdatePanel runat="server" ID="ChartPanel" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GroupSelect_List" EventName="SelectedIndexChanged"/>
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12">
                    <asp:DropDownList runat="server" ID="GroupSelect_List" ToolTip="Select a group" OnSelectedIndexChanged="GroupSelect_List_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Select a group" Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2"></div>
                <div class="col-xs-12 col-md-10">
                    <asp:Table runat="server" CssClass="table" ID="AttendanceTable">
                    </asp:Table>
                </div>
                <div class="col-md-2"></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
