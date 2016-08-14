using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public class DirectChat : MessageHandler
    {
        User user1, user2;
        public DirectChat(User u1, User u2)
        {
            user1 = u1;
            user2 = u2;
            base.messages = new List<ChatMessage>();
        }
    }
}