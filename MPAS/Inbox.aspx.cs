using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Models;
using MPAS.Logic;

namespace MPAS
{
    public partial class Inbox : System.Web.UI.Page
    {
        User currentUser = null;
        List<User> senders;
        List<PrivateMessage> messages;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.User == null || !this.User.Identity.IsAuthenticated)
            {

            } else if (this.User.IsInRole("Administrator"))
            {
                currentUser = Administrator.Get();
            } else
            {
                currentUser = DatabaseUtilities.GetUser(this.User.Identity.Name);
            }

            senders = DatabaseUtilities.GetSendersToUser(currentUser.StudentNumber);
            messages = DatabaseUtilities.GetMessagesForUser(currentUser.StudentNumber);
            PopulatePage();
        }

        /*
         * Fills the page with lists of message threads
         * Will only get one (most recent) message per user
         */
        private void PopulatePage()
        {
            if (senders.Count > 0)
            {
                NoMessages.Visible = false;
                foreach (User s in senders)
                {
                    PrivateMessage pMsg = null;
                    foreach (PrivateMessage m in messages)
                    {
                        if (m.Source.StudentNumber.Equals(s.StudentNumber))
                        {
                            pMsg = m;
                            break;
                        }
                    }

                    TableRow t = new TableRow();
                    TableCell fromCell = new TableCell();
                    TableCell contentCell = new TableCell();
                    TableCell dateCell = new TableCell();

                    // these strings will format the message in bold if it is unread
                    string formattingStartStr = "<h5>" + (!pMsg.Read ? "<b>" : "");
                    string formattingEndStr = (!pMsg.Read ? "</b>" : "") + "</h5>";

                    // populate the table cells
                    fromCell.Text = $"{formattingStartStr}<a href='MessageThread.aspx?s={s.StudentNumber}'>" + 
                        $"{s.FirstName} {s.Surname}</a>{formattingEndStr}";
                    contentCell.Text = formattingStartStr + (pMsg.MessageContent.Length > 50 ? (pMsg.MessageContent.Substring(0, 46) + "...") : pMsg.MessageContent) + formattingEndStr;
                    dateCell.Text = formattingStartStr + pMsg.SendTime.ToShortTimeString() + ", " + pMsg.SendTime.ToShortDateString() + formattingEndStr;

                    t.Controls.Add(fromCell);
                    t.Controls.Add(contentCell);
                    t.Controls.Add(dateCell);
                    MessageThreadTable.Controls.Add(t);

                    MessageThreadTable.Visible = true;
                }
            }
        }
    }


}