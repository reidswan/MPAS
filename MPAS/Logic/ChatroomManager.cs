using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MPAS.Models;
using System.Threading;

namespace MPAS.Logic
{
    public class ChatroomManager
    {
        private static Dictionary<int, Chatroom> rooms = new Dictionary<int, Chatroom>();
        private static ReaderWriterLock lck = new ReaderWriterLock();
        public static Chatroom GetChatroom(int groupNumber)
        {
            lck.AcquireReaderLock(20);
            if (rooms.ContainsKey(groupNumber))
            {
                lck.ReleaseReaderLock();
                return rooms[groupNumber];
            }
            else
            {
                lck.ReleaseReaderLock();
                lck.AcquireWriterLock(20);
                Chatroom cr;
                // this is in case somehow another object had the writer lock after this reader lock was released
                // and added the chatroom object 
                if (!rooms.ContainsKey(groupNumber))
                {
                    cr = DatabaseUtilities.GetChatroom(groupNumber);
                    rooms.Add(groupNumber, cr);
                } else
                {
                    cr = rooms[groupNumber];
                }
                lck.ReleaseWriterLock();
                return cr;
            }
        }
    }
}