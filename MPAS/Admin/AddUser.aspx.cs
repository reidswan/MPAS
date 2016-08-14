using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Models;
using MPAS.Logic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

namespace MPAS.Admin
{
    public partial class AddUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            if(!this.User.IsInRole("Administrator"))
            {
                Response.Redirect("~/Error/AuthError.aspx");
            }
        }

        protected void SubmitButton_Clicked(object sender, EventArgs e)
        {
            if (this.IsValid) CreateUser();
        }

        public void CreateUser()
        {

            // access database and hold results
            Models.ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult IdRoleResult;
            IdentityResult IdUserResult;

            // Create a RoleStore object by using the ApplicationDbContext object.
            // The RoleStore is only allowed to contain IdentityRole objects.
            var roleStore = new RoleStore<IdentityRole>(context);

            // Create a RoleManager object that is only allowed to contain IdentityRole objects
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            // create the new user
            IdRoleResult = roleMgr.Create(new IdentityRole("User"));
            if (!IdRoleResult.Succeeded)
            {
                // Handle the error condition if there's a problem creating the RoleManager object.
            }

            // Create a UserManager object based on the UserStore object and the ApplicationDbContext object.
            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var appUser = new ApplicationUser()
            {
                UserName = StudentNumber_TextBox.Text,
            };
            string password = MakeRandomPassword();
            IdUserResult = userMgr.Create(appUser, password);
            
            if (IdUserResult.Succeeded)
            {
                IdUserResult = userMgr.AddToRole(appUser.Id, "User");
                
                string studentNumber = StudentNumber_TextBox.Text;
                string firstName = FirstName_TextBox.Text;
                string surname = Surname_TextBox.Text;
                int role = (MentorCheckBox.Checked) ? 1 : 0;
                // send the user their new login details
                EmailUtilities.Email(studentNumber + "@myuct.ac.za", "[MPAS] New User Login Details","Hey "+firstName+" "+surname+"\n\n"+"Username: " + StudentNumber_TextBox.Text + "\nPassword: " + password+"\n\n Thanks,"+"\nMPAS Team");

                User u = null;
                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                SqlCommand getUserComm = new SqlCommand("SELECT * FROM ProfileDetails WHERE StudentNumber=@studentNumber");
                // set the parameters
                getUserComm.Parameters.Add("@studentNumber", SqlDbType.VarChar);
                getUserComm.Parameters["@studentNumber"].Value = studentNumber;
                getUserComm.Connection = conn;
                conn.Open();

                using (conn)
                using (var reader = getUserComm.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        using (var _db = new MPAS.Models.ApplicationDbContext())
                        {
                            _db.Database.ExecuteSqlCommand("INSERT INTO ProfileDetails (StudentNumber, FirstName, Surname, Role) " +
                                    "VALUES (@p0, @p1, @p2, @p3)", studentNumber, firstName, surname, role);
                        }
                    } else
                    {
                        Warning.Text = "User already exists";
                        Warning.ForeColor = System.Drawing.Color.Red;
                    }
                }
                /*ApplicationDbContext db = ApplicationDbContext.Create();
                using (var _db = new MPAS.Models.ApplicationDbContext())
                {
                    _db.Database.ExecuteSqlCommand("INSERT INTO ProfileDetails (StudentNumber, FirstName, Surname, Role) " + 
                        "VALUES (@p0, @p1, @p2, @p3)", studentNumber, firstName, surname, role);
                }*/

                // indicate that the account was created successfully
                Status_Label.Text = firstName + " " + surname + " was added successfully";
                Status_Label.ForeColor = System.Drawing.Color.Green;
                Status_Label.Visible = true;

                // remove existing info in textboxes
                StudentNumber_TextBox.Text = "";
                FirstName_TextBox.Text = "";
                Surname_TextBox.Text = "";
                MentorCheckBox.Checked = false;
            }
            else
            {
                // indicate that account creation failed
                Status_Label.Text = "Failed to add user: " + IdUserResult.Errors.First().ToString();
                Status_Label.ForeColor = System.Drawing.Color.Red;
                Status_Label.Visible = true;
            }

        }

        public string MakeRandomPassword()
        {
            string pss = "";
            Random r = new Random();
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            alpha += alpha.ToUpper();
            alpha += "0123456789";
            for (int i = 0; i < 8; i++)
            {
                pss += alpha[r.Next(0, alpha.Length)];
            }
            return pss;
        }

        protected void StudentNumberValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (Regex.IsMatch(args.Value, @"^[a-zA-Z]{6,6}[0-9]{3,3}$") // student number
                || Regex.IsMatch(args.Value, @"^[0-9]{8,8}$") // staff number
                );
        }

        protected void NameValidate(object source, ServerValidateEventArgs args)
        {
            // non-numeric name of length at least 1
            args.IsValid = (Regex.IsMatch(args.Value, @"^[^(1-9)]+$"));
        }
    }
}