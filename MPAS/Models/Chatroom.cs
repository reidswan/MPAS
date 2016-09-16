using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MPAS.Logic;

namespace MPAS.Models
{
    public class Chatroom : MessageHandler
    {
        MentorGroup group;

        public Chatroom(MentorGroup group)
        {
            this.group = group;
            base.receivers = new List<IMessageReceiver>();
            base.messages = new List<Message>();
        }

        /**
         * calls the MessageHandler.Receive method to receive a message and then adds the message to the Chatrooms database
         */
        public override void Receive(Message msg)
        {
            ChatMessage cMsg = (ChatMessage)msg;
            base.Receive(cMsg);
            DatabaseUtilities.SendChatroomMessage(cMsg.Source.GroupNumber, cMsg.Source.StudentNumber, cMsg.MessageContent, cMsg.SendTime);
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