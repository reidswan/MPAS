using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MPAS.Models
{
    public class Chatroom : MessageHandler
    {
        MentorGroup group;

        public Chatroom(MentorGroup group)
        {
            this.group = group;
            base.messages = new List<ChatMessage>();
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