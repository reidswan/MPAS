﻿using System;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            // send the user to the login page if they are not logged in
            if((System.Web.HttpContext.Current.User == null) ||
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

            // populate the announcements preview on the homepage
            List<Announcement> announcements = DatabaseUtilities.GetAnnouncements(currentUser.GroupNumber);
            if (announcements.Count >= 1)
            {
                AnnouncementTitle1_Label.Text = "<a href='AnnouncementView?announcementID=" + announcements[0].ID + "'>" + 
                    announcements[0].Title + "</a>";
                AnnouncementDate1_Label.Text = announcements[0].CreationDate.ToShortDateString();
                if (announcements.Count >= 2)
                {
                    AnnouncementTitle2_Label.Text = "<a href='AnnouncementView?announcementID=" + announcements[1].ID + "'>" +
                        announcements[1].Title + "</a>";
                    AnnouncementDate2_Label.Text = announcements[1].CreationDate.ToShortDateString();
                }
            }

            // populate the meetings preview on the homepage
            List<Meeting> meetings = DatabaseUtilities.GetMeetingsForGroup(currentUser.GroupNumber);
            if (meetings.Count >= 1)
            {
                MeetingLocation1_Label.Text = "<a href='MeetingView?meetingID=" + meetings[0].ID + "'>" +
                    meetings[0].Location + "</a>";
                MeetingDate1_Label.Text = meetings[0].StartTime.ToShortDateString();
                if (meetings.Count >= 2)
                {
                    MeetingLocation2_Label.Text = "<a href='MeetingView?meetingID=" + meetings[1].ID + "'>" +
                        meetings[1].Location + "</a>";
                    MeetingDate2_Label.Text = meetings[1].StartTime.ToShortDateString();
                }
            }
        }
    }
}