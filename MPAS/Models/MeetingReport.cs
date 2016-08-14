using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public class MeetingReport
    {
        Mentor mentor;
        Meeting meeting;
        string summary, additionalComments;
        Dictionary<string, string> menteeConcerns; // <studentNum, comment>

        public MeetingReport(Mentor mentor, Meeting meeting)
        {
            this.mentor = mentor;
            this.meeting = meeting;
            foreach(string s in meeting.Attendance.Keys) {
                this.menteeConcerns.Add(s, "");
            }
        }

        public String Summary
        {
            get { return summary; }
            set { this.summary = value; }
        }

        public string AdditionalComments
        {
            get
            {
                return additionalComments;
            }

            set
            {
                additionalComments = value;
            }
        }

        public Dictionary<string, string> MenteeConcerns
        {
            get
            {
                return menteeConcerns;
            }

            set
            {
                menteeConcerns = value;
            }
        }

        public void AddConcern(string studentNumber, string concern)
        {
            this.menteeConcerns[studentNumber] = concern;
        }
    }
}