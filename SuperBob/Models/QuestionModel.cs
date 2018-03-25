using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperBob.Models
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public DateTime Posted { get; set; }
        public string QuestionBody { get; set; }
    }
}