using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace MPAS.Models
{
    public class Meeting : GroupActivity
    {
        static int nextId = 0;
        DateTime startTime, endTime;
        string location;
        string agenda;
        Dictionary<string, AttendanceStatus> attendance;
        
        public Meeting(MentorGroup g, string title, string location, string agenda, DateTime startTime, DateTime endTime)
        {
            this.ID = ++nextId;
            foreach(Mentee m in g.Mentees)
            {
                attendance.Add(m.StudentNumber, AttendanceStatus.NO_DATA);
            }
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public Meeting(int ID, MentorGroup g, string title, string location, string agenda, DateTime startTime, DateTime endTime)
        {
            this.ID = ID;
            foreach (Mentee m in g.Mentees)
            {
                attendance.Add(m.StudentNumber, AttendanceStatus.NO_DATA);
            }

            this.startTime = startTime;
            this.endTime = endTime;
        }

        public void RecordAttendance(string stdNum, AttendanceStatus t)
        {
            attendance[stdNum] = t;
        }

        public void RecordAttendance(Mentee m, AttendanceStatus t)
        {
            RecordAttendance(m.StudentNumber, t);
        }

        public AttendanceStatus GetAttendance(string stdNum)
        {
            return attendance[stdNum];
        }

        public override void EmailAlert()
        {
            foreach (Mentee m in Group.Mentees)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("mpasUCT@gmail.com");
                    mail.To.Add(m.StudentNumber + "@myuct.ac.za");
                    mail.Subject = "MPAS Meeting: " + Title;
                    mail.Body = this.ToString();
                    mail.IsBodyHtml = true;

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("mpasUCT@gmail.com", "michaellarryreid");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                }
                catch (Exception ex)
                {
                    // todo : show mail sending exceptions
                }
            }
        }

        public override string ToString()
        {
            string val = "";
            val += "<h1>" + Title + "</h1>"; 
            val += "Date: " + startTime.ToShortDateString() + "\n";
            val += "Time: " + startTime.ToShortTimeString() + " - "  + endTime.ToShortTimeString() + "\n";
            val += "Location: " + location;
            val += "Agenda: " + agenda;
            val += "";

            return val;
        }

        public Dictionary<string, AttendanceStatus> Attendance
        {
            get
            {
                return attendance;
            }

            set
            {
                attendance = value;
            }
        }

        public static int NextId
        {
            get
            {
                return nextId;
            }

            set
            {
                nextId = value;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                startTime = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                endTime = value;
            }
        }

        public string Agenda
        {
            get
            {
                return agenda;
            }

            set
            {
                agenda = value;
            }
        }

        public string Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }
    }

    public enum AttendanceStatus
    {
        NO_DATA = 0, 
        ATTENDED, 
        NOT_ATTENDED
    }
}