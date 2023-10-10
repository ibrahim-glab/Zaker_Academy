using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.infrastructure.Entities
{
    public class QuestionOptions
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OptionText { get; set; }

        [Required]
        public int QuestionId { get; set; } // Foreign key to link the option to a question

        // Additional properties for indicating whether this option is the correct answer
        public bool IsCorrect { get; set; }

        // Navigation properties
        public QuizeQuestion Question { get; set; } // Represents the question to which the option belongs

        // Add more navigation properties as needed

        // Constructors, additional methods, and custom validation c
    }
}