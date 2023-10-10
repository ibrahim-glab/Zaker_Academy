using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.infrastructure.Entities
{
    public class Student : applicationUser
    {
        public ICollection<Course> Courses { get; set; }
    }
}