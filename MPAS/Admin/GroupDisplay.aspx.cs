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
                if (!groups.ContainsKey(u.GroupNumber))
                {
                    groups[u.GroupNumber] = new MentorGroup();
                }
                if (u is Mentor)
                {
                    groups[u.GroupNumber].Mentor = (Mentor)u;
                } else if (u is Mentee)
                {
                    groups[u.GroupNumber].AddMentee((Mentee)u);
                }
            }

            int maxGroup = 0;
            foreach(int ind in groups.Keys)
            {
                if (ind > maxGroup) maxGroup = ind;
            }

            for(int i = 0; i <= maxGroup; i++)
            {
                if (!groups.ContainsKey(i) || groups[i] == null) continue;
                TableRow tr = new TableRow();
                TableCell groupNum = new TableCell(), name = new TableCell(), stdNum = new TableCell();
                
                if (groups[i].Mentor != null)
                {
                    groupNum.Text = $"<h5>{i}</h5>";
                    name.Text = $"<h5><b>{groups[i].Mentor.FirstName} {groups[i].Mentor.Surname}</b></h5>";
                    stdNum.Text = $"<h5>{groups[i].Mentor.StudentNumber}</h5>";
                    tr.Controls.Add(groupNum);
                    tr.Controls.Add(name);
                    tr.Controls.Add(stdNum);
                    GroupsTable.Controls.Add(tr);
                }

                foreach (Mentee m in groups[i].Mentees)
                {
                    tr = new TableRow();
                    groupNum = new TableCell();
                    name = new TableCell();
                    stdNum = new TableCell();

                    groupNum.Text = $"<h5>{i}</h5>";
                    name.Text = $"<h5>{m.FirstName} {m.Surname}</h5>";
                    stdNum.Text = $"<h5>{m.StudentNumber}</h5>";

                    tr.Controls.Add(groupNum);
                    tr.Controls.Add(name);
                    tr.Controls.Add(stdNum);
                    GroupsTable.Controls.Add(tr);

                }
            }
        }
    }
}