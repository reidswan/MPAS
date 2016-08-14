using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MPAS.Models
{
    public class MentorGroup
    {
        static int nextID = 0;
        Mentor mentor;
        List<Mentee> mentees;
        List<GroupActivity> activities;
        int id;
        Chatroom chatroom;
        public static MentorGroup GENERAL = new MentorGroup
        {
            Id = 0,
            Mentor = null,
            Mentees = Mentee.ALL,
            Activities = new List<GroupActivity>(),
            Chatroom = new Chatroom(GENERAL)
        };

        public Mentor Mentor
        {
            get
            {
                return mentor;
            }

            set
            {
                mentor = value;
            }
        }

        public List<Mentee> Mentees
        {
            get
            {
                return mentees;
            }

            set
            {
                mentees = value;
            }
        }

        public List<GroupActivity> Activities
        {
            get
            {
                return activities;
            }

            set
            {
                activities = value;
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

        public Chatroom Chatroom
        {
            get
            {
                return chatroom;
            }

            set
            {
                chatroom = value;
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

        public MentorGroup()
        {
            Id = ++NextID;
            Mentor = null;
            Mentees = new List<Mentee>();
            Activities = new List<GroupActivity>();
        }

        /*
         * Adds a Mentee to this MentorGroup
         */ 
        public void AddMentee(Mentee m)
        {
            m.Group = this;
            Mentees.Add(m);
        }

        /*
         * Changes the mentor of this MentorGroup to the provided one
         */
        public void SetMentor(Mentor m)
        {
            Mentor = m;
            Mentor.Group = this;
        }

        /*
         * 
         */ 
        public void RemoveMentee(Mentee m)
        {
            // todo : implement
        }

        public void AddActivity(GroupActivity a)
        {
            this.Activities.Add(a);
        }

        public GroupActivity GetActivity(int actID)
        {
            foreach(GroupActivity g in this.Activities)
            {
                if (g.ID == Id) return g;
            }

            throw new KeyNotFoundException("Activity with ID " + actID + " is not a member of group " + this.Id);
        }

        public override string ToString()
        {
            if (Id == 0) return "General";
            else return "Group " + Id;
        }

    }
}