using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MPAS.Logic;

namespace MPAS.Models 
{
    /*
     * A private chat between two users
     */
    public class PrivateChat : MessageHandler
    {
        private User user1, user2;
        public PrivateChat(User user1, User user2)
        {
            this.user1 = user1;
            this.user2 = user2;
            base.receivers = new List<IMessageReceiver>();
            base.messages = new List<Message>();
        }

        public override void Receive(Message msg)
        {
            PrivateMessage pMsg = (PrivateMessage)msg;
            base.Receive(pMsg);
            DatabaseUtilities.SendPrivateMessage(pMsg.Source.StudentNumber, pMsg.Destination.StudentNumber, pMsg.MessageContent, pMsg.SendTime);
        }

        public void AddMessage(Message msg)
        {
            base.messages.Add(msg);
        }

        public List<Message> Messages
        {
            get
            {
                return base.messages;
            }
        }
    }
}