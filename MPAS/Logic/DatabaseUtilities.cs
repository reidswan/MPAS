﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MPAS.Models;

namespace MPAS.Logic
{
    /*
     * Class to handle all database logic
     */
    public class DatabaseUtilities
    {
        public static void AddMeeting(string title, string agenda, DateTime start, DateTime end, int groupNumber, string madeByStdNum)
        {

        }

        public static int GetHighestGroupNumber ()
        {
            // TODO: implement
            return 0;
        }

        // get the User object associated with the given student number
        public static User GetUser( string studentNumber)
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
    }
}