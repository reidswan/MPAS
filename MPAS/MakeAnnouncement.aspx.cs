using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace MPAS
{
    public partial class MakeAnnouncement : System.Web.UI.Page
    {
        private User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(User.IsInRole("Administrator"))
            {
                currentUser = Administrator.Get();
            } else
            {
                currentUser = GetUser(User.Identity.Name);
            }

            if (currentUser is Administrator)
            {
                // allow mentor to select which group to send announcement to 
                AdminOptions.Visible = true;
                PopulateGroupDropDown();
                
            } else if (currentUser is Mentor)
            {
                MentorOptions.Visible = true;
            } 
            else if (currentUser is Mentee)
            {
                Response.Redirect("~/Error/AuthError.aspx");
            }
        }
        User GetUser(string studentNumber)
        {
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
                    if (!reader.IsDBNull(3)) u.DateOfBirth = reader.GetDateTime(3);
                    //u.Group = GetGroup(reader.GetInt32(4));
                    u.GroupNumber = (!reader.IsDBNull(4)) ? reader.GetInt32(4) : 0;
                }
            }
            return u;
        }

        private void PopulateGroupDropDown()
        {
            // add general announcement option
            Groups_DropDown.Items.Add("General Announcement");

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand populateDropdownComm = new SqlCommand("SELECT MAX(GroupNumber) FROM ProfileDetails ");
            // set the parameters
            populateDropdownComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = populateDropdownComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    int maxGroup = (reader.IsDBNull(0)) ? 0 : reader.GetInt32(0); // get highest group number and fill dropdown until there
                    for (int i = 1; i <= maxGroup; i++)
                    {
                        Groups_DropDown.Items.Add("Group " + i);
                    }
                }
            }
        }

        protected void SubmitButton_Click(Object source, EventArgs args)
        {
            Announcement created = new Announcement();
            created.Title = Title_Textbox.Text;
            created.Content = Content_Textbox.Text;
            created.CreationDate = DateTime.Now;
            created.MadeBy = currentUser;
            created.Group = new MentorGroup();
            if (currentUser is Administrator)
            {
                created.Group.Id = Groups_DropDown.SelectedIndex;
            } else if (currentUser is Mentor)
            {
                created.Group.Id = (General_CheckBox.Checked) ? 0 : currentUser.GroupNumber;
            }

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand newAnnouncementComm = new SqlCommand("INSERT INTO Announcements (Id, Title, Content, GroupNumber, MadeBy, Date) " + 
                " values(@id, @title, @content,@groupNum, @maker, @created)");
            //parameterization
            newAnnouncementComm.Parameters.Add("@id", SqlDbType.Int);
            newAnnouncementComm.Parameters.Add("@title", SqlDbType.VarChar);
            newAnnouncementComm.Parameters.Add("@content", SqlDbType.VarChar);
            newAnnouncementComm.Parameters.Add("@created", SqlDbType.DateTime);
            newAnnouncementComm.Parameters.Add("@maker", SqlDbType.VarChar);
            newAnnouncementComm.Parameters.Add("@groupNum", SqlDbType.Int);

            // set the parameters
            newAnnouncementComm.Parameters["@id"].Value = created.ID;
            newAnnouncementComm.Parameters["@title"].Value = created.Title;
            newAnnouncementComm.Parameters["@content"].Value = created.Content;
            newAnnouncementComm.Parameters["@created"].Value = created.CreationDate;
            newAnnouncementComm.Parameters["@maker"].Value = created.MadeBy.StudentNumber;
            newAnnouncementComm.Parameters["@groupNum"].Value = created.Group.Id;

            newAnnouncementComm.Connection = conn;
            conn.Open();

            using (conn)
            {
                newAnnouncementComm.ExecuteNonQuery();
            }

            // redirect to announcement list page
            Response.Redirect("~/AnnouncementView.aspx?announcementID=" + created.ID);
        }

        protected void Validate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (Regex.IsMatch(args.Value, @"^[^(1-9)]+$") && !Regex.IsMatch(args.Value, @"<[^>]+>"));
        }
    }
}