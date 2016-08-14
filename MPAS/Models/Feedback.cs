using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public class Feedback
    {
        string addtionalComments;
        List<byte> ratings;

        public Feedback (FeedbackQuestions source)
        {
            this.ratings = new List<byte>();
            foreach (string question in source.Questions)
            {
                ratings.Add(0);
            }
        }

        public void SetRating(int question, byte rating)
        {
            ratings[question] = rating;
        }
    }
}