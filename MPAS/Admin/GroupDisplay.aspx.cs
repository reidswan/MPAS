using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Models;
using MPAS.Logic;

namespace MPAS.Admin
{
    public partial class GroupDisplay : System.Web.UI.Page
    { 
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!this.User.IsInRole("Administrator"))
            {
                Response.Redirect("~/Error/AuthError.aspx?source=GroupDisplay.aspx");
            }

            // render the table with all users and their group numbers
            PageSetup();

        }
        /**
         * Fills the group assignments table 
         */
        private void PageSetup()
        {
            List<User> users = new List<Models.User>();
            Dictionary<int, MentorGroup> groups = new Dictionary<int, MentorGroup>();
            foreach(string stdNum in DatabaseUtilities.GetMenteeStudentNumbers())
            {
                users.Add(DatabaseUtilities.GetUser(stdNum));
            }
            foreach (string stdNum in DatabaseUtilities.GetMentorStudentNumbers())
            { 
                users.Add(DatabaseUtilities.GetUser(stdNum));
            }

            foreach(User u in users)
            {
                if (groups.ContainsKey(u.GroupNumber))
                {

                }
            }
        }
    }
}