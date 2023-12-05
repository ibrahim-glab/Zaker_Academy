using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.infrastructure.Entities
{
    public class StudentQuizScore
    {
        public int Id { get; set; }
        public applicationUser student { get; set; }
        public Quiz Quiz { get; set; }
        public decimal score { get; set; }
        public DateTime CompleteAt { get; set; }
    }
}