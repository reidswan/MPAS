using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Models;
using System.Data;
using System.Data.SqlClient;

namespace MPAS
{
    public partial class AnnouncementView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // get the announcement to view and populate the page with its details
            string strID = Request.QueryString["announcementID"];
            int announcementID = 0;
            if(strID != null && Int32.TryParse(strID, out announcementID))
            {
                PopulatePage(announcementID);
            }
        }

        private void PopulatePage(int id)
        {
            // fill the announcement object from the database
            Announcement announcement = GetAnnouncement(id);

            // set the control text to those values
            AnnouncementTitle_Label.Text = announcement.Title;
            AnnouncementDate_Label.Text = announcement.CreationDate.ToShortTimeString() + ", "
                + announcement.CreationDate.ToShortDateString();
            AnnouncementGroup_Label.Text = announcement.Group.ToString();
            Content_Label.Text = announcement.Content;
        }

        private Announcement GetAnnouncement(int id)
        {
            Announcement announcement = new Announcement(id);

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand announcementComm = new SqlCommand("SELECT Title, Date, Content, GroupNumber FROM Announcements WHERE Id = @id");
            announcementComm.Parameters.Add("@id", SqlDbType.Int);
            announcementComm.Parameters["@id"].Value = id;
            announcementComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = announcementComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    announcement.Title = reader.GetString(0);
                    announcement.CreationDate = reader.GetDateTime(1);
                    announcement.Content = reader.GetString(2);
                    int groupId = reader.GetInt32(3);
                    announcement.Group = groupId == 0 ? MentorGroup.GENERAL : new MentorGroup() { Id = groupId };
                } else
                {
                    // fields set to inform reader that the announcement does not exist
                    announcement.Title = "Announcement " + id + " not found";
                    announcement.CreationDate = DateTime.Now;
                    announcement.Content = "";
                    announcement.Group = MentorGroup.GENERAL;
                }
            }

            return announcement;
        }
    }
}