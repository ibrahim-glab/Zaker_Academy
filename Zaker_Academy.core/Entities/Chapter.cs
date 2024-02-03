using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.infrastructure.Entities;

namespace Zaker_Academy.core.Entities
{
    public class Chapter
    {
        public int ChapterId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        // Foreign key
        public int CourseId { get; set; }

        // Navigation property
        public Course Course { get; set; }

        // Collection navigation property
      //  public ICollection<Lesson>? Lessons { get; set; }
    }
}
