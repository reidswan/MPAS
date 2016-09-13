using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Models;
using MPAS.Logic;
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
            Announcement announcement = DatabaseUtilities.GetAnnouncementById(id);

            // set the control text to those values
            AnnouncementTitle_Label.Text = announcement.Title;
            AnnouncementDate_Label.Text = announcement.CreationDate.ToShortTimeString() + ", "
                + announcement.CreationDate.ToShortDateString();
            AnnouncementGroup_Label.Text = announcement.Group.ToString();
            Content_Label.Text = announcement.Content;
        }
        
    }
}