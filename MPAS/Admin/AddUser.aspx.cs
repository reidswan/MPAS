using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Models;
using MPAS.Logic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using MPAS.Logic;

namespace MPAS.Admin
{
    public partial class AddUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            if(!this.User.IsInRole("Administrator"))
            {
                Response.Redirect("~/Error/AuthError.aspx?source=AddUser");
            }
        }

        protected void SubmitButton_Clicked(object sender, EventArgs e)
        {
            if (this.IsValid) CreateUser();
        }

        public void CreateUser()
        {

            if(DatabaseUtilities.AddUser(StudentNumber_TextBox.Text, FirstName_TextBox.Text, Surname_TextBox.Text, MentorCheckBox.Checked)) {

                // indicate that the account was created successfully
                Status_Label.Text = FirstName_TextBox.Text + " " + Surname_TextBox.Text + " was added successfully";
                Status_Label.ForeColor = System.Drawing.Color.Green;
                Status_Label.Visible = true;

                // remove existing info in textboxes
                StudentNumber_TextBox.Text = "";
                FirstName_TextBox.Text = "";
                Surname_TextBox.Text = "";
                MentorCheckBox.Checked = false;
            }
            else
            {
                // indicate that account creation failed
                Status_Label.Text = "Failed to add user";
                Status_Label.ForeColor = System.Drawing.Color.Red;
                Status_Label.Visible = true;
            }

        }


        protected void StudentNumberValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (Regex.IsMatch(args.Value, @"^[a-zA-Z]{6,6}[0-9]{3,3}$") // student number
                || Regex.IsMatch(args.Value, @"^[0-9]{8,8}$") // staff number
                );
        }

        protected void NameValidate(object source, ServerValidateEventArgs args)
        {
            // non-numeric name of length at least 1
            args.IsValid = (Regex.IsMatch(args.Value, @"^[^(1-9)]+$"));
        }
    }
}