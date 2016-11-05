using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public class FeedbackComment
    {
        private string snum, comment;

        //Constructor that sets the snum and comment variables to the string parameters that it takes in
        public FeedbackComment(string snum, string comment)
        {
            this.Snum = snum;
            this.Comment = comment;
        }

        //Getter and setter for the snum variable
        public string Snum
        {
            get
            {
                return snum;
            }
            set
            {
                snum = value;
            }
        }

        //Getter and setter for the comment variable
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
            }
        }

    }
}