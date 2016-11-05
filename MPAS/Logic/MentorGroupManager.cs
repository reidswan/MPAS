using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MPAS.Models;

namespace MPAS.Logic
{
    // Class for cacheing MentorGroup objects so that multiple references to the same
    // mentorgroup point to the same object
    public class MentorGroupManager
    {
        private static Dictionary<int, MentorGroup> groups = new Dictionary<int, MentorGroup>();

        public static MentorGroup GetGroup(int groupNumber)
        {
            //if (groupNumber == 0) return MentorGroup.GENERAL;
            if (groups.ContainsKey(groupNumber))
            {
                return groups[groupNumber];
            } else
            {
                MentorGroup g = DatabaseUtilities.GetGroup(groupNumber);
                if (g != null)
                { 
                    groups[groupNumber] = g;
                }
                return g;
            }
        }
    }
}