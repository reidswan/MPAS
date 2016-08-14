﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
namespace MPAS.Admin
{
    public partial class DeleteUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            using (var _db = new MPAS.Models.ApplicationDbContext())
            {
                string studentNumber = StudentNumberDelete_TextBox.Text;
                var myItem = (from c in _db.Users where c.UserName == studentNumber select c).FirstOrDefault();
                if (myItem != null)
                {
                    _db.Users.Remove(myItem);
                    _db.SaveChanges();

                    _db.Database.ExecuteSqlCommand("DELETE FROM ProfileDetails WHERE StudentNumber=@p0", studentNumber);

                    // indicate success
                    StatusLabel.Text = "User deleted";
                    StatusLabel.Visible = true;
                    StatusLabel.ForeColor = System.Drawing.Color.Green;

                    StudentNumberDelete_TextBox.Text = "";
                }
                else
                {
                    //indicate failure
                    StatusLabel.Text = "Unable to locate user";
                    StatusLabel.Visible = true;
                    StatusLabel.ForeColor = System.Drawing.Color.Red;
                }
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