using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using MPAS.Logic;
using MPAS.Models;

namespace MPAS
{
    public partial class NewMessage : System.Web.UI.Page
    {
        private User currentUser; 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.User == null || !this.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
            } else if (this.User.IsInRole("Administrator"))
            {
                currentUser = Administrator.Get();
            } else
            {
                currentUser = DatabaseUtilities.GetUser(this.User.Identity.Name);
            }
        }

        protected void StdNumValidate(object source, ServerValidateEventArgs args)
        {
            // non-numeric name of length at least 1
            args.IsValid = (Regex.IsMatch(args.Value, @"^[a-zA-Z]{6,6}[0-9]{3,3}$") // student number
                || Regex.IsMatch(args.Value, @"^[0-9]{8,8}$")); // staff number;
        }

        protected void MsgValidate(object src, ServerValidateEventArgs args)
        {
            args.IsValid = (!Regex.IsMatch(args.Value, @"<[^>]+>"));
        }

        /*
         * Triggers a search for users based on the content of the search text box, returning a clickable table of users to populate the destination textbox
         */
        protected void Search_Button_Click(object sender, EventArgs e)
        {
            if (Search_TextBox.Text.Trim() == "") return;
            List<string> suggestions = SearchUtility.Search(Search_TextBox.Text);

            foreach(string s in suggestions)
            {
                TableRow r = new TableRow();
                TableCell contents = new TableCell();
                LinkButton selectionButton = new LinkButton();
                selectionButton.CssClass = "button";
                selectionButton.Text = s;
                string stdNum = s.Split(' ')[0];
                selectionButton.ID = stdNum + "_btn";
                selectionButton.OnClientClick = $"document.getElementById('{Recipient_TextBox.ClientID}').value = '{stdNum}';";
                AsyncPostBackTrigger t = new AsyncPostBackTrigger();
                t.ControlID = selectionButton.ID;
                contents.Controls.Add(selectionButton);
                r.Controls.Add(contents);
                SearchResult_Table.Controls.Add(r);
                Search_Panel.Triggers.Add(t);
            }

        }

        /**
         * OnClick handler for buttons added to the search result table
         */
        protected void TableButton_Click(object sender, EventArgs e)
        {
            // remove the set of buttons and their triggers
            Search_Panel.Triggers.Clear();
            AsyncPostBackTrigger searchBtnTrigger = new AsyncPostBackTrigger();
            searchBtnTrigger.ControlID = Search_Button.ID;
            Search_Panel.Triggers.Add(searchBtnTrigger);

            SearchResult_Table.Controls.Clear();
            Search_TextBox.Text = "";

            Recipient_TextBox.Text = (sender as Button).ID.Replace("_btn", "").ToUpper();
        }

        protected void SendButton_Clicked(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // send the message
                DatabaseUtilities.SendPrivateMessage(currentUser.StudentNumber, Recipient_TextBox.Text, Content_TextBox.Text, DateTime.Now);
                Response.Redirect("~/MessageThread.aspx?s=" + Recipient_TextBox.Text);
            }
        }
    }
}