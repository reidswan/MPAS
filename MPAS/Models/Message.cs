using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public abstract class Message
    {
        string messageContent;
        User source;
        DateTime sendTime;

        public string MessageContent
        {
            get
            {
                return messageContent;
            }

            set
            {
                messageContent = value;
            }
        }

        public User Source
        {
            get
            {
                return source;
            }

            set
            {
                source = value;
            }
        }

        public DateTime SendTime
        {
            get
            {
                return sendTime;
            }

            set
            {
                sendTime = value;
            }
        }

        public abstract void Send();
    }
}