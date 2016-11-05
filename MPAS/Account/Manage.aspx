<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="MPAS.Account.Manage" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>


    
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <meta http-equiv="Cache-Control" content="no-cache">
<meta http-equiv="Pragma" content="no-cache">
<meta http-equiv="Expires" content="0">
    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <h4>Change your account settings</h4>
                <hr />

                <div class="form-group">
                <div class="col-md-8 col-xs-8" >
                   <div runat="server" class="container-popup" ID ="photoOverlay">
                        <div class="popup">
                            <h2>Update Profile Picture</h2>
                            <div runat="server" top="15%">
                            
                                        <%-- Left Button --%>
                                        <asp:FileUpload id="FileUploadControl" runat="server"/>
                                        <asp:Button runat="server" id="UploadButton" text="Upload" onclick="UploadButton_Click" />
                                        <br /><br />
                                        <asp:Label runat="server" id="Overlay_Status_label" text="Upload status: " Width="90%"/>
                                       <%-- Left Button --%>
                                    
                            </div>
                            <div runat="server" class ="close-button-container">
                                <asp:LinkButton runat="server" Class ="hvr-icon-fade close-button " onClick="CloseOverlayForm_Click"></asp:LinkButton>
                            </div>  
                        </div>
                    </div>
                    <div class="col-md-4 col-xs-4" style="left:50%;">
                                
                                <asp:Label runat="server" ID="PicturePath_Label"></asp:Label>
                            <div runat="server" class ="caption" id="container_For_Blue">
					            <div class="blur"></div>
					                <div class="caption-text" style ="top:55px">
					        	        <h1 >Update Photo</h1>
                                        <asp:LinkButton runat="server" ID ="LinkButton1" OnClick="DisplayOverlay_Click">Click Here</asp:LinkButton>
					                 </div>
				            </div>
                    </div>
                </div>
                
                 </div>


                <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="FirstName_Label" >First Name:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:TextBox runat="server" ID="FirstName_TextBox"  CssClass="form-control"></asp:TextBox>
                    <asp:CustomValidator runat="server" ID="UpdateFirstNameValidator" ControlToValidate="FirstName_TextBox" OnServerValidate="NameValidate" CssClass="text-danger" ErrorMessage="First name must be alphabetic, non-empty and non-HTML"></asp:CustomValidator>
                </div>
                
                 </div>
        

                <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="Surname_Label">Surname:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:TextBox runat="server" ID="Surname_TextBox"  CssClass="form-control"></asp:TextBox>
                    <asp:CustomValidator runat="server" ID="UpdateSurnameValidator" ControlToValidate="Surname_TextBox" OnServerValidate="NameValidate" CssClass="text-danger" ErrorMessage="Surname must be alphabetic, non-empty and non-HTML"></asp:CustomValidator>
                </div>
               
            </div>

            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="StudentNumber_Label">Student Number:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:TextBox runat="server" ID="StudentNumber_TextBox"  style="text-transform:uppercase" CssClass="form-control"></asp:TextBox>
                    
                </div>
            
            </div>

            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="DOB_Label">Date Of Birth:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <div>
                        <asp:DropDownList ID="DropDownList1" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DropDownList2" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DropDownList3" runat="server">
                        </asp:DropDownList>
                       
                    </div>
                </div>
            
            </div>



            <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" Id="Mentor_Label">Mentor: </asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:CheckBox runat="server"  Checked="false" Id="MentorCheckBox" CssClass="checkbox"/>
                </div>
            </div>

                <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="GroupNumber_label" >Group Number:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                    <asp:TextBox runat="server" ID="GroupNumber_TextBox"  CssClass="form-control"></asp:TextBox>
                </div>
                
                 </div>

                <%-- Start Changing Password--- --%>
                <div class="form-group">
                <div class="col-md-4 col-xs-4">
                    <asp:Label runat="server" ID="Password_Label" >Password:</asp:Label>
                </div>
                <div class="col-md-8 col-xs-8">
                     <asp:HyperLink NavigateUrl="/Account/ManagePassword"  Text="[Change]" Visible="false" ID="ChangePassword" runat="server" />
                </div>
                
                 </div>

                <%-- End Changing Password--- --%>

                 <div class="form-group">
                <div class="col-md-12 col-xs-12" style="margin-top:5%">
                    <asp:Button runat="server" ID="SubmitButton" Class="testingClass" Text="Submit" OnClick="SubmitButton_Clicked" />
                </div>
                <div class="col-md-12 col-xs-12">
                    <asp:Label id="Main_Status_Label" runat="server" Visible="false"></asp:Label>
                </div>
            </div>


                
            </div>
        </div>
    </div>

</asp:Content>


