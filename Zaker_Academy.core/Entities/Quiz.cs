using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.infrastructure.Entities
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public DateTime LastUpdate { get; set; }
        public Lesson Lesson { get; set; } // Represents the lesson to which the quiz belongs

        public ICollection<QuizeQuestion> QuizQuestions { get; set; } // Represents quiz questions associated with the quiz
        // Add more navigation properties as needed

        // Constructors, additional methods, and custom validation can be added here
    }
}