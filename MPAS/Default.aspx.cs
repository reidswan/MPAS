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
    public partial class _Default : Page
    {
        User currentUser;
        static readonly int CHARCOUNT = 35;
        protected void Page_Load(object sender, EventArgs e)
        {
            // send the user to the login page if they are not logged in
            if ((System.Web.HttpContext.Current.User == null) ||
                !System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login");
            }

            // populate a user object with the user's details
            if (this.User.IsInRole("Administator"))
            {
                currentUser = Administrator.Get();
            } else
            {
                currentUser = DatabaseUtilities.GetUser(this.User.Identity.Name);
            }

            // populate up to two of the latest announcements preview on the homepage
            List<Announcement> announcements = DatabaseUtilities.GetAnnouncements(currentUser.GroupNumber);
            for (int i = 0; i < 2 && i < announcements.Count; i++) 
            {
                // fill the table with the announcement's details
                TableRow announcementRow = new TableRow();
                TableCell announcementTitleCell = new TableCell();
                TableCell announcementDateCell = new TableCell();
                announcementTitleCell.Text = "<a href='AnnouncementView?announcementID=" + announcements[i].ID + "'>" + 
                    announcements[i].Title + "</a>";
                announcementDateCell.Text = announcements[i].CreationDate.ToShortDateString();
                announcementRow.Controls.Add(announcementTitleCell);
                announcementRow.Controls.Add(announcementDateCell);
                AnnouncementTable.Controls.Add(announcementRow);
            }

            // populate the meetings preview on the homepage
            List<Meeting> meetings = DatabaseUtilities.GetMeetingsForGroup(currentUser.GroupNumber);
            for(int i = 0; i < 2 && i < meetings.Count; i++)
            {
                // fill the table with the meeting's details
                TableRow meetingRow = new TableRow();
                TableCell meetingLocationCell = new TableCell();
                TableCell meetingDateCell = new TableCell();
                meetingLocationCell.Text = "<a href='MeetingView?meetingID=" + meetings[i].ID + "'>" +
                    meetings[i].Location + "</a>";
                meetingDateCell.Text = meetings[i].StartTime.ToShortDateString();

                // add to table
                meetingRow.Controls.Add(meetingLocationCell);
                meetingRow.Controls.Add(meetingDateCell);
                MeetingsTable.Controls.Add(meetingRow);
            }
            
            // get the latest messages from the chatroom
            Chatroom cr = ChatroomManager.GetChatroom(currentUser.GroupNumber);
            for(int i = 0; i < 2 && i < cr.Messages.Count; i++)
            {
                ChatMessage m = cr.Messages[cr.Messages.Count - i - 1] as ChatMessage;
                string content = m.MessageContent;
                // truncate the message for spacing reasons
                if (content.Length >= CHARCOUNT) content = content.Substring(0, CHARCOUNT-3) + "...";

                // table row and cells
                TableRow messageRow = new TableRow();
                TableCell senderCell = new TableCell();
                TableCell contentCell = new TableCell();

                contentCell.Text = content;
                senderCell.Text = m.Source.FirstName + " " + m.Source.Surname;

                // add to table
                messageRow.Controls.Add(senderCell);
                messageRow.Controls.Add(contentCell);
                MessageTable.Controls.Add(messageRow);
            }
        }
    }
}