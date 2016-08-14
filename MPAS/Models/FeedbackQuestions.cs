using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAS.Models
{
    public class FeedbackQuestions
    {
        List<string> questions;

        public FeedbackQuestions()
        {
            this.questions = new List<string>();
        }

        public List<string> Questions
        {
            get
            {
                return questions;
            }
        }
    }
}