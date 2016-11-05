using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using MPAS.Models;
using System.Data.SqlClient;
using MPAS.Logic;
using System.IO;

using System.Web.Services;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace MPAS.Account
{
    public partial class Manage : System.Web.UI.Page
    {

        DateTime FullDOB;
        string currentUser;
        
        protected string SuccessMessage
        {
            get;
            private set;
        }

        private bool HasPassword(ApplicationUserManager manager)
        {
            return manager.HasPassword(User.Identity.GetUserId());
        }

        public bool HasPhoneNumber { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public bool TwoFactorBrowserRemembered { get; private set; }

        public int LoginsCount { get; set; }

        protected void CloseOverlayForm_Click(object sender, EventArgs e)
        {
            photoOverlay.Visible = false;
            Response.Redirect("Manage.aspx", false);
        }

       
        protected void DisplayOverlay_Click(object sender, EventArgs e)
        {
            photoOverlay.Visible = true;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (!IsPostBack)
            {
                //Stuff to do to everyone
                //Start Calendar
                GetDaysListed(DropDownList1);
                GetMonthsListed(DropDownList2);
                GetYearsListed(DropDownList3);

                //End Calendar
                StudentNumber_TextBox.Enabled = false;
                photoOverlay.Visible = false;
               
                
                currentUser = this.User.Identity.Name;
                User u = DatabaseUtilities.GetUser(currentUser);
                FirstName_TextBox.Text = u.FirstName;
                Surname_TextBox.Text = u.Surname;
                StudentNumber_TextBox.Text = u.StudentNumber;
                DropDownList3.SelectedValue = u.DateOfBirth.Year.ToString();
                DropDownList2.SelectedValue = u.DateOfBirth.Month.ToString();
                DropDownList1.SelectedValue = u.DateOfBirth.Day.ToString();
                GroupNumber_TextBox.Text = "" + u.GroupNumber;
                GroupNumber_TextBox.Enabled = false;
                
                if (u is Mentor)
                    MentorCheckBox.Checked = true;
                else
                    MentorCheckBox.Checked = false;
                MentorCheckBox.Enabled = false;

                //if Admin then 
                if (currentUser == "01360406")
                {
                    HideNotNeededInfo();
                    string imagePath = "01360406.jpg";
                    PicturePath_Label.Text = @"<ul class = 'caption-style-1' ><li><img src ='/Images/" + imagePath + @"' alt ='Missing Image' style = 'width: 200px; height: 200px; </li></ul>'>";

                }
                //
                //Taking all the information from the table where the student number matches the one that is logged in
                else//its a user, not admin
                {
                    ChangePassword.Visible = true;
                    Password_Label.Visible = true;
                    string imagePath = u.PathToPicture;
                    PicturePath_Label.Text = @"<ul class = 'caption-style-1' ><li><img src ='/Images/" + imagePath + @"' alt ='Missing Image' style = 'width: 200px; height: 200px; </li></ul>'>";
                     
                }

               
                
                //--------------------


                //Start Code for taking image with webcam
                
                //End Code for taking image with webcam
            }

            

        /*HasPhoneNumber = String.IsNullOrEmpty(manager.GetPhoneNumber(User.Identity.GetUserId()));

            // Enable this after setting up two-factor authentientication
            //PhoneNumber.Text = manager.GetPhoneNumber(User.Identity.GetUserId()) ?? String.Empty;

            TwoFactorEnabled = manager.GetTwoFactorEnabled(User.Identity.GetUserId());

            LoginsCount = manager.GetLogins(User.Identity.GetUserId()).Count;

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            if (!IsPostBack)
            {
                // Determine the sections to render
                if (HasPassword(manager))
                {
                    ChangePassword.Visible = true;
                }
                else
                {
                    ChangePassword.Visible = false;
                }

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "Your password has been changed."
                        : message == "SetPwdSuccess" ? "Your password has been set."
                        : message == "RemoveLoginSuccess" ? "The account was removed."
                        : message == "AddPhoneNumberSuccess" ? "Phone number has been added"
                        : message == "RemovePhoneNumberSuccess" ? "Phone number was removed"
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }
            }*/
        }
       
        protected void HideNotNeededInfo()
        {
            FirstName_TextBox.Enabled = false;
            Surname_TextBox.Enabled = false;
            Password_Label.Visible = false;
            SubmitButton.Visible = false;
            DOB_Label.Visible = false;
            DropDownList1.Visible = false;
            DropDownList2.Visible = false;
            DropDownList3.Visible = false;
            Mentor_Label.Visible = false;
            MentorCheckBox.Visible = false;
            GroupNumber_label.Visible = false;
            GroupNumber_TextBox.Visible = false;
            container_For_Blue.Visible = false;
        }
        protected void DiplayOpenFile_Click(object sender, EventArgs e)
        {
            
        }
        
        protected void SubmitButton_Clicked(object sender, EventArgs e)
        {
            if (this.IsValid)
                UpdateUser();
        }

        public void UpdateUser()
        {
           
            CombineDOB();
            if (DatabaseUtilities.UpdateUser(StudentNumber_TextBox.Text, FirstName_TextBox.Text, Surname_TextBox.Text, FullDOB))
            {

                // indicate that the account was created successfully
                Main_Status_Label.Text = FirstName_TextBox.Text + " " + Surname_TextBox.Text +"," + " was updated successfully";
                Main_Status_Label.ForeColor = System.Drawing.Color.Green;
                Main_Status_Label.Visible = true;
                
            }
            else
            {
                // indicate that account creation failed
                Main_Status_Label.Text = "Failed to update user";
                Main_Status_Label.ForeColor = System.Drawing.Color.Red;
                Main_Status_Label.Visible = true;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // Remove phonenumber from user
        protected void RemovePhone_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var result = manager.SetPhoneNumber(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return;
            }
            var user = manager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                Response.Redirect("/Account/Manage?m=RemovePhoneNumberSuccess");
            }
        }

        // DisableTwoFactorAuthentication
        protected void TwoFactorDisable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), false);

            Response.Redirect("/Account/Manage");
        }

        //EnableTwoFactorAuthentication 
        protected void TwoFactorEnable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), true);

            Response.Redirect("/Account/Manage");
        }



        protected void GetDaysListed(DropDownList DDDayList)
        {
            for (int i = 1; i < 32; i++)
            {
                ListItem list = new ListItem();
                list.Text = i.ToString();
                list.Value = i.ToString();
                DDDayList.Items.Add(list);
            }
            ListItem mlist = new ListItem();
           // mlist.Text = "[Day]";
           // mlist.Value = "[Day]";
           // mlist.Selected = true;
            
            DDDayList.Items.Add(mlist);


        }


        protected void GetMonthsListed(DropDownList DDMonthList)
        {
            DateTime month = Convert.ToDateTime("1/1/2007");
            for (int i = 0; i < 12; i++)
            {
                
                DateTime NextMont = month.AddMonths(i);
                ListItem list = new ListItem();
                list.Text = NextMont.ToString("MMMM");
                list.Value = NextMont.Month.ToString();
                DDMonthList.Items.Add(list);
            }


            ListItem mlist = new ListItem();
           // mlist.Text = "[Month]";
           // mlist.Value = "[Month]";
           // mlist.Selected = true;
            DDMonthList.Items.Add(mlist);
        }


        protected void GetYearsListed(DropDownList DDYearList)
        {
            int yearLast = DateTime.Now.Year - 10;
            int yearThen = yearLast - 90;
            for (int i = yearThen; i < yearLast; i++)
            {
                ListItem list = new ListItem();
                list.Text = yearThen.ToString();
                list.Value = yearThen.ToString();
                DDYearList.Items.Add(list);
                yearThen += 1;
            }


            ListItem mlist = new ListItem();
           // mlist.Text = "[Year]";
           // mlist.Value = "[Year]";
           // mlist.Selected = true;
            DDYearList.Items.Add(mlist);
        }
        
        protected void UploadButton_Click(object send, EventArgs e)
        {
            
            if (FileUploadControl.HasFile)
            {
                try
                {
                    if (FileUploadControl.PostedFile.ContentType == "image/jpeg")
                    {
                        if (FileUploadControl.PostedFile.ContentLength < 50000000)
                        {
                            //Delete the original picture
                            currentUser = this.User.Identity.Name;
                            User u = DatabaseUtilities.GetUser(currentUser);
                            if (u.PathToPicture != "default.jpg") File.Delete(Server.MapPath("~/Images/") + u.PathToPicture);
                            
                            string filename = Path.GetFileName(FileUploadControl.FileName);
                            string newFileName = this.User.Identity.Name + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                            FileUploadControl.SaveAs(Server.MapPath("~/Images/") + this.User.Identity.Name+"-"+DateTime.Now.ToString("yyyyMMddHHmmss") +".jpg");
                            CombineDOB();
                            if (DatabaseUtilities.UpdatePicture(StudentNumber_TextBox.Text, newFileName))
                            {

                                // indicate that the account was created successfully
                                Overlay_Status_label.Text = "Upload status: File uploaded!";
                                Overlay_Status_label.ForeColor = System.Drawing.Color.Green;
                                Overlay_Status_label.Visible = true;
                                
                                

                            }
                            else
                            {
                                // indicate that account creation failed
                                Overlay_Status_label.Text = "Upload status: The file has to be less than 5mb!";
                                Overlay_Status_label.ForeColor = System.Drawing.Color.Red;
                                Overlay_Status_label.Visible = true;
                            }
                            
                        }
                        
                    }
                    else
                        Overlay_Status_label.Text = "Upload status: Only JPEG files are accepted!";
                }
                catch (Exception ex)
                {
                    Overlay_Status_label.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
        }

        public void CombineDOB()
        {
            //FullDOB = DropDownList1.SelectedItem.Text + "-" + DropDownList2.SelectedItem.Text + "-" + DropDownList3.SelectedItem.Text;
            int day = 1, month= 1, year = 1999;
            bool success = Int32.TryParse(DropDownList1.SelectedValue, out day) && Int32.TryParse(DropDownList2.SelectedValue, out month)
                && Int32.TryParse(DropDownList3.SelectedValue, out year);
            year = year <= 0 ? 1999 : year;
            FullDOB = new DateTime(year, month, day); 

        }

        protected void NameValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (Regex.IsMatch(args.Value, @"^[^(1-9)]+$") && !Regex.IsMatch(args.Value, @"<[^>]+>"));
        }
    }
}