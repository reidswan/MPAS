using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPAS.Models;
using MPAS.Logic;
using Microsoft.Ajax;

namespace MPAS
{
    public partial class GroupChat : System.Web.UI.Page, IMessageReceiver
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
            // register this page with the Chatroom
            cr.RegisterReceiver(this);
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
                ChatroomLabel.Text += "<div class='col-xs-3 col-md-3'>\n";
                ChatroomLabel.Text += $"<small><b>{m.Source.FirstName} {m.Source.Surname}</b> ({m.SendTime.ToShortTimeString()}, {m.SendTime.ToShortDateString()}):</small>";
                //add the message
                ChatroomLabel.Text += "</div>\n<div class='col-xs-9 col-md-9'>";
                ChatroomLabel.Text += $"{m.MessageContent}";
                ChatroomLabel.Text += "</div>\n</div>\n";
            }
        }

        /*
         * Send the contents of the message box to the chatroom
         */
        protected void SendButtonClick(object sender, EventArgs e)
        {
            ChatMessage newMsg = new ChatMessage(cr);
            newMsg.Source = currentUser;
            newMsg.MessageContent = MessageBox.Text;
            MessageBox.Text = "";
            newMsg.SendTime = DateTime.Now;
            cr.Receive(newMsg);
        }

        // from the interface; receive a message sent to the chatroom
        public void Receive(Message m)
        {
            ChatMessage cMsg = (ChatMessage)m;
            ChatroomLabel.Text += "<div class='row'>\n";
            // add the sender name and date
            ChatroomLabel.Text += "<div class='col-xs-3 col-md-3'>\n";
            ChatroomLabel.Text += $"<small><b>{m.Source.FirstName} {m.Source.Surname}</b> ({m.SendTime.ToShortTimeString()}, {m.SendTime.ToShortDateString()}):</small>";
            //add the message
            ChatroomLabel.Text += "</div>\n<div class='col-xs-9 col-md-9'>";
            ChatroomLabel.Text += $"{cMsg.MessageContent}";
            ChatroomLabel.Text += "</div>\n</div>\n";
        }
    }
}