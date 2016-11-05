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
    public partial class MeetingList : System.Web.UI.Page
    {
        User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.User.IsInRole("Administrator"))
            {
                currentUser = Administrator.Get();
                MakeMeetingButton.Visible = true;
            } else
            {
                currentUser = DatabaseUtilities.GetUser(this.User.Identity.Name);
                if (currentUser is Mentor)
                {
                    MakeMeetingButton.Visible = true;
                }
            }

            // populate the table with upcoming meetings
            foreach (Meeting m in DatabaseUtilities.GetMeetingsForGroup(currentUser.GroupNumber))
            {
                NoMeetings.Visible = false;
                MeetingTable.Visible = true;
                // only select those meetings in the future
                if (m.StartTime > DateTime.Now)
                {
                    // rows in the announcements table have the form
                    // | title | made by | date | start | end
                    TableRow row = new TableRow();

                    TableCell title = new TableCell();
                    TableCell madeBy = new TableCell();
                    TableCell date = new TableCell();
                    TableCell start = new TableCell();
                    TableCell end = new TableCell();

                    title.Text = "<a href='MeetingView?meetingID=" + m.ID + "'><h5>" + m.Title + "</h5></a>";
                    madeBy.Text = "<h5>" + m.MadeBy.FirstName + " " + m.MadeBy.Surname + "</h5>";
                    date.Text = "<h5>" + m.StartTime.ToShortDateString() + "</h5>";
                    start.Text = "<h5>" + m.StartTime.ToShortTimeString() + "</h5>";
                    end.Text = "<h5>" + m.EndTime.ToShortTimeString() + "</h5>";

                    row.Controls.Add(title);
                    row.Controls.Add(madeBy);
                    row.Controls.Add(date);
                    row.Controls.Add(start);
                    row.Controls.Add(end);

                    MeetingTable.Controls.Add(row);
                    System.Diagnostics.Debug.WriteLine("Added!");
                }
            }
        }
    }
}