using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public class Inbox
    {
        Dictionary<string, DirectChat> messageThreads;// studentnumber of recipient, DirectChat holding messages
        User owner;
        public Inbox(User owner)
        {
            this.owner = owner;
        }

        public void AddThread(User other, DirectChat thread)
        {
            this.messageThreads.Add(other.StudentNumber, thread);
        }
    }
}