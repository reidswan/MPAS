using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Models;
using MPAS.Logic;
using System.Data.SqlClient;
using System.Data;

namespace MPAS
{
    public partial class MakeFeedback : System.Web.UI.Page
    {
        string role, title;

        //Assigns the role variable to the value passed in the query string
        protected void Page_Load(object sender, EventArgs e)
        {
            role = Request.QueryString["role"];
            if (this.User.Identity.Name != "01360406") // redirect if not admin
            {
                Response.Redirect("~/Error/AuthError.aspx");
            }
        }

        //Disables the title textbox and confirm button
        //Makes the question label, add button, and delete button visible
        protected void ConfirmButton_Click(Object source, EventArgs args)
        {
            Title_Textbox.Enabled = false;
            Confirm_Button.Enabled = false;
            Question_Label.Visible = true;
            Question_Textbox.Visible = true;
            Add_Button.Visible = true;
            Done_Button.Visible = true;
        }

        //Adds the created question to the database with the given role and title values
        protected void AddButton_Click(Object source, EventArgs args)
        {
            FeedbackItem created = new FeedbackItem();
            created.Question = Question_Textbox.Text.Trim();
            created.Role = role;
            title = Title_Textbox.Text;
            created.Title = title;

            //load question and fetch based on role

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand newFeedbackComm = new SqlCommand("INSERT INTO Feedback (Question, Role, Total, Count, Title, Average) " +
                "VALUES(@question, @role, @total, @count, @title, @average)");
            //parameterization
            newFeedbackComm.Parameters.Add("@question", SqlDbType.VarChar);
            newFeedbackComm.Parameters.Add("@role", SqlDbType.VarChar);
            newFeedbackComm.Parameters.Add("@total", SqlDbType.Int);
            newFeedbackComm.Parameters.Add("@count", SqlDbType.Int);
            newFeedbackComm.Parameters.Add("@title", SqlDbType.VarChar);
            newFeedbackComm.Parameters.Add("@average", SqlDbType.Int);
            // set the parameters
            newFeedbackComm.Parameters["@question"].Value = created.Question;
            newFeedbackComm.Parameters["@role"].Value = created.Role;
            newFeedbackComm.Parameters["@total"].Value = created.Total;
            newFeedbackComm.Parameters["@count"].Value = created.Count;
            newFeedbackComm.Parameters["@title"].Value = created.Title;
            newFeedbackComm.Parameters["@average"].Value = 0;

            newFeedbackComm.Connection = conn;
            conn.Open();

            using (conn)
            {
                newFeedbackComm.ExecuteNonQuery();
            }

            conn.Close();
            
            //Shows the questions that have been added to the current feedback form
            if (DatabaseUtilities.CountFeedback(role, title) != 0)
            {
                int counter = 1;
                foreach (FeedbackItem a in DatabaseUtilities.GetFeedback(role, title))
                {
                    string qID = "Q" + (counter).ToString();
                    string rID = "R" + (counter).ToString();
                    TableRow qRow = QuestionTable.FindControl(rID) as TableRow;
                    Label qLabel = qRow.FindControl(qID) as Label;
                    qLabel.Text = "<h5 style='width:25%'>" + a.Question + "</h4>";
                    qRow.Visible = true;
                    counter++;
                }
            }

            //Disables the Question textbox and Add button if the maximum amount of questions for the form has been reached
            if (DatabaseUtilities.CountFeedback(role, title) == 10)
            {
                Question_Textbox.Enabled = false;
                Add_Button.Enabled = false;
                Question_Textbox.Text = "Maximum amount of questions has been reached";
            }
            else
            {
                Question_Textbox.Text = "";
                Question_Textbox.Focus();
            }

        }

        //Removes the related question from the current feedback form
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            LinkButton delButton = sender as LinkButton;
            string dID = delButton.ID;
            string qID = "Q" + dID.Substring(1, dID.Length-1);
            string rID = "R" + dID.Substring(1, dID.Length-1);
            TableRow qRow = QuestionTable.FindControl(rID) as TableRow;
            Label qLabel = qRow.FindControl(qID) as Label;
            string question = qLabel.Text.Substring(22, qLabel.Text.Length-27);
            title = Title_Textbox.Text;

            qRow.Visible = false;
            using (var _db = new MPAS.Models.ApplicationDbContext())
            {
                _db.Database.ExecuteSqlCommand("DELETE FROM Feedback WHERE Question = @p0 AND Role = @p1 AND Title = @p2", question, role, title);
            }

            //Enables the Question textbox and Add button if the maximum amount of questions had been reached before the deletion
            if (DatabaseUtilities.CountFeedback(role, title) == 9)
            {
                Question_Textbox.Enabled = true;
                Add_Button.Enabled = true;
                Question_Textbox.Text = "";
            }
        }

        //Makes the created feedback form available to the selected audience (mentees or mentors)
        protected void DoneButton_Click(Object source, EventArgs args)
        {
            int roleInt = 1;
            if (role == "Mentee")
            {
                roleInt = 0;
            }
      
            using (var _db = new MPAS.Models.ApplicationDbContext())
            {
                _db.Database.ExecuteSqlCommand("UPDATE ProfileDetails SET Feedback = 0 WHERE Role = @p0", roleInt);
            }
   
            Response.Redirect("~/Feedback.aspx");
        }

    }
}