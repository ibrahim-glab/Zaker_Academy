using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.infrastructure.Entities
{
    public class Reply
    {
        public int ReplyId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public int CommentId { get; set; } // The comment to which the reply is related

        public string applicationUserId { get; set; }
    }
}