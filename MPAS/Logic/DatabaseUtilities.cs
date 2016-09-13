using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MPAS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MPAS.Logic
{
    /*
     * Class to handle all database accesses 
     */
    public class DatabaseUtilities
    {
        /*
         * Adds the given details for a meeting to the meeting database
         */
        public static void AddMeeting(string title, string location, string agenda, DateTime start, DateTime end, int groupNumber, string madeByStdNum)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getUserComm = new SqlCommand("INSERT INTO Meetings (Title, Agenda, Location, StartTime, EndTime, MadeBy, GroupNumber) " +
                " values(@title, @agenda, @location, @starttime, @endtime, @madeby, @groupNumber)");
            // set the parameters
            getUserComm.Parameters.Add("@title", SqlDbType.VarChar);
            getUserComm.Parameters["@title"].Value = title;
            getUserComm.Parameters.Add("@agenda", SqlDbType.VarChar);
            getUserComm.Parameters["@agenda"].Value = agenda;
            getUserComm.Parameters.Add("@location", SqlDbType.VarChar);
            getUserComm.Parameters["@location"].Value = location;
            getUserComm.Parameters.Add("@starttime", SqlDbType.DateTime);
            getUserComm.Parameters["@starttime"].Value = start;
            getUserComm.Parameters.Add("@endtime", SqlDbType.DateTime);
            getUserComm.Parameters["@endtime"].Value = end;
            getUserComm.Parameters.Add("@madeby", SqlDbType.VarChar);
            getUserComm.Parameters["@madeby"].Value = madeByStdNum;
            getUserComm.Parameters.Add("@groupNumber", SqlDbType.Int);
            getUserComm.Parameters["@groupNumber"].Value = groupNumber;

            getUserComm.Connection = conn;
            conn.Open();

            using (conn)
            {
                getUserComm.ExecuteNonQuery();
            }
        }

        /*
         * Determines the highest group number currently assigned
         */
        public static int GetHighestGroupNumber ()
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getUserComm = new SqlCommand("SELECT MAX(GroupNumber)" +
                " FROM ProfileDetails" );
            getUserComm.Connection = conn;
            conn.Open();

            int groupNumber = 0;
            using (conn)
            using (var reader = getUserComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // read the record
                    reader.Read();
                    groupNumber = reader.GetInt32(0);
                }
            }
            return groupNumber;
        }
        
        /*
         * Creates a User object for the user with the given student number
         */
        public static User GetUser( string studentNumber)
        {
            // special case for the singleton admin class
            if (studentNumber == "01360406") return Administrator.Get();

            User u = null;
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getUserComm = new SqlCommand("SELECT StudentNumber, FirstName, Surname, DateOfBirth, Role, GroupNumber " +
                " FROM ProfileDetails WHERE StudentNumber='" + studentNumber + "'");
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
                    u.GroupNumber = (!reader.IsDBNull(4)) ? reader.GetInt32(4) : 0;
                }
            }
            return u;
        }

        /*
         * Generate and populate a list of meetings for the group 
         * with the given group number
         */
        public static List<Meeting> GetMeetingsForGroup(int groupNumber)
        {
            List<Meeting> meetings = new List<Meeting>();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getUserComm = new SqlCommand("SELECT Id, Title, Agenda, Location, StartTime, EndTime, MadeBy, GroupNumber " +
                " FROM Meetings WHERE GroupNumber='" + groupNumber+ "' ORDER BY StartTime");
            getUserComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = getUserComm.ExecuteReader())
            {
                if (reader.HasRows)
                { 
                    while(reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        String title = reader.GetString(1);
                        String agenda = reader.GetString(2);
                        String location = reader.GetString(3);
                        DateTime startTime = reader.GetDateTime(4);
                        DateTime endTime = reader.GetDateTime(5);
                        String madeby = reader.GetString(6);
                        int groupNum = reader.GetInt32(7);
                        MentorGroup group = MentorGroupManager.GetGroup(groupNum);
                        meetings.Add(new Meeting(ID, group, title, location, agenda, startTime, endTime));
                    }

                }
            }
            return meetings;
        }

        /*
         * Reads a group's details from the database
         * returns null if the group DNE
         * Preferable to use MentorGroupManager.GetGroup to get a group
         */
        public static MentorGroup GetGroup(int groupNumber)
        {
            MentorGroup g = null;
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getUserComm = new SqlCommand("SELECT GroupNumber, Mentor FROM Groups WHERE GroupNumber='" + groupNumber + "'");
            getUserComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = getUserComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    g = new MentorGroup();
                    g.Id = reader.GetInt32(0);
                    g.Mentor = (Mentor)GetUser(reader.GetString(1));
                }
            }

            return g;
        }

        /*
         * Generates and populates a list of Announcements that are directed at the group with
         * the given group number
         */
        public static List<Announcement> GetAnnouncements(int groupNumber)
        {
            List<Announcement> announcements = new List<Announcement>();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand titleComm = new SqlCommand("SELECT Id, Title, MadeBy, Date, Content FROM Announcements WHERE GroupNumber='0' OR GroupNumber='" + groupNumber + "' ORDER BY Date DESC");
            titleComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = titleComm.ExecuteReader())
            {
                while(reader.Read())
                {
                    // create and populate the announcement
                    Announcement a = new Announcement(reader.GetInt32(0));
                    a.Title = reader.GetString(1);
                    a.MadeBy = GetUser(reader.GetString(2));
                    if (a.MadeBy == null)
                    {
                        a.MadeBy = Mentor.NULL;
                    }
                    a.CreationDate = reader.GetDateTime(3);
                    a.Content = reader.GetString(4);
                    announcements.Add(a);
                }

            }
            return announcements;
        }

        /*
         * Returns an announcement object representing the announcement with the given ID
         */
        public static Announcement GetAnnouncementById(int id)
        {
            Announcement announcement = new Announcement(id);

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand announcementComm = new SqlCommand("SELECT Title, Date, Content, GroupNumber FROM Announcements WHERE Id = @id");
            announcementComm.Parameters.Add("@id", SqlDbType.Int);
            announcementComm.Parameters["@id"].Value = id;
            announcementComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = announcementComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    announcement.Title = reader.GetString(0);
                    announcement.CreationDate = reader.GetDateTime(1);
                    announcement.Content = reader.GetString(2);
                    int groupId = reader.GetInt32(3);
                    announcement.Group = groupId == 0 ? MentorGroup.GENERAL : new MentorGroup() { Id = groupId };
                }
                else
                {
                    // fields set to inform reader that the announcement does not exist
                    announcement.Title = "Announcement " + id + " not found";
                    announcement.CreationDate = DateTime.Now;
                    announcement.Content = "";
                    announcement.Group = MentorGroup.GENERAL;
                }
            }

            return announcement;
        }

        public static Meeting GetMeetingbyId(int id)
        {
            Meeting m = null;

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand meetingComm = new SqlCommand("SELECT Title, Agenda, Location, StartTime, EndTime, GroupNumber FROM Meetings WHERE Id = @id");
            meetingComm.Parameters.Add("@id", SqlDbType.Int);
            meetingComm.Parameters["@id"].Value = id;
            meetingComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = meetingComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    // read the elements of the meeting from the 
                    string title = reader.GetString(0);
                    string agenda = reader.GetString(1);
                    string location = reader.GetString(2);
                    DateTime startTime = reader.GetDateTime(3);
                    DateTime endTime = reader.GetDateTime(4);
                    int groupNum = reader.GetInt32(5);
                    m = new Meeting(id, MentorGroupManager.GetGroup(groupNum), title, location, agenda, startTime, endTime);
                }
                else
                {
                    // fields set to inform reader that the meeting does not exist
                    m = new Meeting(MentorGroup.GENERAL, "Meeting not found", "", "", DateTime.Now, DateTime.Now);
                }
            }

            return m;
        }

        public static bool AddUser(string studentNumber, string firstName, string surname, bool isMentor)
        {
            bool result = false;

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
                UserName = studentNumber,
            };
            string password = MakeRandomPassword();
            IdUserResult = userMgr.Create(appUser, password);

            if (IdUserResult.Succeeded)
            {
                IdUserResult = userMgr.AddToRole(appUser.Id, "User");

                int role = (isMentor) ? 1 : 0;
                // send the user their new login details
                EmailUtilities.Email(studentNumber + "@myuct.ac.za", "[MPAS] New User Login Details", "Hey " + firstName + " " + surname + "\n\n" + "Username: " + studentNumber + "\nPassword: " + password + "\n\n Thanks," + "\nMPAS Team");

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
                        result = true;
                        using (var _db = new MPAS.Models.ApplicationDbContext())
                        {
                            _db.Database.ExecuteSqlCommand("INSERT INTO ProfileDetails (StudentNumber, FirstName, Surname, Role) " +
                                    "VALUES (@p0, @p1, @p2, @p3)", studentNumber, firstName, surname, role);
                        }
                    }
                }
            }

            return result;
        }

        public static string MakeRandomPassword()
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

        /*
         * Remove a user from the user database
         */
        public static bool DeleteUser(string studentNumber)
        {
            bool result = false;
            using (var _db = new MPAS.Models.ApplicationDbContext())
            {
                var myItem = (from c in _db.Users where c.UserName == studentNumber select c).FirstOrDefault();
                if (myItem != null)
                {
                    _db.Users.Remove(myItem);
                    _db.SaveChanges();

                    _db.Database.ExecuteSqlCommand("DELETE FROM ProfileDetails WHERE StudentNumber=@p0", studentNumber);

                    // indicate success
                    result = true;

                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        /*
         * Creates and populates a MPAS.Models.Chatroom object with data from the database
         * For efficiency reasons, it is preferable to use ChatroomManager.GetChatroom(groupNumber)
         */
        public static Chatroom GetChatroom(int groupNumber)
        {
            Chatroom cr = new Chatroom(MentorGroupManager.GetGroup(groupNumber));

            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            // the command to be executed
            SqlCommand chatroomComm = new SqlCommand("SELECT Sender, Message, TimeStamp FROM Chatrooms WHERE GroupNumber=@groupNumber ORDER BY TimeStamp DESC");
            // set the parameters
            chatroomComm.Parameters.Add("@groupNumber", SqlDbType.Int);
            chatroomComm.Parameters["@groupNumber"].Value = groupNumber;
            chatroomComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = chatroomComm.ExecuteReader())
            {
                while (reader.Read()) { 
                    // create a message with the details of this row
                    ChatMessage m = new ChatMessage(cr);
                    m.Source = GetUser(reader.GetString(0));
                    m.MessageContent = reader.GetString(1);
                    m.SendTime = reader.GetDateTime(2);
                    // add the message to the chat room
                    cr.Receive(m);
                }
            }

            return cr;

        }

        /*
         * Adds the sent chatroom message to the Chatrooms database
         */
        public static void SendChatroomMessage(int groupNumber, String srcStudentNumber, String message, DateTime sendTime)
        {
            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            // the command to be executed
            SqlCommand chatroomComm = new SqlCommand("INSERT INTO Chatrooms (GroupNumber, Sender, Message, TimeStamp) " + 
                "values(@groupNumber, @srcStudentNumber, @message, @sendTime)");
            // set the parameters
            chatroomComm.Parameters.Add("groupNumber", SqlDbType.Int);
            chatroomComm.Parameters.Add("srcStudentNumber", SqlDbType.VarChar);
            chatroomComm.Parameters.Add("message", SqlDbType.VarChar);
            chatroomComm.Parameters.Add("sendTime", SqlDbType.DateTime);

            chatroomComm.Parameters["@groupNumber"].Value = groupNumber;
            chatroomComm.Parameters["@srcStudentNumber"].Value = srcStudentNumber;
            chatroomComm.Parameters["@message"].Value = message;
            chatroomComm.Parameters["@sendTime"].Value = sendTime;
            chatroomComm.Connection = conn;
            conn.Open();

            using (conn)
            {
                chatroomComm.ExecuteNonQuery();
            }

        }
    }
}