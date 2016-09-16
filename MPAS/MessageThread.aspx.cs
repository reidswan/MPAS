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
    public partial class MessageThread : System.Web.UI.Page, IMessageReceiver
    {
        User currentUser = null;
        User otherUser = null;
        //List<PrivateMessage> messages;
        private PrivateChat chat;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.User == null || this.User.Identity.IsAuthenticated == false)
            {
                Response.Redirect("~/Account/Login.aspx");
            } else if (this.User.IsInRole("Administrator"))
            {
                currentUser = Administrator.Get();
            } else
            {
                currentUser = DatabaseUtilities.GetUser(this.User.Identity.Name);
            }

            string otherStudentNumber = Request.QueryString["s"];
            if (otherStudentNumber != null)
            {
                otherUser = DatabaseUtilities.GetUser(otherStudentNumber);
                //messages = DatabaseUtilities.GetMessagesBetweenUsers(currentUser.StudentNumber, otherUser.StudentNumber);
                chat = PrivateChatManager.GetChat(currentUser.StudentNumber, otherUser.StudentNumber);
                // tell the chat that we want to receive its messages
                chat.RegisterReceiver(this);
                PopulatePage();
            } else
            {
                // no other user was supplied so return them to the inbox page
                Response.Redirect("~/Inbox.aspx");
            }
        }
        
        /**
         * Fills the page with pre-sent messages
         */
        private void PopulatePage()
        {
            MessageThreadLabel.Text = "";
            foreach (Message m in chat.Messages)
            {
                MessageThreadLabel.Text += "<div class='row'>\n";
                // add the sender name and date
                MessageThreadLabel.Text += "<div class='col-xs-3 col-md-3'>\n";
                MessageThreadLabel.Text += $"<small><b>{m.Source.FirstName} {m.Source.Surname}</b> ({m.SendTime.ToShortTimeString()}, {m.SendTime.ToShortDateString()}):</small>";
                //add the message
                MessageThreadLabel.Text += "</div>\n<div class='col-xs-9 col-md-9'>";
                MessageThreadLabel.Text += $"{m.MessageContent}";
                MessageThreadLabel.Text += "</div>\n</div>\n";
            }
        }

        public void SendButtonClick(object sender, EventArgs e)
        {
            PrivateMessage newMsg = new PrivateMessage();
            newMsg.Source = currentUser;
            newMsg.Destination = otherUser;
            newMsg.MessageContent = MessageBox.Text;
            MessageBox.Text = "";
            newMsg.SendTime = DateTime.Now;
            chat.Receive(newMsg);
        }

        /**
         * Receives a message from the PrivateChat
         */
        public void Receive(Message msg)
        {
            PrivateMessage m = (PrivateMessage)msg;
            MessageThreadLabel.Text += "<div class='row'>\n";
            // add the sender name and date
            MessageThreadLabel.Text += "<div class='col-xs-3 col-md-3'>\n";
            MessageThreadLabel.Text += $"<small><b>{m.Source.FirstName} {m.Source.Surname}</b> ({m.SendTime.ToShortTimeString()}, {m.SendTime.ToShortDateString()}):</small>";
            //add the message
            MessageThreadLabel.Text += "</div>\n<div class='col-xs-9 col-md-9'>";
            MessageThreadLabel.Text += $"{m.MessageContent}";
            MessageThreadLabel.Text += "</div>\n</div>\n";
        }
    }
}