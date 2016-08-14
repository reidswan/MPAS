using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public class Administrator : User
    {
        // only admin account that may exist
        private static Administrator admin = new Administrator("Admin","", "01360406",0,MentorGroup.GENERAL);

        // a constructor that is hidden for use only by the static admin variable
        private Administrator(string firstName, string surname, string studentNumber, int groupNumber, MentorGroup group)
        {
            base.FirstName = firstName;
            base.Surname = surname;
            base.StudentNumber = studentNumber;
            base.GroupNumber = GroupNumber;
            base.Group = group;
            base.DateOfBirth = DateTime.Now;
            base.Degree = "";
        }

        // access to the admin class
        public static Administrator Get()
        {
            return admin;
        }

        // hide properties of base class to prevent changing the admin account details
        new DateTime DateOfBirth
        {
            get
            {
                return base.DateOfBirth;
            }
        }

        new string FirstName
        {
            get { return base.FirstName; }
        }
        
        new string Surname
        {
            get
            {
                return base.Surname;
            }
        }
        
        new string PathToPicture
        {
            get
            {
                return base.PathToPicture;
            }
        }
        
        new string StudentNumber
        {
            get
            {
                return base.StudentNumber;
            }
        }
        
        new string Degree
        {
            get
            {
                return base.Degree;
            }
        }

        new MentorGroup Group
        {
            get
            {
                return base.Group;
            }
        }

        new int GroupNumber
        {
            get
            {
                return base.GroupNumber;
            }
        }
    }
}