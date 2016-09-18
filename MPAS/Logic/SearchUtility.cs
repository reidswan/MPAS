using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Logic
{
    /**
     * Class containing logic for searching for a user given an input query string
     */
    public class SearchUtility
    {
        public static List<string> userStrings = DatabaseUtilities.GetAllUserStrings();

        /**
         *  returns a list of users matching the query string
         */
        public static List<string> Search(string query)
        {
            List<string> res = new List<string>();
            var queryParts = query.Trim().Split(' ');
            foreach(string user in userStrings)
            {
                /*inner:*/
                foreach (string q in queryParts) 
                {
                    if (user.Contains(q))
                    {
                        res.Add(user);
                        break /*inner*/ ;
                    }
                }
            }

            return res;
        }
    }
}