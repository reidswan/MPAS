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
    public partial class Feedback : System.Web.UI.Page
    {
        User currentUser;
        string role = "";
        string title = "";

        //Obtains the role of the user and loads the relevant Feedback page
        protected void Page_Load(object sender, EventArgs e)
        {
            //Gets the user object based on the user role
            currentUser = null;
            if (!this.User.IsInRole("Administrator"))
            {
                currentUser = DatabaseUtilities.GetUser(this.User.Identity.Name);
            }
            else
            {
                currentUser = Administrator.Get();
            }

            //Makes buttons and the dropdown list visible for the administrator to create feedback forms and view completed feedback forms
            if (currentUser is Administrator)
            {
                MentorFeedbackButton.Visible = true;
                MenteeFeedbackButton.Visible = true;
                ViewLabel.Visible = true;
                FeedbackList.Visible = true;
                View_Button.Visible = true;
                //Populates the dropdown list with all previous feedback forms
                if (FeedbackList.Items.Count == 0)
                {
                    foreach (string a in DatabaseUtilities.GetFeedbackListTitles())
                    {
                        FeedbackList.Items.Add(a);
                    }
                }
            }
            //Makes the current feedback form visible for a user in a mentee or mentor position
            //Makes an additional comments section available in the feedback form for mentors
            else
            {
                if (currentUser is Mentee)
                {
                    role = "Mentee";
                }
                else if (currentUser is Mentor)
                {
                    role = "Mentor";
                    CommentLabel.Visible = true;
                    Comment_Textbox.Visible = true;
                }

                if (currentUser.Feedback == 1)
                {
                    Status_Label.Visible = true;
                    Status_Label.Text = "No feedback to complete";
                }
                else
                {
                    title = DatabaseUtilities.GetTitle(role);

                    if (DatabaseUtilities.CountFeedback(role, title) != 0)
                    {
                        int counter = 1;
                        foreach (FeedbackItem a in DatabaseUtilities.GetFeedback(role, title))
                        {
                            string qID = "Q" + (counter).ToString();
                            string rID = "R" + (counter).ToString();
                            string rbrID = "RBR" + (counter).ToString();
                            TableRow qRow = FeedbackTable.FindControl(rID) as TableRow;
                            Label qLabel = qRow.FindControl(qID) as Label;
                            qLabel.Text = "<h5 style='width:25%'>" + a.Question + "</h4>";
                            qRow.Visible = true;
                            TableRow rbrRow = FeedbackTable.FindControl(rbrID) as TableRow;
                            rbrRow.Visible = true;
                            counter++;
                        }
                        FeedbackTable.Visible = true;
                        SubmitButton.Visible = true;
                    }
                }
            }
        }
        
        //Redirects the administrator to the MakeFeedback page, passing "Mentee" in the query string
        protected void MenteeButton_Click(Object source, EventArgs args)
        {
            Response.Redirect("~/MakeFeedback.aspx?role=Mentee");
        }

        //Redirects the administrator to the MakeFeedback page, passing "Mentor" in the query string
        protected void MentorButton_Click(Object source, EventArgs args)
        {
            Response.Redirect("~/MakeFeedback.aspx?role=Mentor");
        }

        //Redirects the administrator to the view feedback page, passing the selected feedback form name in the query string
        protected void ViewButton_Click(Object source, EventArgs args)
        {
            string selectedFeedback = FeedbackList.SelectedValue;
            string selectedRole = selectedFeedback.Substring(0, 6);
            string selectedTitle = selectedFeedback.Substring(9, selectedFeedback.Length - 9);
            Response.Redirect("~/ViewFeedback.aspx?role=" + selectedRole + "&title=" + selectedTitle);
        }

        //Makes the submit button available to mentees and mentors once they have completed all required fields in the feedback form
        protected void Index_Changed(Object source, EventArgs args)
        {
            RadioButtonList rbList = source as RadioButtonList;
            int rbID = Convert.ToInt32(rbList.ID.Substring(2, 1));
            if (rbID == DatabaseUtilities.CountFeedback(role, title))
            {
                SubmitButton.Enabled = true;
            }
        }

        //Saves the feedback from the user in the Feedback table
        protected void SubmitButton_Click(Object source, EventArgs args)
        {
            int counter = 1;
            foreach (FeedbackItem a in DatabaseUtilities.GetFeedback(role, title))
            {
                string qID = "Q" + (counter).ToString();
                string rID = "R" + (counter).ToString();
                string rbrID = "RBR" + (counter).ToString();
                string rbID = "RB" + (counter).ToString();
                TableRow qRow = FeedbackTable.FindControl(rID) as TableRow;
                Label qLabel = qRow.FindControl(qID) as Label;
                TableRow rbrRow = FeedbackTable.FindControl(rbrID) as TableRow;
                RadioButtonList rbList = rbrRow.FindControl(rbID) as RadioButtonList;
                string question = qLabel.Text.Substring(22, qLabel.Text.Length - 27);

                int feedbackCount = DatabaseUtilities.GetFeedbackCount(role, title, question);
                int feedbackTotal = DatabaseUtilities.GetFeedbackTotal(role, title, question);

                feedbackCount = feedbackCount + 1;
                feedbackTotal = feedbackTotal + Convert.ToInt32(rbList.SelectedValue);

                using (var _db = new MPAS.Models.ApplicationDbContext())
                {
                    _db.Database.ExecuteSqlCommand("UPDATE Feedback SET Count = @p0, Total = @p1 WHERE Role = @p2 AND Title = @p3 AND Question = @p4", feedbackCount, feedbackTotal, role, title, question);
                }
                counter++;
            }

            //Saves the mentor's additional comments in the database
            if (currentUser is Mentor && Comment_Textbox.Text != "")
            {
                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                SqlCommand newFeedbackComm = new SqlCommand("INSERT INTO FeedbackComments (Comment, SNum, Title) " +
                    "VALUES(@comment, @snum, @title)");

                //parameterization
                newFeedbackComm.Parameters.Add("@comment", SqlDbType.VarChar);
                newFeedbackComm.Parameters.Add("@snum", SqlDbType.VarChar);
                newFeedbackComm.Parameters.Add("@title", SqlDbType.VarChar);

                //set parameter values
                newFeedbackComm.Parameters["@comment"].Value = Comment_Textbox.Text;
                newFeedbackComm.Parameters["@snum"].Value = this.User.Identity.Name;
                newFeedbackComm.Parameters["@title"].Value = title;

                newFeedbackComm.Connection = conn;
                conn.Open();
                using (conn)
                {
                    newFeedbackComm.ExecuteNonQuery();
                }
                conn.Close();
            }
            using (var _db = new MPAS.Models.ApplicationDbContext())
            {
                _db.Database.ExecuteSqlCommand("UPDATE ProfileDetails SET Feedback = 1 WHERE StudentNumber = @p0", this.User.Identity.Name);
            }
            Response.Redirect("~/Default");
        }
    }
}