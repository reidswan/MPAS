using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using MPAS.Logic;
using MPAS.Models;
using System.Data;
using System.Data.SqlClient;

namespace MPAS
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RoleActions roleActions = new RoleActions();
            roleActions.createAdmin();

            InitializeFromDatabase();
        }

        void InitializeFromDatabase()
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand setupComm = new SqlCommand("SELECT MAX(Id) FROM Announcements");
            setupComm.Connection = conn;
            conn.Open();

            using (conn)
            using (var reader = setupComm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    Announcement.NextID = (reader.IsDBNull(0)) ? 0 : reader.GetInt32(0);
                }
            }
        }
    }
}