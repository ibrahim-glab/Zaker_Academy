using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.infrastructure.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CourseId { get; set; } // The course to which the comment is related

        public string applicationUserId { get; set; }   // The user who posted the comment
        public ICollection<Reply> Replies { get; set; } // Replies to this comment
    }
}