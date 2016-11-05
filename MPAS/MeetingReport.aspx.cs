using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Logic;
using MPAS.Models;

namespace MPAS
{
    public partial class MeetingReport : System.Web.UI.Page
    {
        List<Meeting> meetings;
        string script = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.User.IsInRole("Administrator"))
            {
                Response.Redirect("/Error/AuthError?source=MeetingReport");
            }
            
            int maxGroupNo = DatabaseUtilities.GetHighestGroupNumber();
            for(int i = 1; i <= maxGroupNo; i++)
            {
                if (MentorGroupManager.GetGroup(i) != null)
                {
                    ListItem li = new ListItem();
                    li.Text = "Group " + i;
                    li.Value = i.ToString();
                    GroupSelect_List.Items.Add(li);
                }
            }
        }

        protected void GroupSelect_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            // remove the default "Select a Group" value
            if (GroupSelect_List.Items[0].Value == "-1")
            {
                GroupSelect_List.Items.RemoveAt(0);
            }

            // reset the table for repopulation
            AttendanceTable.Controls.Clear();
            TableHeaderRow header = new TableHeaderRow();
            TableHeaderCell meetingTitle = new TableHeaderCell(), attendancePercent = new TableHeaderCell();
            meetingTitle.Text = "Meeting";
            attendancePercent.Text = "Attended (%)";
            header.Controls.Add(meetingTitle);
            header.Controls.Add(attendancePercent);
            AttendanceTable.Controls.Add(header);
            // populate table with details for every meeting
            foreach (Meeting m in DatabaseUtilities.GetMeetingsForGroup(Int32.Parse(GroupSelect_List.SelectedValue)))
            {
                // if this is a valid meeting
                if (m != null && m.Group != null)
                {
                    TableRow row = new TableRow();
                    TableCell titleCell = new TableCell(), attendanceCell = new TableCell();

                    titleCell.Text = m.Title;

                    // calculate the percentage attendance
                    int attendedCount = 0;
                    int total = 0;
                    foreach(var student in m.Group.Mentees)
                    {
                        total++;
                        if (DatabaseUtilities.GetAttendance(student.StudentNumber, m.ID))
                        {
                            attendedCount++;
                        }
                    }

                    attendanceCell.Text = ((100*attendedCount)/ (total <= 0 ? 1 : total)).ToString() + "%";

                    row.Controls.Add(titleCell);
                    row.Controls.Add(attendanceCell);
                    AttendanceTable.Controls.Add(row);
                }
            }
        }
    }
}