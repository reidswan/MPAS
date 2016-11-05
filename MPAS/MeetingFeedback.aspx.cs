using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Models;
using MPAS.Logic;

namespace MPAS
{
    public partial class MeetingFeedback : System.Web.UI.Page
    {
        User currentUser;
        Meeting currentMeeting;
        int meetingID;
        Dictionary<string, CheckBox> dynamicallyAdded;

        protected void Page_Load(object sender, EventArgs e)
        { 
            if(this.User == null || !this.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Account/Login.aspx");
            } else
            {
                currentUser = DatabaseUtilities.GetUser(this.User.Identity.Name);
            }
            
            if (Request.QueryString["meetingID"] == null || !Int32.TryParse(Request.QueryString["meetingID"], out meetingID))
            {
                Response.Redirect("/MeetingList.aspx");
            }

            currentMeeting = DatabaseUtilities.GetMeetingbyId(meetingID);
            if (currentUser is Mentor && currentUser.GroupNumber == currentMeeting.Group.Id || currentUser is Administrator)
            {
                dynamicallyAdded = new Dictionary<string, CheckBox>();
                PageSetup();
            } else
            {
                Response.Redirect("/Error/AuthError.aspx?source=MeetingFeedback.aspx");
            }
        }

        /**
         * Fills the table with the list of all users as well as checkboxes indicating attendance
         */
        private void PageSetup()
        {
            foreach(Mentee m in currentMeeting.Group.Mentees)
            {
                TableRow row = new TableRow();
                TableCell student = new TableCell();
                student.Text = $"<h4>{m.FirstName} {m.Surname}</h4>";
                TableCell attended = new TableCell();
                CheckBox attendedChk = new CheckBox();
                attendedChk.ID = m.StudentNumber + "_attended"; 
                attendedChk.Checked = DatabaseUtilities.GetAttendance(m.StudentNumber, meetingID);
                attendedChk.CssClass = "checkbox";
                attendedChk.Style[HtmlTextWriterStyle.TextAlign] = "center";
                dynamicallyAdded[m.StudentNumber] = attendedChk;
                attended.Controls.Add(attendedChk);
                row.Controls.Add(student);
                row.Controls.Add(attended);
                AttendanceTable.Controls.Add(row);
            }
        }

        /**
         * Updates the database with the data and navigates to the homepage
         */
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            // register the user's attendance
            foreach(Mentee m in currentMeeting.Group.Mentees)
            {
                if (dynamicallyAdded.ContainsKey(m.StudentNumber))
                {
                    DatabaseUtilities.SetAttendance(m.StudentNumber, meetingID, dynamicallyAdded[m.StudentNumber].Checked);
                } 
            }
        }
    }
}