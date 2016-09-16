using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MPAS.Models
{
    public abstract class User
    {
        // profile details
        private string firstname, surname, pathToPicture, studentNumber, degree;
        // group
        private MentorGroup group;
        private int groupNumber; // redundant but temporary 
        private DateTime dateOfBirth;
        private Schedule schedule;

        [StringLength(20), Display(Name = "First Name")]
        public string FirstName {
            get { return firstname; }
            set { firstname = value; }
        }

        [StringLength(20), Display(Name = "Surname")]
        public string Surname
        {
            get
            {
                return surname;
            }

            set
            {
                surname = value;
            }
        }
        
        [StringLength(100), ScaffoldColumn(false)]
        public string PathToPicture
        {
            get
            {
                return pathToPicture;
            }

            set
            {
                pathToPicture = value;
            }
        }

        [Required, StringLength(9), Display(Name = "Student Number")]
        public string StudentNumber
        {
            get
            {
                return studentNumber;
            }

            set
            {
                studentNumber = value;
            }
        }

        [StringLength(20), Display(Name = "Degree")]
        public string Degree
        {
            get
            {
                return degree;
            }

            set
            {
                degree = value;
            }
        }

        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth
        {
            get
            {
                return dateOfBirth;
            }

            set
            {
                dateOfBirth = value;
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

        public int GroupNumber
        {
            get
            {
                return groupNumber;
            }

            set
            {
                groupNumber = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is User)) return false;

            return ((User)obj).StudentNumber.Equals(this.StudentNumber);
        }
    }
}