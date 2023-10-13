using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.infrastructure.Entities
{
    public class EnrollmentCourses
    {
        public int id { get; set; }
        public Student Student { get; set; }
        public string StudentId { get; set; }

        public Course Course { get; set; }
        public int CourseId { get; set; }

        public DateTime EnrolmentDate { get; set; }

        public bool CompleteIt { get; set; }
    }
}