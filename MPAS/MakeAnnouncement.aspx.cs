using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Models;
using System.Data.SqlClient;
using System.Data;

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
            SqlCommand getUserComm = new SqlCommand("SELECT MAX(GroupNumber) FROM ProfileDetails ");
            // set the parameters
            getUserComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = getUserComm.ExecuteReader())
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

        }
    }
}