using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public class PrivateMessage : Message
    {
        private User destination;

        public User Destination
        {
            get
            {
                return destination;
            }

            set
            {
                destination = value;
            }
        }

        public override void Send()
        {
            return;
        }
    }
}