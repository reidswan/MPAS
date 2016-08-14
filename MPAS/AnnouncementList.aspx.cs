using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MPAS.Models;

namespace MPAS
{
    public partial class AnnouncementList : System.Web.UI.Page
    {
        User currentUser;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = null;
            if (!this.User.IsInRole("Administrator"))
            {
                // get the mentor/mentee object based on the user's student number
                currentUser = GetUser(this.User.Identity.Name);
            } else
            {
                // get the admin account
                currentUser = Administrator.Get();
            }

            if(currentUser is Administrator || currentUser is Mentor)
            {
                MakeAnnouncementButton.Visible = true;
            }

            if (CountAnnouncements(currentUser.GroupNumber) != 0)
            {
                NoAnnouncements.Visible = false;
                foreach(Announcement a in GetAnnouncements(currentUser.GroupNumber))
                {
                    // rows in the announcements table have the form
                    // | title | made by | date |
                    TableRow row = new TableRow();

                    TableCell title = new TableCell();
                    title.Text = "<a href='AnnouncementView.aspx?announcementID=" + a.ID + "'>" + 
                        "<h4 style='width:50%'>" + a.Title + "</h4></a>";
                    row.Controls.Add(title);

                    TableCell madeBy = new TableCell();
                    madeBy.Text = "<h5 style='width:25%'>" + a.MadeBy.FirstName + " " + a.MadeBy.Surname + "</h4>";
                    row.Controls.Add(madeBy);

                    TableCell date = new TableCell();
                    date.Text = "<h5 style='width:25%'>" + a.CreationDate.ToShortTimeString() + ", " + a.CreationDate.ToShortDateString() + "</h4>";
                    row.Controls.Add(date);

                    AnnouncementTable.Controls.Add(row);
                }
                AnnouncementTable.Visible = true;
            } 
        }

        private int CountAnnouncements(int groupNumber)
        {
            int announcementCount = 0;
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand announcementComm = new SqlCommand("SELECT COUNT(*) FROM Announcements WHERE GroupNumber = '0' OR GroupNumber='" + groupNumber + "';");
            announcementComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = announcementComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    announcementCount = reader.GetInt32(0);
                }
            }

            return announcementCount;
        }

        private List<Announcement> GetAnnouncements(int groupNumber)
        {
            List<Announcement> announcements = new List<Announcement>();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand titleComm = new SqlCommand("SELECT Id, Title, MadeBy, Date, Content FROM Announcements WHERE GroupNumber='0' OR GroupNumber='" + groupNumber + "' ORDER BY Date DESC");
            titleComm.Connection = conn;
            conn.Open();

            using (conn) 
            using(var reader = titleComm.ExecuteReader())
            {
                for (int i = 0; i < CountAnnouncements(groupNumber); i++) {
                    reader.Read();

                    // create and populate the announcement
                    Announcement a = new Announcement(reader.GetInt32(0)); 
                    a.Title = reader.GetString(1);
                    a.MadeBy = GetUser(reader.GetString(2));
                    a.CreationDate = reader.GetDateTime(3);
                    a.Content = reader.GetString(4);
                    announcements.Add(a);
                }

            }
            return announcements;
        }

        User GetUser(string studentNumber)
        {
            if (studentNumber == "01360406") return Administrator.Get();
            User u = null;
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getUserComm = new SqlCommand("SELECT StudentNumber, FirstName, Surname, DateOfBirth, Role, GroupNumber " + 
                " FROM ProfileDetails WHERE StudentNumber=@studentNumber");
            // set the parameters
            getUserComm.Parameters.Add("@studentNumber", SqlDbType.VarChar);
            getUserComm.Parameters["@studentNumber"].Value = studentNumber;
            getUserComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = getUserComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // read the record
                    reader.Read();
                    int role = reader.GetInt32(4); // get the role of the user
                    if (role == 0)
                    {
                        u = new Mentee(studentNumber);
                    }
                    else
                    {
                        u = new Mentor(studentNumber);
                    }
                    // set the values in the object
                    u.FirstName = reader.GetString(1);
                    u.Surname = reader.GetString(2);
                    if(!reader.IsDBNull(3)) u.DateOfBirth = reader.GetDateTime(3);
                    //u.Group = GetGroup(reader.GetInt32(4));
                    u.GroupNumber = (!reader.IsDBNull(4)) ? reader.GetInt32(4) : 0;
                }
            }
            return u;
        }
    }
}