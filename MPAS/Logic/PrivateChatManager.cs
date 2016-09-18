using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MPAS.Models;
using System.Threading;

namespace MPAS.Logic
{
    public class PrivateChatManager
    {
        private static Dictionary<Tuple<string, string>, PrivateChat> privateChats = new Dictionary<Tuple<string, string>, PrivateChat>();
        private static ReaderWriterLock lck = new ReaderWriterLock();
        /**
         * If a PrivateChat object exists between two users, returns it, otherwise, 
         * creates it from the database and adds it to the dictionary and returns it
         */
        public static PrivateChat GetChat(string stdNum1, string stdNum2)
        {
            string s1 = stdNum1, s2 = stdNum2;
            if (stdNum1.CompareTo(stdNum2) > 0)
            {
                s1 = stdNum2;
                s2 = stdNum1;
            }

            Tuple<string, string> queryTuple = new Tuple<string, string>(s1, s2);
            lck.AcquireReaderLock(20);
            if (privateChats.ContainsKey(queryTuple))
            {
                lck.ReleaseReaderLock();
                return privateChats[queryTuple];
            } else
            {
                lck.ReleaseReaderLock();
                lck.AcquireWriterLock(20);
                if (!privateChats.ContainsKey(queryTuple))
                {
                    privateChats[queryTuple] = DatabaseUtilities.GetPrivateChat(stdNum1, stdNum2);
                }
                lck.ReleaseWriterLock();
                return privateChats[queryTuple];
            }
        }
    }
}