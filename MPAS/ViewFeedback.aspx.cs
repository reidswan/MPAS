using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Logic;
using MPAS.Models;
using System.Web.UI.DataVisualization;
using System.Web.UI.DataVisualization.Charting;

namespace MPAS
{
    public partial class ViewFeedback : System.Web.UI.Page
    {
        GraphLogicLayer l1 = new GraphLogicLayer();
        private string title = "";

        //Displays the feedback questions for the selected form and their results
        protected void Page_Load(object sender, EventArgs e)
        {
            string role = Request.QueryString["role"];
            title = Request.QueryString["title"];
            FeedbackItem created = new FeedbackItem();
            TitleLabel.Text = "<h2>" + role + " Feedback - " + title + "</h2>";

            if (DatabaseUtilities.CountFeedback(role, title) != 0)
            {
                int counter = 1;
                foreach (FeedbackItem a in DatabaseUtilities.GetFeedback(role, title))
                {
                    string qID = "Q" + (counter).ToString();
                    string rID = "R" + (counter).ToString();
                    string aID = "A" + (counter).ToString();

                    TableRow qRow = FeedbackTable.FindControl(rID) as TableRow;

                    Label qLabel = qRow.FindControl(qID) as Label;
                    qLabel.Text = "<h4 style='width:25%'>" + a.Question + "</h4>";

                    float feedbackAverage;
                    if (a.Count == 0)
                    {
                        feedbackAverage = 0;
                    }
                    else
                    {
                        feedbackAverage = a.Total / a.Count;
                    }
                    string stringAverage = feedbackAverage.ToString("0.00");
                    //Update the average values of each question
                    DatabaseUtilities.UpdateAverageFeedback(role, title,a.Question, feedbackAverage);
                    Label aLabel = qRow.FindControl(aID) as Label;
                    aLabel.Text = "<h5 style='width:25%'>" + stringAverage + "</h4>";

                    qRow.Visible = true;
                    counter++;
                }
            }
            //comment box
            //Displays additional comments from mentors for the form if they exist
            if (DatabaseUtilities.CountFeedbackComments(title) != 0)
            {
                if(FeedbackList.Items.Count==0)
                {
                    foreach (string fbSNum in DatabaseUtilities.GetFeedbackSNum(title))
                    {
                        FeedbackList.Items.Add(fbSNum);
                    }
                }
                
                ViewLabel.Visible = true;
                FeedbackList.Visible = true;
                ViewComment.Text = "\"" + DatabaseUtilities.GetFeedbackComment(title, FeedbackList.SelectedItem.Text) +"\"";
                ViewComment.Visible = true;
            }
            //Chart 
            if (!IsPostBack)
            {
                FillChartControl(title);
            }
            
        }

        //Displays the additional comment of the selected mentor in the dropdown list
        protected void FeedbackList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewComment.Text = "\"" + DatabaseUtilities.GetFeedbackComment(title, FeedbackList.SelectedItem.Text) + "\"";
            ViewComment.Visible = true;
        }

        private void FillChartControl(string title)
        {
            Chart1.DataSource = l1.SelectFeedbacks(title);
            Chart1.DataBind();
            //Change the label to the total average of the entire series
            AverageLabel_Display.Text = Chart1.DataManipulator.Statistics.Mean("Series1").ToString();
            //Max value of y is 5 as 5 is the highest answer value
            Chart1.ChartAreas[0].AxisY.Maximum = 5;
            //If selected value is Ascending
            if (ddlSortDirection.SelectedValue =="ASC")
            {
                Chart1.Series["Series1"].Sort(System.Web.UI.DataVisualization.Charting.PointSortOrder.Ascending, ddlSortBy.SelectedValue);
            }
            //else make it descending
            else
            {
                Chart1.Series["Series1"].Sort(System.Web.UI.DataVisualization.Charting.PointSortOrder.Descending, ddlSortBy.SelectedValue);

            }
        }

        //Method - when the sort by Question/Average Value drop down list's index has changed, call the FillChartControl method
        protected void ddlSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillChartControl(title);
        }
        //Method - when the sort by ASC/DESC drop down list's index has changed, call the FillChartControl method
        protected void ddlSortDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillChartControl(title);
        }
        /*
        protected void GetChartTypes()
        {
            foreach(int chartType in Enum.GetValues(typeof(SeriesChartType)))
            {
                ListItem li = new ListItem(Enum.GetName(typeof(SeriesChartType),chartType),chartType.ToString());
                ChartTypeList.Items.Add(li);
            }
        }
        
        protected void ChartTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Chart1.Series["Series1"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ChartTypeList.SelectedValue);
        }
        */

    }
}