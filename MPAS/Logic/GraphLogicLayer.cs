using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MPAS.Logic
{
    public partial class GraphLogicLayer : System.Web.UI.Page
    {
        DatabaseUtilities d1 = new DatabaseUtilities();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public System.Data.DataSet SelectFeedbacks(string title)
        {
            return d1.SelectFeedback(title);
        }
    }
}