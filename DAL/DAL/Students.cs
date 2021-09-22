using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class Students
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string CVFileName { get; set; }
        public string PictureFileName { get; set; }

        //id to distinguish multiple groups 
        public int? GroupsId { get; set; }
        //groups
        public Groups Groups { get; set; }
        //multiple examresults
        public ICollection<ExamResults> ExamResults { get; set; } = new HashSet<ExamResults>();

    }
}
