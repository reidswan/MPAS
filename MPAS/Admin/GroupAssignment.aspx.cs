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
        private int count = 0;
        private bool isGenerating = false;
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
            AssignmentLabel.Text = "Assigning users to groups. Page will redirect on completion";
            // begin assignment to groups
            Scheduler s = new Scheduler(DatabaseUtilities.GetMentorStudentNumbers(), DatabaseUtilities.GetMenteeStudentNumbers());
            s.Schedule();
            var groups = s.AssignToGroups(3);
            // now take the assigned data and create the groups
            DatabaseUtilities.CreateGroups(groups);
            // redirect to page showing group assignments
            Response.Redirect("~/Admin/GroupDisplay.aspx");
        }

        /**
         * Initiates the random generation of 25 mentors and 100 mentees
         */
        protected void GenerateButton_Click(object sender, EventArgs e)
        {
            isGenerating = true;
            AssignmentLabel.Visible = true;
            AssignButton.Enabled = false;
            count = 0;
            // hide the generate button to prevent adding a further 125 users
            GenerateButton.Visible = false;
            AssignmentLabel.Text = "Generating mentors. Please wait";
            // generate the 25 mentors
            DatabaseUtilities.RandomlyGenerateMentor(25);
            AssignmentLabel.Text = "Generating mentees. Please wait";
            // generate 100 mentees
            DatabaseUtilities.RandomlyGenerateMentee(100);
            AssignButton.Enabled = true;
            isGenerating = false;
            AssignmentLabel.Text = "Users generated";
        }

    }
}