<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Feedback.aspx.cs" Inherits="MPAS.Feedback" %>
<asp:Content ID="FeedbackContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Feedback</h1>
    
    <div class="row form-horizontal">
        <div class="container col-md-8">
            <div class="form-group" style="padding-bottom:20px; padding-top:20px">
                <div class="col-xs-4">
                    <asp:LinkButton runat="server" ID="MentorFeedbackButton" visible="false" OnClick="MentorButton_Click" CSSclass="regular" style="margin-top:5%">
                        Add New Mentor Feedback Form
                    </asp:LinkButton>
                </div>
                <div class="col-xs-4">
                    <asp:LinkButton runat="server" ID="MenteeFeedbackButton" visible="false" OnClick="MenteeButton_Click" CSSclass="regular" style="margin-top:5%; padding-bottom:150px; padding-top:150px">
                        Add New Mentee Feedback Form
                    </asp:LinkButton>
                </div>
            </div>
            <div class="form-group">
                <div class="col-xs-3" style="padding-top:7px">
                    <asp:Label runat="server" ID="ViewLabel" visible="false">View Feedback:</asp:Label>
                </div>
                <div class="col-xs-5" style="padding-top:7px">
                    <asp:DropDownList id="FeedbackList" Visible="false" AutoPostBack="True" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="col-xs-4">
                    <asp:Button runat="server" ID="View_Button" Text="View" OnClick="ViewButton_Click" CssClass="btn btn-default" Visible="false"/>
                </div>
            </div>
        </div>
    </div>
    <br />
    <img src="Images/Scale.png" style="height:75px; width:500px"/>
    <asp:Table ID="FeedbackTable" runat="server" Visible="false" EnableTheming="true" CssClass="table" style="width:100%">
        <asp:TableRow ID="R1" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q1"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="RBR1" runat="server" Visible="false">
            <asp:TableCell Width="400px">
                <asp:RadioButtonList ID="RB1" runat="server" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="50" RepeatLayout="table" OnSelectedIndexChanged=Index_Changed AutoPostBack="true" Width="400px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R2" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q2"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="RBR2" runat="server" Visible="false">
            <asp:TableCell Width="400px">
                <asp:RadioButtonList ID="RB2" runat="server" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="50" RepeatLayout="table" OnSelectedIndexChanged=Index_Changed AutoPostBack="true" Width="400px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R3" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q3"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="RBR3" runat="server" Visible="false">
            <asp:TableCell Width="400px">
                <asp:RadioButtonList ID="RB3" runat="server" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="50" RepeatLayout="table" OnSelectedIndexChanged=Index_Changed AutoPostBack="true" Width="400px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R4" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q4"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="RBR4" runat="server" Visible="false">
            <asp:TableCell Width="400px">
                <asp:RadioButtonList ID="RB4" runat="server" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="50" RepeatLayout="table" OnSelectedIndexChanged=Index_Changed AutoPostBack="true" Width="400px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R5" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q5"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="RBR5" runat="server" Visible="false">
            <asp:TableCell Width="400px">
                <asp:RadioButtonList ID="RB5" runat="server" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="50" RepeatLayout="table" OnSelectedIndexChanged=Index_Changed AutoPostBack="true" Width="400px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R6" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q6"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="RBR6" runat="server" Visible="false">
            <asp:TableCell Width="400px">
                <asp:RadioButtonList ID="RB6" runat="server" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="50" RepeatLayout="table" OnSelectedIndexChanged=Index_Changed AutoPostBack="true" Width="400px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R7" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q7"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="RBR7" runat="server" Visible="false">
            <asp:TableCell Width="400px">
                <asp:RadioButtonList ID="RB7" runat="server" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="50" RepeatLayout="table" OnSelectedIndexChanged=Index_Changed AutoPostBack="true" Width="400px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R8" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q8"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="RBR8" runat="server" Visible="false">
            <asp:TableCell Width="400px">
                <asp:RadioButtonList ID="RB8" runat="server" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="50" RepeatLayout="table" OnSelectedIndexChanged=Index_Changed AutoPostBack="true" Width="400px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R9" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q9"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="RBR9" runat="server" Visible="false">
            <asp:TableCell Width="400px">
                <asp:RadioButtonList ID="RB9" runat="server" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="50" RepeatLayout="table" OnSelectedIndexChanged=Index_Changed AutoPostBack="true" Width="400px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="R10" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label runat="server" ID="Q10"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="RBR10" runat="server" Visible="false">
            <asp:TableCell Width="400px">
                <asp:RadioButtonList ID="RB10" runat="server" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="50" RepeatLayout="table" OnSelectedIndexChanged=Index_Changed AutoPostBack="true" Width="400px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <div class="form-group" style="padding-bottom:20px; padding-top:20px">
        <div class="col-xs-2">
            <asp:Label runat="server" ID="CommentLabel" visible="false">Additional Comments:</asp:Label>
        </div>
        <div class="col-xs-6">
            <asp:TextBox placeholder="Eg. It was difficult to find/book appropriate venues for mentee meetings" runat="server" ID="Comment_Textbox" TextMode="MultiLine" CssClass="form-control" Visible="false"></asp:TextBox>
        </div>
    </div>


    <br />
    <div class="container col-md-8">
        <div class="form-group">
            <div class="col-md-12 col-xs-12" style="margin-top:5%">
                <asp:Button runat="server" ID="SubmitButton" Text="Submit" OnClick="SubmitButton_Click" CssClass="btn btn-default" Visible="false" Enabled="false"/>
            </div>
            <div class="col-md-8 col-xs-12">
                <asp:Label id="Status_Label" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
