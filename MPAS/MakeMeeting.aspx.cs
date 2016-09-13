using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Logic;
using MPAS.Models;

namespace MPAS
{
    public partial class MakeMeeting : System.Web.UI.Page
    {
        private User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.User.IsInRole("Administrator"))
            {
                AdminOptions.Visible = true;
                currentUser = Administrator.Get();
            } else
            {
                currentUser = DatabaseUtilities.GetUser(this.User.Identity.Name);
            }

            // set the default meeting date to tomorrow
            DatePicker.SelectedDate = DateTime.Today.AddDays(1);
            
            // populate the dropdowns with valid times
            for (int i = 8; i <= 17; i++)
            {
                StartHour.Items.Add((i < 10 ? "0" : "") + i);
                EndHour.Items.Add((i < 10 ? "0" : "") + i);
            }

            for (int i = 0; i <= 55; i+=5)
            {
                StartMinute.Items.Add((i < 10 ? "0" : "") + i);
                EndMinute.Items.Add((i < 10 ? "0" : "") + i);
            }

            // populate the group number dropdown with valid group numbers
            for (int i = 1; i <= DatabaseUtilities.GetHighestGroupNumber(); i++)
            {
                Groups_DropDown.Items.Add(""+i);
            }
        }

        protected void DateSelectionChanged(Object source, EventArgs args)
        {
            if(DatePicker.SelectedDate <= DateTime.Today)
            {
                Status_Label.Visible = true;
                Status_Label.Text = "Please select a date after " + DateTime.Today.ToShortDateString();
                Status_Label.ForeColor = System.Drawing.Color.Red;
                SubmitButton.Enabled = false;
            } else
            {
                Status_Label.Visible = false;
                SubmitButton.Enabled = true;
            }
        }

        protected void SubmitButton_Click(Object source, EventArgs args)
        {
            DateTime selectedDay = DatePicker.SelectedDate;
            DateTime startTime = new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day,
                Int32.Parse(StartHour.Items[StartHour.SelectedIndex].Text),
                Int32.Parse(StartMinute.Items[StartMinute.SelectedIndex].Text), 0);
            DateTime endTime = new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day,
                Int32.Parse(EndHour.Items[EndHour.SelectedIndex].Text),
                Int32.Parse(EndMinute.Items[EndMinute.SelectedIndex].Text), 0);

            if (endTime <= startTime)
            {
                Status_Label.Visible = true;
                Status_Label.Text = "Please select an end time later than the chosen start time";
                Status_Label.ForeColor = System.Drawing.Color.Red;
            } else
            {
                Status_Label.Visible = false;
                // if the user is not an admin, take their group number, 
                // else get the group selected by the admin
                int selectedGroup = currentUser.GroupNumber;
                {
                    selectedGroup = Int32.Parse(Groups_DropDown.Items[Groups_DropDown.SelectedIndex].Text);
                }

                DatabaseUtilities.AddMeeting(Title_Textbox.Text, Location_Textbox.Text, Agenda_Textbox.Text,
                    startTime, endTime, selectedGroup, currentUser.StudentNumber);
            }
        }
    }
}