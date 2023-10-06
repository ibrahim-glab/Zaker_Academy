using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.infrastructure.Entities
{
    public class QuizeQuestion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string QuestionText { get; set; }

        // Navigation properties
        public Quiz Quiz { get; set; } // Represents the quiz to which the question belongs

        //public ICollection<QuestionOption> Options { get; set; } // Represents options for the question
        // Add more navigation properties as needed

        // Constructors, additional methods, and custom validation
    }
}