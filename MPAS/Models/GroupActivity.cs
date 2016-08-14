using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace MPAS.Models
{
    public abstract class GroupActivity
    {
        private static int nextID = 0;
        private string title;
        private bool status;
        private int id;
        private MentorGroup group;
        private User madeBy;
        private DateTime creationDate;

        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
            }
        }

        public User MadeBy
        {
            get
            {
                return madeBy;
            }

            set
            {
                madeBy = value;
            }
        }

        public MentorGroup Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
            }
        }

        public bool Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        public static int NextID
        {
            get
            {
                return nextID;
            }

            set
            {
                nextID = value;
            }
        }

        /*
         * Emails an alert to mentees about the scheduled activity
         */
        public abstract void EmailAlert();
    }
}