using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MPAS.Models;

namespace MPAS.Logic
{
    public class ChatroomManager
    {
        private static Dictionary<int, Chatroom> rooms = new Dictionary<int, Chatroom>();

        public static Chatroom GetChatroom(int groupNumber)
        {
            if (rooms.ContainsKey(groupNumber))
            {
                return rooms[groupNumber];
            } else
            {
                Chatroom cr = DatabaseUtilities.GetChatroom(groupNumber);
                rooms.Add(groupNumber, cr);
                return cr;
            }
        }
    }
}