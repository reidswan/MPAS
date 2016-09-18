<%@ Page Title="Group Assignment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GroupAssignment.aspx.cs" Inherits="MPAS.Admin.GroupAssignment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3> Add User </h3>
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
            <asp:UpdatePanel runat="server" ID="AssignmentPanel" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="AssignButton" />
                </Triggers>
                <ContentTemplate>
                    <asp:Button CssClass="btn btn-default" runat="server" ID="AssignButton" OnClick="AssignButton_Click" Text="Assign" />
                    <br />
                    <asp:Label runat="server" ID="AssignmentLabel" Visible="false">Assigning users to groups. Page will redirect on completion</asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
