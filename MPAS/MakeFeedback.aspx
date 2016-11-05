<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MakeFeedback.aspx.cs" Inherits="MPAS.MakeFeedback" %>
<asp:Content ID="MakeFeedbackContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Add Feedback Form</h1>
    <hr />

    <div class="container">
        <div class="row form-horizontal">
            <div class="container col-md-12">
                <div class="form-group">
                    <div class="col-xs-1">
                        <asp:Label runat="server" ID="Title_Label"><h4>Title:</h4></asp:Label>
                    </div>
                    <div class="col-xs-3">
                        <asp:TextBox placeholder="Eg. Mentor Feedback - Term 1" runat="server" ID="Title_Textbox" TextMode="SingleLine" CssClass="form-control" Width="150%"></asp:TextBox>
                    </div>
                    <div class="col-xs-8">
                        <asp:Button runat="server" ID="Confirm_Button" Text="Confirm" OnClick="ConfirmButton_Click" CssClass="btn btn-default" />
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="container col-xs-12">
            <asp:UpdatePanel runat="server" ID="QuestionPanel" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Add_Button" />
                </Triggers>
                <ContentTemplate>
                    <asp:Table ID="QuestionTable" runat="server" EnableTheming="true" CssClass="table" Style="width: 100%">
                        <asp:TableRow ID="R1" runat="server" Visible="false">
                            <asp:TableCell Width="30px">
                                <asp:LinkButton style="padding-bottom:12px" runat="server" ID="D1" OnClick="DeleteButton_Click" Class="hvr-icon-grow fa-2x"></asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="Q1"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="R2" runat="server" Visible="false">
                            <asp:TableCell>
                                <asp:LinkButton style="padding-bottom:12px" runat="server" ID="D2" OnClick="DeleteButton_Click" Class="hvr-icon-grow fa-2x"></asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="Q2"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="R3" runat="server" Visible="false">
                            <asp:TableCell>
                                <asp:LinkButton style="padding-bottom:12px" runat="server" ID="D3" OnClick="DeleteButton_Click" Class="hvr-icon-grow fa-2x"></asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="Q3"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="R4" runat="server" Visible="false">
                            <asp:TableCell>
                                <asp:LinkButton style="padding-bottom:12px" runat="server" ID="D4" OnClick="DeleteButton_Click" Class="hvr-icon-grow fa-2x"></asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="Q4"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="R5" runat="server" Visible="false">
                            <asp:TableCell>
                                <asp:LinkButton style="padding-bottom:12px" runat="server" ID="D5" OnClick="DeleteButton_Click" Class="hvr-icon-grow fa-2x"></asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="Q5"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="R6" runat="server" Visible="false">
                            <asp:TableCell>
                                <asp:LinkButton style="padding-bottom:12px" runat="server" ID="D6" OnClick="DeleteButton_Click" Class="hvr-icon-grow fa-2x"></asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="Q6"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="R7" runat="server" Visible="false">
                            <asp:TableCell>
                                <asp:LinkButton style="padding-bottom:12px" runat="server" ID="D7" OnClick="DeleteButton_Click" Class="hvr-icon-grow fa-2x"></asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="Q7"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="R8" runat="server" Visible="false">
                            <asp:TableCell>
                                <asp:LinkButton style="padding-bottom:12px" runat="server" ID="D8" OnClick="DeleteButton_Click" Class="hvr-icon-grow fa-2x"></asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="Q8"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="R9" runat="server" Visible="false">
                            <asp:TableCell>
                                <asp:LinkButton style="padding-bottom:12px" runat="server" ID="D9" OnClick="DeleteButton_Click" Class="hvr-icon-grow fa-2x"></asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="Q9"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="R10" runat="server" Visible="false">
                            <asp:TableCell>
                                <asp:LinkButton style="padding-bottom:12px" runat="server" ID="D10" OnClick="DeleteButton_Click" Class="hvr-icon-grow fa-2x"></asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="Q10"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <div class="row form-horizontal">
                        <div class="container col-md-12">
                            <div class="form-group">
                                <div class="col-xs-1" style="padding-top: 15px">
                                    <asp:Label runat="server" ID="Question_Label" Visible="false">Question:</asp:Label>
                                </div>
                                <div class="col-xs-8">
                                    <asp:TextBox placeholder="Eg. Have you found the Mentor Programme helpful?" runat="server" ID="Question_Textbox" TextMode="MultiLine" CssClass="form-control" Visible="false"></asp:TextBox>
                                </div>
                                <div class="col-xs-3" style="padding-top: 10px">
                                    <asp:Button runat="server" ID="Add_Button" Text="Add" OnClick="AddButton_Click" CssClass="btn btn-default" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-8 col-xs-12">
                        <asp:Label ID="Status_Label" runat="server" Visible="false"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
        <div class="form-group">
            <div class="col-xs-3" style="padding-top: 15px; float: right">
                <asp:Button runat="server" ID="Done_Button" Text="Done" OnClick="DoneButton_Click" CssClass="btn btn-default" Visible="false" Height="60px" Width="120px" Font-Size="X-Large" />
            </div>
        </div>
    </div>
</asp:Content>
