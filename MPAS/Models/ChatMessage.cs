using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public class ChatMessage : Message
    {
        Chatroom destination;
        public ChatMessage(Chatroom dest)
        {
            this.destination = dest;
        }
        public override void Send()
        {
            destination.Receive(this);
        }
    }
}