using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MPAS.Models;
using MPAS.Logic;

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
                currentUser = DatabaseUtilities.GetUser(this.User.Identity.Name);
            } else
            {
                // get the admin account
                currentUser = Administrator.Get();
            }

            if(currentUser is Administrator || currentUser is Mentor)
            {
                MakeAnnouncementButton.Visible = true;
            }

            NoAnnouncements.Visible = false;
            foreach(Announcement a in DatabaseUtilities.GetAnnouncements(currentUser.GroupNumber))
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
}