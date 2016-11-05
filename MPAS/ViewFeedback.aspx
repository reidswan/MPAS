<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewFeedback.aspx.cs" Inherits="MPAS.ViewFeedback" %>
<asp:Content ID="ViewFeedbackContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" ID="TitleLabel"></asp:Label>
    <img src="Images/Scale.png" style="height:75px; width:500px"/>
    
    <asp:Table ID="FeedbackTable" runat="server" EnableTheming="true" CssClass="table" Style="width: 100%">
        <asp:TableHeaderRow>
            <asp:TableCell><h3>Question</h3></asp:TableCell>
            <asp:TableCell><h3>Average Answer</h3></asp:TableCell>
        </asp:TableHeaderRow>
        <asp:TableRow ID="R1" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q1"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" ID="A1"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R2" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q2"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" ID="A2"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R3" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q3"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" ID="A3"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R4" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q4"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" ID="A4"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R5" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q5"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" ID="A5"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R6" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q6"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" ID="A6"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R7" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q7"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" ID="A7"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R8" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q8"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" ID="A8"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R9" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q9"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" ID="A9"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R10" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q10"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" ID="A10"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <%--Starting with the comment --%>
    <div class="row form-horizontal">
        <div class="container col-md-8">
            <div class="form-group">
                <div class="col-xs-4" style="padding-top:7px">
                    <asp:Label runat="server" ID="ViewLabel" visible="false">View Additional Comments:</asp:Label>
                </div>
                <div class="col-xs-4" style="padding-top:7px">
                    <asp:DropDownList id="FeedbackList" Visible="false" AutoPostBack="True" runat="server" OnSelectedIndexChanged="FeedbackList_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-xs-3" style="padding-top:7px">
                    <asp:Label runat="server" ID="ViewComment" visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <%--Ending with the comment --%>


    <%--Starting with the DropdownLists --%>
    <hr />
    <asp:Table runat="server" ID="DisplaySortingTable">
        <asp:TableRow>
            <asp:TableCell>
                <asp:label runat="server">Sort By: </asp:label>
                <asp:DropDownList ID="ddlSortBy" AutoPostBack ="true" runat="server" OnSelectedIndexChanged="ddlSortBy_SelectedIndexChanged">
                    <asp:ListItem Text="Question" Value="AxisLabel"></asp:ListItem>
                    <asp:ListItem Text="Average" Value="Y"></asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell>
                <asp:label runat="server">Sort By: </asp:label>
                <asp:DropDownList ID="ddlSortDirection" AutoPostBack ="true" runat="server" OnSelectedIndexChanged="ddlSortDirection_SelectedIndexChanged">
                    <asp:ListItem Text="Ascending" Value="ASC"></asp:ListItem>
                    <asp:ListItem Text="Descending" Value="DESC"></asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <%--Ending with the DropdownLists --%>
    <%--Starting with the Chart --%>
    <asp:Chart ID="Chart1" runat="server" Width="1000px" Height="400px" Palette="None">
        <Series>
            <asp:Series Name="Series1" XValueMember="Question" YValueMembers="Average" ></asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
                <AxisY Title="Average Value">
            </AxisY>
            <AxisX Title="Questions" IsLabelAutoFit="True">
                <LabelStyle Angle="-90" Interval="1" />
            </AxisX>
            </asp:ChartArea>
        </ChartAreas>
        <Legends>
            <asp:Legend Name="Information" TitleAlignment="Near"></asp:Legend>
        </Legends>
    </asp:Chart>
    <div class="row form-horizontal">
        <div class="container col-md-8 col-xs-12">
            <div class="form-group">
                <div class="col-md-4 col-xs-6">
                        <label><b><u>Additional Information: </u></b></label>
                        <label>Entire FeedBack Form Average Score :</label>
                        <asp:Label runat="server" ID="AverageLabel_Display"></asp:Label>
                    
                    </div>
                </div>
            </div>
        </div>
    <%--ending with the Chart --%>
</asp:Content>


