using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    public class ResultViewModel
    {
        public int StudentId { get; set; }
        public string ExamName { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswer { get; set; }
        public int WrongAnswer { get; set; }
        public int? ExamsId { get; set; }
    }
}
