using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using MPAS.Logic;

namespace MPAS.Models
{
    public class Announcement : GroupActivity
    {
        string content;
        public Announcement()
        {

        }

        public Announcement (MentorGroup g, string title, string content)
        {
            base.Group = g;
            base.Title = title;
            this.content = content;
        }

        public string Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
            }
        }

        public override void EmailAlert()
        {
            string subject = "[MPAS Announcement] " + Title;
            string body = content;
            foreach(Mentee m in Group.Mentees)
            {
                EmailUtilities.Email(m.StudentNumber + "@myuct.ac.za", subject, body);
            }
        }
    }
}