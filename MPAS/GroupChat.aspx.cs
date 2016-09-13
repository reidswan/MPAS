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
    public partial class GroupChat : System.Web.UI.Page
    {

        User currentUser = null;
        MPAS.Models.Chatroom cr;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.User == null || !this.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Error/AuthError.aspx?source=Chatroom");
            } else if (this.User.IsInRole("Administrator"))
            {
                currentUser = Administrator.Get();
            } else
            {
                currentUser = DatabaseUtilities.GetUser(this.User.Identity.Name);
            }

            cr = ChatroomManager.GetChatroom(currentUser.GroupNumber);

            // put all the messages in the chatroom on the page
            PopulateChatMessages();
        }
        
        /**
         * Place all received messages in the chat
         */
        private void PopulateChatMessages()
        {
            ChatroomLabel.Text = "";
            foreach(ChatMessage m in cr.Messages)
            {
                ChatroomLabel.Text += "<div class='row'>\n";
                // add the sender name and date
                ChatroomLabel.Text += "<div class='col-xs-4 col-md-4'>\n";
                ChatroomLabel.Text += $"<b>{m.Source.FirstName} {m.Source.Surname}</b> <small>({m.SendTime.ToShortTimeString()}, {m.SendTime.ToShortDateString()}):</small>";
                //add the message
                ChatroomLabel.Text += "</div>\n<div class='col-xs-8 col-md-8'>";
                ChatroomLabel.Text += $"{m.MessageContent}";
                ChatroomLabel.Text += "</div>\n</div>\n";
            }
        }

        protected void SendButtonClick(object sender, EventArgs e)
        {

        }
    }
}