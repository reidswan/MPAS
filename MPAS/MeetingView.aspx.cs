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
        protected void Page_Load(object sender, EventArgs e)
        {
            // determine the meeting that is to be viewed
            string strID = Request.QueryString["meetingID"];
            int announcementID = 0;
            if (strID != null && Int32.TryParse(strID, out announcementID))
            {
                // if the meeting is valid, populate the page with its details
                PopulatePage(announcementID);
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
        }
    }
}