using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPAS.Models
{
    public interface IMessageReceiver
    {
        void Receive(ChatMessage msg);
    }
}
