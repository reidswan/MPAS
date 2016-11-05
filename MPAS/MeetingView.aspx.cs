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
    public partial class MeetingView : System.Web.UI.Page
    {
        User currentUser;
        int meetingID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(this.User == null || !this.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Account/Login.aspx");
            }
            if (this.User.IsInRole("Administrator"))
            {
                currentUser = Administrator.Get();
            } else
            {
                currentUser = DatabaseUtilities.GetUser(this.User.Identity.Name);
            }
            // determine the meeting that is to be viewed
            string strID = Request.QueryString["meetingID"];
            meetingID = 0;
            if (strID != null && Int32.TryParse(strID, out meetingID))
            {
                // if the meeting is valid, populate the page with its details
                PopulatePage(meetingID);
            }
        }

        /* 
         * Set the values of the elements of the page based on the details of the meeting
         * with the given ID
         */
        private void PopulatePage(int id)
        {
            Meeting m = DatabaseUtilities.GetMeetingbyId(id);
            MeetingTitle_Label.Text = m.Title;
            MeetingGroup_Label.Text = "Group " + m.Group.Id;
            Agenda_Label.Text = m.Agenda;
            Location_Label.Text = m.Location;
            MeetingDate_Label.Text = m.StartTime.ToShortDateString();
            MeetingTime_Label.Text = m.StartTime.ToShortTimeString() + " - " + m.EndTime.ToShortTimeString();
            if ((currentUser is Mentor && (currentUser as Mentor).GroupNumber == m.Group.Id) || currentUser is Administrator)
            {
                Feedback_Button.Visible = true;
            }
        }
        
        protected void Feedback_Button_Click(object sender, EventArgs e)
        {
            Response.Redirect("MeetingFeedback.aspx?meetingID=" + meetingID);
        }
    }
}