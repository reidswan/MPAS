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
            base.messages = new List<ChatMessage>();
        }

        /**
         * calls the MessageHandler.Receive method to receive a message and then adds the message to the Chatrooms database
         */
        public override void Receive(ChatMessage msg)
        {
            base.Receive(msg);
            DatabaseUtilities.SendChatroomMessage(msg.Source.GroupNumber, msg.Source.StudentNumber, msg.MessageContent, msg.SendTime);
        }

        public List<ChatMessage> Messages
        {
            get
            {
                return base.messages;
            }
        }
    }
}