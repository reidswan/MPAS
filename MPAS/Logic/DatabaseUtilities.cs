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
            conn.Close();
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
            conn.Close();
            return groupNumber;
        }
        
        /*
         * Creates a User object for the user with the given student number
         */
        public static User GetUser( string studentNumber)
        {
            // special case for the singleton admin class
            if (studentNumber == "01360406") return Administrator.Get();

            User u = new Mentee("NULL");
            u.FirstName = "Missing/Deleted";
            u.Surname = "";
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getUserComm = new SqlCommand("SELECT StudentNumber, FirstName, Surname, DateOfBirth, Role, GroupNumber, Feedback, PictureURL" +
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
                    u.GroupNumber = (!reader.IsDBNull(5)) ? reader.GetInt32(5) : 0;
					u.Feedback = (!reader.IsDBNull(6)) ? reader.GetInt32(6) : 0;
					u.PathToPicture = reader.IsDBNull(7) ? "default.jpg" : reader.GetString(7);
                }
            }
            conn.Close();
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
                " FROM Meetings WHERE GroupNumber=@groupNumber ORDER BY StartTime ASC");
            getUserComm.Parameters.Add("@groupNumber", SqlDbType.Int);
            getUserComm.Parameters["@groupNumber"].Value = groupNumber;
            getUserComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = getUserComm.ExecuteReader())
            {
                while (reader.Read())
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
                    Meeting m = new Meeting(ID, group, title, location, agenda, startTime, endTime);
                    m.MadeBy = GetUser(madeby);
                    meetings.Add(m);
                }
            }
            conn.Close();
            return meetings;
        }

        /*
         * Reads a group's details from the database
         * returns null if the group DNE
         * Preferable to use MentorGroupManager.GetGroup to get a group
         */
        public static MentorGroup GetGroup(int groupNumber)
        {
            MentorGroup g = new MentorGroup();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getUserComm = new SqlCommand("SELECT GroupNumber, Mentor FROM Groups WHERE GroupNumber=@groupNumber");
            getUserComm.Parameters.Add("@groupNumber", SqlDbType.Int);
            getUserComm.Parameters["@groupNumber"].Value = groupNumber;
            getUserComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = getUserComm.ExecuteReader())
            {
                while (reader.Read())
                {
                    g = new MentorGroup();
                    g.Id = reader.GetInt32(0);
                    g.Mentor = (Mentor)GetUser(reader.GetString(1));
                }
            }
            conn.Close();

            // fill the group with mentees
            conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand menteesForGroupComm = new SqlCommand("SELECT StudentNumber FROM ProfileDetails WHERE GroupNumber=@groupNumber");// AND Role=0");
            menteesForGroupComm.Parameters.Add("@groupNumber", SqlDbType.Int);
            menteesForGroupComm.Parameters["@groupNumber"].Value = groupNumber;
            menteesForGroupComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = menteesForGroupComm.ExecuteReader())
            {
                while (reader.Read())
                {
                    var u = GetUser(reader.GetString(0));
                    if (u is Mentee)
                    {
                        g.AddMentee((Mentee)u);
                    }
                }
            }

            conn.Close();
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
            conn.Close();
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
            conn.Close();
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
            conn.Close();
            return m;
        }

        public static bool AddUser(string studentNumber, string firstName, string surname, bool isMentor)
        {
            bool result = false;
			int feedback = 1; //1 means that Feedback has been completed and the tab is not visible
			int defaultGroupNumber = -1;
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

                string defaultImagePath = "default.jpg";

                using (conn)
                using (var reader = getUserComm.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        result = true;
                        using (var _db = new MPAS.Models.ApplicationDbContext())
                        {
                           _db.Database.ExecuteSqlCommand("INSERT INTO ProfileDetails (StudentNumber, FirstName, Surname, Role,GroupNumber, PictureURL, Feedback) " +
                                    "VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)", studentNumber, firstName, surname, role, defaultGroupNumber, defaultImagePath, feedback);
                        }
                    }
                }
                conn.Close();
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
            SqlCommand chatroomComm = new SqlCommand("SELECT Sender, Message, TimeStamp FROM Chatrooms WHERE GroupNumber=@groupNumber ORDER BY TimeStamp ASC");
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
                    cr.AddMessage(m);
                }
            }

            conn.Close();
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
            chatroomComm.Parameters.Add("@groupNumber", SqlDbType.Int);
            chatroomComm.Parameters.Add("@srcStudentNumber", SqlDbType.VarChar);
            chatroomComm.Parameters.Add("@message", SqlDbType.VarChar);
            chatroomComm.Parameters.Add("@sendTime", SqlDbType.DateTime);

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
            conn.Close();
        }

        /**
         * Returns a list of Users that have sent messages to the user with the given student number
         */
        public static List<User> GetSendersToUser(string stdNum)
        {
            List<User> sendersToUser = new List<User>();
            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            // the command to be executed
            SqlCommand msgComm = new SqlCommand("SELECT DISTINCT Source FROM Messages WHERE Destination=@studentNumber");
            // set the parameters
            msgComm.Parameters.Add("@studentNumber", SqlDbType.VarChar);
            msgComm.Parameters["@studentNumber"].Value = stdNum;
            msgComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = msgComm.ExecuteReader())
            {
                while (reader.Read())
                {
                    sendersToUser.Add(GetUser(reader.GetString(0)));
                }
            }

            conn.Close();
            return sendersToUser;
        }

        /**
         * Queries all messages directed at the user with the given student number and returns them
         */
        public static List<PrivateMessage> GetMessagesForUser(string username)
        {
            List<PrivateMessage> messages = new List<PrivateMessage>();

            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            // the command to be executed
            SqlCommand msgComm = new SqlCommand("SELECT Source, Destination, SendTime, MessageContent, IsRead FROM Messages WHERE Destination=@studentNumber ORDER BY SendTime ASC");
            // set the parameters
            msgComm.Parameters.Add("@studentNumber", SqlDbType.VarChar);
            msgComm.Parameters["@studentNumber"].Value = username;
            msgComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = msgComm.ExecuteReader())
            {
                while (reader.Read())
                {
                    // create a message with the details of this row
                    PrivateMessage m = new PrivateMessage();
                    m.Source = GetUser(reader.GetString(0));
                    m.Destination = GetUser(reader.GetString(1));
                    m.SendTime = reader.GetDateTime(2);
                    m.MessageContent = reader.GetString(3);
                    m.Read = reader.IsDBNull(4) ? true : reader.GetBoolean(4);
                    messages.Add(m);
                }
            }

            conn.Close();
            return messages;
        }

        /**
         * Returns a list of private messages between users with the given student numbers
         */
        public static List<PrivateMessage> GetMessagesBetweenUsers(string stdNum1, string stdNum2)
        {
            List<PrivateMessage> messages = new List<PrivateMessage>();
            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            // the command to be executed
            SqlCommand msgComm = new SqlCommand("SELECT Source, Destination, SendTime, MessageContent, IsRead FROM Messages " + 
                "WHERE (Destination=@studentNumber1 AND Source=@studentNumber2) OR (Source=@studentNumber1 AND Destination=@studentNumber2) " +
                "ORDER BY SendTime ASC");
            // set the parameters
            msgComm.Parameters.Add("@studentNumber1", SqlDbType.VarChar);
            msgComm.Parameters.Add("@studentNumber2", SqlDbType.VarChar);
            msgComm.Parameters["@studentNumber1"].Value = stdNum1;
            msgComm.Parameters["@studentNumber2"].Value = stdNum2;
            msgComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = msgComm.ExecuteReader())
            {
                while (reader.Read())
                {
                    // create a message with the details of this row
                    PrivateMessage m = new PrivateMessage();
                    m.Source = GetUser(reader.GetString(0));
                    m.Destination = GetUser(reader.GetString(1));
                    m.SendTime = reader.GetDateTime(2);
                    m.MessageContent = reader.GetString(3);
                    m.Read = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                    messages.Add(m);
                }
            }
            conn.Close();
            return messages;
        }

        /**
         * Use PrivateChatManager.GetChat instead
         * returns a private chat room between the given users containing all previous messages 
         */
        public static PrivateChat GetPrivateChat(string stdNum1, string stdNum2)
        {
            PrivateChat pChat = new PrivateChat(GetUser(stdNum1), GetUser(stdNum2));
            foreach(PrivateMessage m in GetMessagesBetweenUsers(stdNum1, stdNum2))
            {
                pChat.AddMessage(m);
            }
            return pChat;
        }

        /*
         * Adds the sent chatroom message to the Chatrooms database
         */
        public static void SendPrivateMessage(string sourceStdNum, String destStdNum, String message, DateTime sendTime)
        {
            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            // the command to be executed
            SqlCommand messagesComm = new SqlCommand("INSERT INTO Messages (Source, Destination, MessageContent, SendTime) " +
                "values(@source, @dest, @message, @sendTime)");
            // set the parameters
            messagesComm.Parameters.Add("@source", SqlDbType.VarChar);
            messagesComm.Parameters.Add("@dest", SqlDbType.VarChar);
            messagesComm.Parameters.Add("@message", SqlDbType.VarChar);
            messagesComm.Parameters.Add("@sendTime", SqlDbType.DateTime);

            messagesComm.Parameters["@source"].Value = sourceStdNum;
            messagesComm.Parameters["@dest"].Value = destStdNum;
            messagesComm.Parameters["@message"].Value = message;
            messagesComm.Parameters["@sendTime"].Value = sendTime;
            messagesComm.Connection = conn;
            conn.Open();

            using (conn)
            {
                messagesComm.ExecuteNonQuery();
            }
            conn.Close();
        }

        /**
         * Returns a list of strings containing Name, Surname and Student Number of all users
         */
         public static List<string> GetAllUserStrings()
        {
            List<string> userStrings = new List<string>();
            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            // the command to be executed
            SqlCommand userComm = new SqlCommand("SELECT StudentNumber, FirstName, Surname FROM ProfileDetails");
            userComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = userComm.ExecuteReader())
            {
                while (reader.Read())
                {
                    userStrings.Add(reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(2));
                }
            }
            conn.Close();

            return userStrings;
        }

        /**
         * Returns a List object containing the periods in which a user is free;
         * this takes the form of a list of tuples of the form (day [0 to 4], period number [0,9])
         */
        public static List<Tuple<int, int>> GetFreePeriodsForUser(string studentNum)
        {
            List<Tuple<int, int>> freePeriods = new List<Tuple<int, int>>();

            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            // the command to be executed
            SqlCommand scheduleComm = new SqlCommand("SELECT Monday, Tuesday, Wednesday, Thursday, Friday FROM Schedule " +
                "WHERE StudentNumber=@studentNumber");
            // set the parameters
            scheduleComm.Parameters.Add("@studentNumber", SqlDbType.VarChar);
            scheduleComm.Parameters["@studentNumber"].Value = studentNum;
            scheduleComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = scheduleComm.ExecuteReader())
            {
                while (reader.Read())
                {
                    string[] days = new string[5];
                    for(int i = 0; i < 5; i++)
                    {
                        days[i] = reader.GetString(i);
                        for(int j = 0; j < 10; j++)
                        {
                            if(days[i].Contains(""+j))
                            {
                                freePeriods.Add(new Tuple<int, int>(i, j));
                            }
                        }
                    }
                }
            }
            conn.Close();
            return freePeriods;
        }

        /**
         * Gets a list of the student numbers of all mentors
         */
         public static List<string> GetMentorStudentNumbers()
        {
            List<string> mentorStdNums = new List<string>();

            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            // the command to be executed
            SqlCommand stdNumComm = new SqlCommand("SELECT StudentNumber FROM ProfileDetails " +
                "WHERE Role='1'");
            stdNumComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = stdNumComm.ExecuteReader())
            {
                while (reader.Read())
                {
                    string s = reader.GetString(0);
                    if (s != "01360406")
                    {
                        mentorStdNums.Add(reader.GetString(0));
                    }
                }
            }
            conn.Close();

            return mentorStdNums;
        }

        /**
         * Gets a list of the student numbers of all mentees
         */
        public static List<string> GetMenteeStudentNumbers()
        {
            List<string> menteeStdNums = new List<string>();

            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            // the command to be executed
            SqlCommand stdNumComm = new SqlCommand("SELECT StudentNumber FROM ProfileDetails " +
                "WHERE Role='0'");
            stdNumComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = stdNumComm.ExecuteReader())
            {
                while (reader.Read())
                {
                    menteeStdNums.Add(reader.GetString(0));
                }
            }
            conn.Close();

            return menteeStdNums;
        }

        /**
         * Takes a completed Scheduler and uses the data to assign mentors and mentees to groups
         */
        public static void CreateGroups(Scheduler s)
        {
            // comm for creating a group
            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            // the command to be executed
            SqlCommand createGroupComm = new SqlCommand("INSERT INTO Groups (GroupNumber, Mentor) " +
                "values(@groupNumber, @mentor)");
            // set the parameters
            createGroupComm.Parameters.Add("@groupNumber", SqlDbType.Int);
            createGroupComm.Parameters.Add("@mentor", SqlDbType.VarChar);
            createGroupComm.Connection = conn;

            // comm for setting a user's group number
            SqlCommand setGroupComm = new SqlCommand("UPDATE ProfileDetails SET GroupNumber=@groupNumber WHERE StudentNumber=@studentNum");
            setGroupComm.Parameters.Add("@groupNumber", SqlDbType.Int);
            setGroupComm.Parameters.Add("@studentNum", SqlDbType.VarChar);
            conn.Open();

            int groupNum = 0;
            for(int day = 0; day < 5; day++)
            {
                for (int period = 0; period < 10; period++)
                {
                    if (s.MentorsByPeriod[day, period] != null && s.MentorsByPeriod[day, period].Count > 0 && s.MentorsByPeriod[day, period][0] != null)
                    {
                        if (s.MentorsByPeriod[day, period].Count == 1)
                        {
                            groupNum++;
                            createGroupComm.Parameters["@groupNumber"].Value = groupNum;
                            createGroupComm.Parameters["@mentor"].Value = s.MentorsByPeriod[day, period][0].ID;
                            using (conn)
                            {
                                createGroupComm.ExecuteNonQuery();
                            }

                            setGroupComm.Parameters["@groupNumber"].Value = groupNum;
                            setGroupComm.Parameters["@studentNum"].Value = s.MentorsByPeriod[day, period][0].ID;
                            using (conn)
                            {
                                setGroupComm.ExecuteNonQuery();
                            }

                            foreach (var mentee in s.MenteesByPeriod[day, period])
                            {
                                setGroupComm.Parameters["@groupNumber"].Value = groupNum;
                                setGroupComm.Parameters["@studentNum"].Value = mentee.ID;
                                using (conn)
                                {
                                    setGroupComm.ExecuteNonQuery();
                                }
                            }
                        }
                    } else
                    {
                        int startGroupNum = groupNum;
                        foreach(var mentor in s.MentorsByPeriod[day, period])
                        {
                            startGroupNum++;
                            createGroupComm.Parameters["@groupNumber"].Value = groupNum;
                            createGroupComm.Parameters["@mentor"].Value = mentor.ID;
                            using (conn)
                            {
                                createGroupComm.ExecuteNonQuery();
                            }

                            setGroupComm.Parameters["@groupNumber"].Value = groupNum;
                            setGroupComm.Parameters["@studentNum"].Value = mentor.ID;
                            using (conn)
                            {
                                setGroupComm.ExecuteNonQuery();
                            }
                        }
                        groupNum++;
                        int count = 0;
                        int max = (int)Math.Ceiling(s.MenteesByPeriod[day, period].Count / (double)s.MentorsByPeriod[day, period].Count);
                        foreach(var mentee in s.MenteesByPeriod[day, period])
                        {
                            setGroupComm.Parameters["@groupNumber"].Value = groupNum;
                            setGroupComm.Parameters["@studentNum"].Value = mentee.ID;
                            using (conn)
                            {
                                setGroupComm.ExecuteNonQuery();
                            }
                            count++;
                            if (count >= max)
                            {
                                count = 0;
                                groupNum++;
                            }
                        }
                    }
                }
            }

            conn.Close();
        }
        
        /**
         * Sets attendance of the user with the given studentnumber for the given meeting to the given value;
         * if (studentNumber, meetingID) not in the Attendance table, this will add it and set the attendance
         */
         public static void SetAttendance(string studentNumber, int meetingID, bool attended)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand checkExistsComm = new SqlCommand("SELECT COUNT(*) FROM Attendance WHERE StudentNumber=@studentNumber AND MeetingID=@meetingID");
            // set the parameters
            checkExistsComm.Parameters.Add("@studentNumber", SqlDbType.VarChar);
            checkExistsComm.Parameters["@studentNumber"].Value = studentNumber;

            checkExistsComm.Parameters.Add("@meetingID", SqlDbType.Int);
            checkExistsComm.Parameters["@meetingID"].Value = meetingID;

            checkExistsComm.Connection = conn;
            conn.Open();

            bool exists = false;

            using (conn)
            using (var reader = checkExistsComm.ExecuteReader())
            {
                while (reader.Read())
                {
                    exists = reader.GetInt32(0) > 0;
                }
            }
            conn.Close();

            string updateQuery = "UPDATE Attendance SET Attended=@attended WHERE StudentNumber=@studentNumber AND MeetingID = @meetingID";
            string insertQuery = "INSERT INTO Attendance (StudentNumber, MeetingID, Attended) values(@studentNumber, @meetingID, @attended)";

            SqlCommand insertUpdateComm = new SqlCommand(exists ? updateQuery : insertQuery);
            insertUpdateComm.Parameters.Add("@studentNumber", SqlDbType.VarChar);
            insertUpdateComm.Parameters.Add("@attended", SqlDbType.Bit);
            insertUpdateComm.Parameters.Add("@meetingID", SqlDbType.Int);

            insertUpdateComm.Parameters["@studentNumber"].Value = studentNumber;
            insertUpdateComm.Parameters["@attended"].Value = attended;
            insertUpdateComm.Parameters["@meetingID"].Value = meetingID;

            conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            insertUpdateComm.Connection = conn;
            conn.Open();

            using (conn)
            {
                insertUpdateComm.ExecuteNonQuery();
            }

            conn.Close();
        }

        /**
         * Returns whether the student is registered as attending the meeting
         */
        public static bool GetAttendance(string studentNumber, int meetingID)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand checkExistsComm = new SqlCommand("SELECT Attended FROM Attendance WHERE StudentNumber=@studentNumber AND MeetingID=@meetingID");
            // set the parameters
            checkExistsComm.Parameters.Add("@studentNumber", SqlDbType.VarChar);
            checkExistsComm.Parameters["@studentNumber"].Value = studentNumber;

            checkExistsComm.Parameters.Add("@meetingID", SqlDbType.Int);
            checkExistsComm.Parameters["@meetingID"].Value = meetingID;

            checkExistsComm.Connection = conn;
            conn.Open();

            bool attended = false;

            using (conn)
            using (var reader = checkExistsComm.ExecuteReader())
            {
                if (reader.Read())
                {
                    // if no attendance is registered, defaults to false
                    attended = reader.IsDBNull(0) ? false : reader.GetBoolean(0);
                }
            }

            conn.Close();
            return attended;
        }

        //Gets all the questions and their results for the given Feedback form and returns them as FeedbackItems
        public static List<FeedbackItem> GetFeedback(string role, string title)
        {
            List<FeedbackItem> feedbackList = new List<FeedbackItem>();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand titleComm = new SqlCommand("SELECT Question, Role, Total, Count, Title FROM Feedback WHERE Title='" + title + "' AND Role='" + role + "'");
            titleComm.Connection = conn;
            conn.Open();
 
            using (conn)
            using (var reader = titleComm.ExecuteReader())
            {
                for (int i = 0; i < CountFeedback(role, title); i++)
                {
                    reader.Read();
 
                    // create and populate the announcement
                    FeedbackItem a = new FeedbackItem(reader.GetInt32(2), reader.GetInt32(3));
                    a.Title = reader.GetString(4); 
                    a.Question = reader.GetString(0);
                    a.Role = reader.GetString(1);
                    feedbackList.Add(a);
                }
 
            }
            conn.Close();
            return feedbackList;
        }

        //Returns all the stored Feedback form titles 
        public static List<string> GetFeedbackListTitles() 
        {
            List<string> feedbackListTitles = new List<string>();
            string feedbackListTitle = "";
 
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand titleComm = new SqlCommand("SELECT DISTINCT Role, Title FROM Feedback");
 
 
            titleComm.Connection = conn;
            conn.Open();
            
            using (conn)
            using (var reader = titleComm.ExecuteReader())
            {
                for (int i = 0; i < CountAllFeedback(); i++)
                {
                    reader.Read();
                    feedbackListTitle = reader.GetString(0) + " - " + reader.GetString(1);
                    feedbackListTitles.Add(feedbackListTitle);
                }
 
            }
            conn.Close();
            return feedbackListTitles;
        }

        //Returns the number of completions of the given Feedback form
        public static int CountFeedback(string role, string title)
        {
            int feedbackCount = 0;
 
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand feedbackComm = new SqlCommand("SELECT COUNT(*) FROM Feedback WHERE Title='" + title + "' AND Role='" + role + "';");
 
 
            feedbackComm.Connection = conn;
            conn.Open();
 
            using (conn)
            using (var reader = feedbackComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    feedbackCount = reader.GetInt32(0);
                }
 
            }
            conn.Close();
            return feedbackCount;
        }

        //Returns the number of Feedback forms that have been made
        public static int CountAllFeedback()
 
        {
            int feedbackCount = 0;
 
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand feedbackComm = new SqlCommand("SELECT COUNT(DISTINCT Title + '' + Role) FROM Feedback;");
 
            feedbackComm.Connection = conn;
            conn.Open();
 
            using (conn)
            using (var reader = feedbackComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    feedbackCount = reader.GetInt32(0);
                }
            }
            conn.Close();
            return feedbackCount;
 
 
        }

        //Returns the most recent Feedback form created for the given audience
        public static string GetTitle(string role) 
        {
            string recentTitle = "";
 
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand feedbackComm = new SqlCommand("SELECT TOP 1 Title FROM Feedback WHERE Role = '" + role + "' ORDER BY Id DESC;");
            feedbackComm.Connection = conn;
 
            conn.Open();
 
            using (conn)
            using (var reader = feedbackComm.ExecuteReader())
            {
                if (reader.HasRows) 
                {
                    reader.Read();
                    recentTitle = reader.GetString(0);
                }
            }
            conn.Close();
            return recentTitle;
        }

        //Returns the number of completions of the given question in the given Feedback form
        public static int GetFeedbackCount(string role, string title, string question)
        {
            int feedbackCount = 0;
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand feedbackComm = new SqlCommand("SELECT Count FROM Feedback WHERE Role = '" + role + "' AND Title = '" + title + "' AND Question = '" + question + "';");
 
            feedbackComm.Connection = conn;
            conn.Open();

 
            using (conn)
            using (var reader = feedbackComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    feedbackCount = reader.GetInt32(0);
                }
 
            }
            conn.Close();
            return feedbackCount;
        }

        //Returns the total answered result for the given question in the given Feedback form
        public static int GetFeedbackTotal(string role, string title, string question)
        {
            int feedbackTotal = 0;
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand feedbackComm = new SqlCommand("SELECT Total FROM Feedback WHERE Role = '" + role + "' AND Title = '" + title + "' AND Question = '" + question + "';");
 
            feedbackComm.Connection = conn;
            conn.Open();
 
            using (conn)
            using (var reader = feedbackComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    feedbackTotal = reader.GetInt32(0);
                }
            }
            conn.Close();
            return feedbackTotal;
 
        }
        //Retreives the average of the all of the feedback questions and updates it.
        public static bool UpdateAverageFeedback(string role, string title,string question, float average)
        {
            bool result = false;
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getUserComm = new SqlCommand("SELECT * FROM Feedback WHERE Role=@role AND Title=@title AND Question=@question");
            // set the parameters

            getUserComm.Connection = conn;

            getUserComm.Parameters.AddWithValue("@role", role);
            getUserComm.Parameters.AddWithValue("@title", title);
            getUserComm.Parameters.AddWithValue("@question", question);
            conn.Open();

            using (conn)
            using (var reader = getUserComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    result = true;
                    using (var _db = new MPAS.Models.ApplicationDbContext())
                    {
                        _db.Database.ExecuteSqlCommand("UPDATE Feedback SET Average = @p0 WHERE Role = @p1 AND Title = @p2 AND Question = @p3", average, role, title, question);
                    }
                }
            }

            conn.Close();
            return result;
        }
		
        /**
         * Changes the details of the user with the primary key matching `studentNumber` to match those provided
         */
		public static bool UpdateUser(string studentNumber, string firstName, string surname, DateTime DOB)
         {
            bool result = false;
             
               SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                SqlCommand getUserComm = new SqlCommand("SELECT * FROM ProfileDetails WHERE StudentNumber=@studentNumber");
                // set the parameters
               
                getUserComm.Connection = conn;
                
                getUserComm.Parameters.AddWithValue("@studentNumber", studentNumber);
                
            conn.Open();
           
            using (conn)
                using (var reader = getUserComm.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        result = true;
                        using (var _db = new MPAS.Models.ApplicationDbContext())
                        {
                        //_db.Database.ExecuteSqlCommand("UPDATE ProfileDetails  (FirstName, Surname, PictureURL) " +
                        //      "SET (@p0, @p1, @p2)", firstName, surname, PictureURL);
                        _db.Database.ExecuteSqlCommand("UPDATE ProfileDetails SET FirstName = @p0, Surname = @p1 , DateOfBirth = @p2 Where StudentNumber = @p3", firstName, surname, DOB, studentNumber);
                        
                       
						}
                    }
                }

            conn.Close();
            return result;
        }
		//Updates the picture of the student number
		public static bool UpdatePicture(string studentNumber, string pictureURL)
        {
            bool result = false;

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            //Retrieves all the information with the corresonding student number
            SqlCommand getUserComm = new SqlCommand("SELECT * FROM ProfileDetails WHERE StudentNumber=@studentNumber");
            // set the parameters

            getUserComm.Connection = conn;

            getUserComm.Parameters.AddWithValue("@studentNumber", studentNumber);

            conn.Open();
        
            using (conn)
            using (var reader = getUserComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    result = true;
                    using (var _db = new MPAS.Models.ApplicationDbContext())
                    {
                        //_db.Database.ExecuteSqlCommand("UPDATE ProfileDetails  (FirstName, Surname, PictureURL) " +
                        //      "SET (@p0, @p1, @p2)", firstName, surname, PictureURL);
                        
                        _db.Database.ExecuteSqlCommand("UPDATE ProfileDetails SET PictureURL = @p0 Where StudentNumber = @p1",pictureURL, studentNumber);


                    }
                }
            }

            conn.Close();
            return result;
        }
		//retrieves all the pictures of the mentees who have the same group number as the current logged in mentor. For example. If mentor XXX is in group 2. It will display pictures of all mentees with group2.
		public static List<User> GetMenteePictures(string studentNumber, int groupNumber) 
        {
            List<User> listOfMentees = new List<User>();
            
 
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getUserComm = new SqlCommand("SELECT  StudentNumber,FirstName, Surname, PictureURL " +
                " FROM ProfileDetails WHERE GroupNumber ='" + groupNumber + "' AND  StudentNumber !='"+ studentNumber + "'");
 
            getUserComm.Connection = conn;
            conn.Open();
 
            using (conn)
            using (var reader = getUserComm.ExecuteReader())
            {
                while(reader.Read())
                {
                    User u = GetUser(reader.GetString(0));
                    // set the values in the object
                    //u.FirstName = reader.GetString(0);
 
                    // u.Surname = reader.GetString(1);
                    u.FirstName = reader.GetString(1);
                    u.Surname = reader.GetString(2);
 
                    u.PathToPicture = reader.GetString(3);
                    listOfMentees.Add(u);
 
                }
            }
            conn.Close();
            return listOfMentees;
        }
        //retrieves all the pictures of all the users in the database. this is displayed for the admin to view.
        public static List<User> GetAllMenteePictures(string studentNumber, int groupNumber)
        {
            List<User> listOfEveryone = new List<User>();
 
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getUserComm = new SqlCommand("SELECT  StudentNumber,FirstName, Surname, PictureURL FROM ProfileDetails WHERE StudentNumber != @studentNumber");
            getUserComm.Parameters.AddWithValue("@studentNumber", studentNumber);
            

            getUserComm.Connection = conn;
            conn.Open();
 
            using (conn)
            using (var reader = getUserComm.ExecuteReader())
            {
                while (reader.Read())
                {
                    User u = GetUser(reader.GetString(0));
                    u.FirstName = reader.GetString(1);
                    u.Surname = reader.GetString(2);
                    u.PathToPicture = !reader.IsDBNull(3) ? reader.GetString(3) : "default.jpg";
                    listOfEveryone.Add(u);
 
                }
            }
            conn.Close();
            return listOfEveryone;
        }
        //Retrieves all the feedback questions with the corresponding title.
        public DataSet SelectFeedback(string title)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Feedback WHERE Title = '"+ title +"'", conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        //Gets the student numbers of all mentors who have written an additional comment in the given Feedback form
        public static List<string> GetFeedbackSNum(string title)
        {
            List<string> fbSNums = new List<string>();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getFeedbackComm = new SqlCommand("SELECT SNum FROM FeedbackComments WHERE Title = '" + title + "'");
            getFeedbackComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = getFeedbackComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    string fbSNum = reader.GetString(0);
                    fbSNums.Add(fbSNum);
                }
            }
            conn.Close();
            return fbSNums;
        }

        //Gets all the additional feedback comment of the given mentor in the given Feedback form
        public static string GetFeedbackComment(string title, string snum)
        {
            string fbComment = "";
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand getFeedbackComm = new SqlCommand("SELECT Comment FROM FeedbackComments WHERE Title = '" + title + "' AND SNum='" + snum + "'");
            getFeedbackComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = getFeedbackComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    fbComment = reader.GetString(0);
                }
            }
            conn.Close();
            return fbComment;
        }

        //Gets the number of additional comments submitted for the given Feedback form
        public static int CountFeedbackComments(string title)
        {
            int feedbackCount = 0;
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand feedbackComm = new SqlCommand("SELECT COUNT(*) FROM FeedbackComments WHERE Title='" + title + "';");
            feedbackComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = feedbackComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    feedbackCount = reader.GetInt32(0);
                }
            }

            conn.Close();
            return feedbackCount;
        }
		
		/**
         * Sets whether a user is available in a given period
         */
		public static List<Tuple<int, int>> GetFreePeriodsForUser(string studentNum, int day, int period, bool available)
        {
            List<Tuple<int, int>> freePeriods = new List<Tuple<int, int>>();
 
            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            // the command to be executed
            SqlCommand scheduleComm = new SqlCommand("SELECT Monday, Tuesday, Wednesday, Thursday, Friday FROM Schedule " +
                "WHERE StudentNumber=@studentNumber");
            // set the parameters
            scheduleComm.Parameters.Add("@studentNumber", SqlDbType.VarChar);
            scheduleComm.Parameters["@studentNumber"].Value = studentNum;
            scheduleComm.Connection = conn;
            conn.Open();
 
            using (conn)
            using (var reader = scheduleComm.ExecuteReader())
            {
                while (reader.Read())
                {
                    string[] days = new string[5];
                    for (int i = 0; i < 5; i++)
                    {
                        days[i] = reader.GetString(i);
                        for (int j = 0; j < 10; j++)
                        {
                            if (days[i].Contains("" + j))
                            {
                                freePeriods.Add(new Tuple<int, int>(i, j));
                            }
                        }
                    }
                }
            }
            conn.Close();
            return freePeriods;
        }
		
		/**
         * Generates and returns a randomized 9 character StudentNumber
         * Return: random 9-character ID string
         */
        public static string MakeRandomStudentNumber(Random r)
        {
            string sID = "";
            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < 6; i++)
            {
                sID += alpha[r.Next(0, alpha.Length)];
            }
            for(int i = 0; i < 3; i++)
            {
                sID += "" + r.Next(2, 10);
            }
            return sID;
        }
		
		public static void CreateGroups(List<Tuple<Person, List<Person>>> groups)
        {
            // comm for creating a group
            // connection to execute sql command
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            //delete all groups
            SqlCommand deleteGroupsComm = new SqlCommand("DELETE FROM Groups");
            deleteGroupsComm.Connection = conn;
            conn.Open();
            using (conn)
            {
                deleteGroupsComm.ExecuteNonQuery();
            }
            conn.Close();
            
            // the command to be create a group entry
            SqlCommand createGroupComm = new SqlCommand("INSERT INTO Groups (GroupNumber, Mentor) " +
                    "values(@groupNumber, @mentor)");
            // set the parameters
            createGroupComm.Parameters.Add("@groupNumber", SqlDbType.Int);
            createGroupComm.Parameters.Add("@mentor", SqlDbType.VarChar);
 
 
            // comm for setting a user's group number
            SqlCommand setGroupComm = new SqlCommand("UPDATE ProfileDetails SET GroupNumber=@groupNumber WHERE StudentNumber=@studentNum");
            setGroupComm.Parameters.Add("@groupNumber", SqlDbType.Int);
            setGroupComm.Parameters.Add("@studentNum", SqlDbType.VarChar);
            
            // write each group to the database, assigning them a group number
 
            int groupNum = 0;
            foreach(var group in groups)
            {
                groupNum++;
                // create the group
                conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                createGroupComm.Parameters["@groupNumber"].Value = groupNum;
                createGroupComm.Parameters["@mentor"].Value = group.Item1.ID;
                createGroupComm.Connection = conn;
                conn.Open();
                using(conn)
                {
                    createGroupComm.ExecuteNonQuery();
                }
 
                conn.Close();
 
                //set the mentor group numbers
                conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
 
                setGroupComm.Parameters["@groupNumber"].Value = groupNum;
                setGroupComm.Parameters["@studentNum"].Value = group.Item1.ID;
                setGroupComm.Connection = conn;
                conn.Open();
                using (conn)
                {
                    setGroupComm.ExecuteNonQuery();
                }
                conn.Close();
 
                // set the mentees group numbers
                foreach(Person mentor in group.Item2)
                {
                    conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
 
                    setGroupComm.Parameters["@groupNumber"].Value = groupNum;
                    setGroupComm.Parameters["@studentNum"].Value = mentor.ID;
                    setGroupComm.Connection = conn;
                    conn.Open();
 
                    using (conn)
                    {
                        setGroupComm.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
 
            conn.Close();
        }
		
		/**
         * Sets the availability of a user in a given period (specified by a day (0 to 4) and a period (0 to 9))
         * studentNumber : the id of the student for which to update
         * periodsAvailable[,] : a 5x10 array of booleans of where periodsAvailable[day, period] is true if the user is available
         */
        public static void SetAvailablePeriods(string studentNumber, bool[,] periodsAvailable)
        {
            // setup the update command with parameters
            string updateQuery = "UPDATE Schedule SET Monday=@MondayStr, Tuesday = @TuesdayStr, Wednesday=@WednesdayStr, " + 
                " Thursday=@ThursdayStr, Friday=@FridayStr WHERE StudentNumber=@studentNumber";
            SqlCommand updateComm = new SqlCommand(updateQuery);
 
            string[] days = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            string[] stringsByDay = new string[5]; // hold the monday..fridayStr values
 
            // create the strings to enter in the database
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    if (periodsAvailable[i,j])
                    {
                        stringsByDay[i] += j;
                    }
                }
            }
 
            // setup the SQL parameters
            updateComm.Parameters.Add("@studentNumber", SqlDbType.VarChar);
            updateComm.Parameters["@studentNumber"].Value = studentNumber;
            for(int i = 0; i < 5; i++)
            {
                updateComm.Parameters.Add($"@{days[i]}Str", SqlDbType.VarChar);
                updateComm.Parameters[$"@{days[i]}Str"].Value = stringsByDay[i]; 
            }
            
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            updateComm.Connection = conn;
            conn.Open();
 
            using (conn)
            {
                updateComm.ExecuteNonQuery();
            }
 
            conn.Close();
        }
		
		/**
         * Randomly generates a mentor and adds it to the database
         */ 
         public static void RandomlyGenerateMentor(int count)
        {
            // store the sql commands for iteration in the transaction;
            // 2 * count, since one string will update profile details and the other 
            // will update the schedule
            List<string> sqlCommands = new List<string>(2*count);
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                bool[,] avail = new bool[5, 10];
                string stdNumber = MakeRandomStudentNumber(r);
                for (int day = 0; day < 5; day++)
                {
                    for (int per = 0; per < 10; per++)
                    {
                        avail[day, per] = r.Next(1, 101) > 50; // % probability of being busy
                    }
                }
                // generate the commands and add to the list
                sqlCommands.Add($"INSERT INTO ProfileDetails (StudentNumber, FirstName, Surname, Role) values('" + stdNumber + "', '" + stdNumber + "', '', '1')");
                sqlCommands.Add(GetAvailablePeriodCommand(stdNumber, avail));
            }

            // use transactions for quickly excuting multiple commands
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
               connection.Open();

                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand("", connection, trans))
                    {
                        command.CommandType = System.Data.CommandType.Text;

                        // perform each command
                        foreach (var commandString in sqlCommands)
                        {
                            command.CommandText = commandString;
                            command.ExecuteNonQuery();
                        }
                    }
                    // commit all commands to the database
                    trans.Commit();
                }
            }
        }
 
        /**
         * Randomly generates a mentee and adds it to the database
         */
        public static void RandomlyGenerateMentee(int count)
        {
            // store the sql commands for iteration in the transaction;
            // 2 * count, since one string will update profile details and the other 
            // will update the schedule
            List<string> sqlCommands = new List<string>(2 * count);
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                bool[,] avail = new bool[5, 10];
                string studentNumber = MakeRandomStudentNumber(r);
                for (int day = 0; day < 5; day++)
                {
                    for (int per = 0; per < 10; per++)
                    {
                        avail[day, per] = r.Next(1, 101) > 50; // % probability of being busy
                    }
                }
                // generate the commands and add to the list
                sqlCommands.Add($"INSERT INTO ProfileDetails (StudentNumber, FirstName, Surname, Role) values('{studentNumber}', '{studentNumber}', '', '0')");
                sqlCommands.Add(GetAvailablePeriodCommand(studentNumber, avail));
            }

            // use transactions for quickly excuting multiple commands
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                connection.Open();

                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand("", connection, trans))
                    {
                        command.CommandType = System.Data.CommandType.Text;

                        // perform each command
                        foreach (var commandString in sqlCommands)
                        {
                            command.CommandText = commandString;
                            command.ExecuteNonQuery();
                        }
                    }
                    // commit all commands to the database
                    trans.Commit();
                }
            }
        }

        /**
         * returns a sql command string that when run will perform an update in the Schedule table for the user's schedule
         * based on the schedule provided in periodsAvailable
         */
        public static string GetAvailablePeriodCommand(string studentNumber, bool[,] periodsAvailable)
        {
            // setup the update command with parameters
            string[] days = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            string[] stringsByDay = new string[5]; // hold the monday..fridayStr values

            // create the strings to enter in the database
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (periodsAvailable[i, j])
                    {
                        stringsByDay[i] += j;
                    }
                }
            }

            string updateQuery = $"INSERT INTO Schedule (StudentNumber, Monday, Tuesday, Wednesday, Thursday, Friday)" +
                $" values('{studentNumber}', '{stringsByDay[0]}', '{stringsByDay[1]}', '{stringsByDay[2]}', '{stringsByDay[3]}', '{stringsByDay[4]}')";
            return updateQuery;
        }
    }
}
		