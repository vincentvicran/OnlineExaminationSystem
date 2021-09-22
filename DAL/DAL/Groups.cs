using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class Groups
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //id specified for a user in a group
        public int UsersId { get; set; }

        public Users Users { get; set; }

        //multiple students in a class or group
        public ICollection<Students> Students { get; set; } = new HashSet<Students>();

        public ICollection<Exams> Exams { get; set; } = new HashSet<Exams>();
    }
}
