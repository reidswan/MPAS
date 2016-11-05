<%@ Page Title="Group Assignment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GroupAssignment.aspx.cs" Inherits="MPAS.Admin.GroupAssignment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1> Assign to Groups </h1>
    <hr />
    <div class="row">
        <div class="col-md-8 col-xs-12">
            <p>
                Assigns all currently registered mentors and mentees to groups based on their schedules.
            </p>
            <br />
            <p style="color:red">
                Note: This will override any previous group assignments. Please ensure all mentors and mentees
                have been registered and schedules are correct
            </p>
        </div>
    </div>
    <div class="row">
        <div class="col-md-8 col-xs-12">
            <asp:UpdatePanel runat="server" ID="AssignmentPanel">
                <%--<Triggers>
                    <asp:AsyncPostBackTrigger ControlID="AssignButton" />
                    <asp:AsyncPostBackTrigger ControlID="GenerateButton" />
                </Triggers> --%>
                <ContentTemplate>
                    <asp:Button CssClass="btn btn-default" runat="server" ID="AssignButton" OnClick="AssignButton_Click" Text="Assign" />
                    <asp:Button CssClass="btn btn-default" runat="server" ID="GenerateButton" OnClick="GenerateButton_Click" Text="Generate Random Users" />
                    <br />
                    <asp:Label runat="server" ID="AssignmentLabel"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
