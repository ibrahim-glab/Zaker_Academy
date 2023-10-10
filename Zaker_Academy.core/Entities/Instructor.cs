using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.infrastructure.Entities
{
    public class Instructor : applicationUser
    {
        [MaxLength(1000)]
        public string AboutMe { get; set; }

        public ICollection<Student>? Students { get; set; }
        public ICollection<Course>? instructorCourses { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}