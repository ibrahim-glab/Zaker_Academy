using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.infrastructure.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int rating { get; set; }
        public string review { get; set; }
        public DateTime createdIt { get; set; }
    }
}