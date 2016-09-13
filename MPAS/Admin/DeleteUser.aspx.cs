using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using MPAS.Logic;

namespace MPAS.Admin
{
    public partial class DeleteUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.User.IsInRole("Administrator"))
            {
                Response.Redirect("~/Error/AuthError.aspx?source=DeleteUser");
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            if(DatabaseUtilities.DeleteUser(StudentNumberDelete_TextBox.Text)) { 
                    StatusLabel.Text = "User deleted";
                    StatusLabel.Visible = true;
                    StatusLabel.ForeColor = System.Drawing.Color.Green;

                    StudentNumberDelete_TextBox.Text = "";
            } else
            {
                //indicate failure
                StatusLabel.Text = "Unable to locate user";
                StatusLabel.Visible = true;
                StatusLabel.ForeColor = System.Drawing.Color.Red;
            }

        }

        protected void StudentNumberValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (Regex.IsMatch(args.Value, @"^[a-zA-Z]{6,6}[0-9]{3,3}$") // student number
                || Regex.IsMatch(args.Value, @"^[0-9]{8,8}$") // staff number
                );
        }
    }


}