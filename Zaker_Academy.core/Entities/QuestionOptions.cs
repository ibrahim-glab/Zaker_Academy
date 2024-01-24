using System.ComponentModel.DataAnnotations;

namespace Zaker_Academy.infrastructure.Entities
{
    public class QuestionOptions
    {
        [Key]
        public int Id { get; set; }

        [Required]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string OptionText { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required]
        public int QuestionId { get; set; } // Foreign key to link the option to a question

        // Additional properties for indicating whether this option is the correct answer
        public bool IsCorrect { get; set; }

        // Navigation properties
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public QuizeQuestion Question { get; set; } // Represents the question to which the option belongs
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        // Add more navigation properties as needed

        // Constructors, additional methods, and custom validation c
    }
}