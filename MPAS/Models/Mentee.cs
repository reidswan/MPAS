using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public class Mentee : User
    {
        public static List<Mentee> ALL = new List<Mentee>();
        public Mentee(string studentNumber)
        {
            base.StudentNumber = studentNumber;
            ALL.Add(this);
        }

    }
}