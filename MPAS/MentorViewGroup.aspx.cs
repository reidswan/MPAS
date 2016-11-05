using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using MPAS.Models;
using System.Data.SqlClient;
using MPAS.Logic;
using System.IO;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;


namespace MPAS
{
    public partial class MentorViewGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            displayImages();
        }
        protected void displayImages()
        {
            string currentUser = this.User.Identity.Name;
            User u = DatabaseUtilities.GetUser(currentUser);

            if (u.GroupNumber == 0) //admin - view everyone
            {
                MentorGroup_Label.InnerText = "All Mentors & Mentees ";
                System.Diagnostics.Debug.WriteLine(u.StudentNumber);
                List<User> listOfEveryone = DatabaseUtilities.GetAllMenteePictures(currentUser, u.GroupNumber);
                TableRow r = new TableRow();
                int count = 0;
                foreach (User y in listOfEveryone)
                {

                    
                    if (count > 3)
                    {
                        MenteeTable.Controls.Add(r);
                        r = new TableRow();
                        count = 0;
                    }
                    TableCell c = new TableCell();
                    c.BorderWidth = 0;
                    c.Text = @" <div class='row'><li class='caption-style-1'><img src = '/Images/" + y.PathToPicture + @"' alt ='Missing Image' style = 'width: 200px; height: 200px; </li>'></div>";
                    c.Text += @"<div class='row' style='font-size:20px;width:200px;height:200px;text-align:center'>" + y.FirstName + @" " + y.Surname + @"</div>";
                    // c.Text += @"<div class='row'>" + y.Surname + @"</div>";
                    r.Controls.Add(c);
                    count++;

                }
                if (!MenteeTable.Controls.Contains(r))
                {
                    MenteeTable.Controls.Add(r);
                }
            }
            else
            {
                //if its a normal group number above 0. For example, 1, 2, 3, 4 
                if (u.GroupNumber > 0)
                {
                    MentorGroup_Label.InnerText = "Mentor Group - " + u.GroupNumber;
                    List<User> listOfMentees = DatabaseUtilities.GetMenteePictures(currentUser, u.GroupNumber);
                    TableRow r = new TableRow();
                    int count = 0;
                    foreach (User y in listOfMentees)
                    {
                        count++;
                        if (count > 3)
                        {
                            MenteeTable.Controls.Add(r);
                            r = new TableRow();
                            count = 0;
                        }
                        TableCell c = new TableCell();
                        c.BorderWidth = 0;
                        c.Text = @" <div class='row'><li class='caption-style-1'><img src = '/Images/" + y.PathToPicture + @"' alt ='Missing Image' style = 'width: 200px; height: 200px; </li>'></div>";
                        c.Text += @"<div class='row' style='font-size:20px;width:200px;height:200px;text-align:center'>" + y.FirstName + @" " + y.Surname + @"</div>";
                        // c.Text += @"<div class='row'>" + y.Surname + @"</div>";
                        r.Controls.Add(c);


                    }
                    if (count != 0)
                    {
                        MenteeTable.Controls.Add(r);
                    }
                }
                else
                //Else the value is less than 0 which is -1 etc. Shows nothing .
                {
                    MentorGroup_Label.InnerText = "Mentor Group - Not Assigned";

                }
            }
        }


    }
}