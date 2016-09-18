using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Logic;

namespace MPAS.Admin
{
    public partial class GroupAssignment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!this.User.IsInRole("Administrator"))
            {
                Response.Redirect("~/Error/AuthError.aspx?source=GroupAssignment.aspx");
            }
        }

        protected void AssignButton_Click(object sender, EventArgs e)
        {
            AssignmentLabel.Visible = true;
            // begin assignment to groups
            Scheduler s = new Scheduler(DatabaseUtilities.GetMentorStudentNumbers(), DatabaseUtilities.GetMenteeStudentNumbers());
            s.Schedule();
            // now take the assigned data and create the groups
            DatabaseUtilities.CreateGroups(s);
            // redirect to page showing group assignments
            Response.Redirect("~/GroupDisplay.aspx");
        }
    }
}