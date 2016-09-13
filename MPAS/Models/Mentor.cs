using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public class Mentor : User
    {
        public static Mentor NULL = new Mentor()
        {
            DateOfBirth = DateTime.MinValue,
            FirstName = "",
            Surname = "",
            StudentNumber = "STDNUM000",
            GroupNumber = 0
        };

        private Mentor() { }

        public Mentor(string studentNumber)
        {
            base.StudentNumber = studentNumber;
        }

        public void MakeAnnouncement(string title, string content)
        {
            base.Group.AddActivity(new Announcement(base.Group, title, content));
        }

        public void MakeGeneralAnnouncement(string title, string content)
        {
            MentorGroup.GENERAL.AddActivity(new Announcement(MentorGroup.GENERAL, title, content));
        }

        public void CreateMeeting(string title, string location, string agenda, DateTime startTime, DateTime endTime)
        {
            base.Group.AddActivity(new Meeting(base.Group, title, location, agenda, startTime, endTime));
        }
    }
}