using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public class FeedbackItem
    {
        private string question, role, title;

        private int total, count;

        //Constructor that sets total and count variables to 0
        public FeedbackItem()
        {
            this.Count = 0;
            this.Total = 0;
        }
    
        //Overloaded constructor that sets total and count variables to the int parameters that it takes in
        public FeedbackItem(int total, int count)
        {
            this.Count = count;
            this.Total = total;
        }

        //Overloaded construcor that sets question, role, and title variables to the string parameters that it takes in
        public FeedbackItem(string question, string role, string title) : this()
        {
            this.question = question;
            this.role = role;
            this.title = title;
        }

        //Getter and setter for the question variable
        public string Question
        {
            get
            {
                return question;
            }
            set
            {
                question = value;
            }
        }

        //Getter and setter for the total variable
        public int Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
            }
        }

        //Getter and setter for the count variable
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }

        //Getter and setter for the role variable
        public string Role
        {
            get
            {
                return role;
            }
            set
            {
                role = value;
            }
        }

        //Getter and setter for the title variable
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
    }
}