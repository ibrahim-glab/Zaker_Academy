using System.ComponentModel.DataAnnotations;

namespace Zaker_Academy.infrastructure.Entities
{
    public class QuizeQuestion
    {
        [Key]
        public int Id { get; set; }

        [Required]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string QuestionText { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        // Navigation properties
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Quiz Quiz { get; set; } // Represents the quiz to which the question belongs
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        //public ICollection<QuestionOption> Options { get; set; } // Represents options for the question
        // Add more navigation properties as needed

        // Constructors, additional methods, and custom validation
    }
}