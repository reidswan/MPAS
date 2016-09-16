using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MPAS.Models
{
    public abstract class MessageHandler
    {
        protected List<Message> messages;
        protected List<IMessageReceiver> receivers;

        public virtual void Receive(Message m)
        {
            this.messages.Add(m);
            foreach(IMessageReceiver rcvr in receivers)
            {
                rcvr.Receive(m);
            }
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            foreach (Message m in messages)
            {
                s.Append(m.Source.FirstName);
                s.Append(" ");
                s.Append(m.Source.Surname);
                s.Append(" (");
                s.Append(m.SendTime.ToShortDateString());
                s.Append(", ");
                s.Append(m.SendTime.ToShortTimeString());
                s.Append("): ");

                s.Append(m.MessageContent);
            }

            return s.ToString();
        }

        /* 
         * Adds a webpage to receive new chat messages
         */
        public void RegisterReceiver(IMessageReceiver rcvr)
        {
            receivers.Add(rcvr);
        }
    }
}