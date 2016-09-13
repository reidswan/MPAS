using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MPAS.Error
{
    public partial class AuthError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string source = Request.QueryString["source"];
            if (source == null) source = "a protected page";
            From_Label.Text = source;
        }
    }
}