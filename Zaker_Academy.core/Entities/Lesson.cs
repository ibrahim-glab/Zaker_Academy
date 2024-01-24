using System.ComponentModel.DataAnnotations;

namespace Zaker_Academy.infrastructure.Entities
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Title { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [MaxLength(1000)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required]
        public int CourseId { get; set; } // Foreign key to link the lesson to a course

        public int LessonDuration { get; set; } // Duration of the lesson in minutes

        public int OrderInCourse { get; set; } // Order of the lesson within the course

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string ResourcesUrl { get; set; } // URL for additional resources
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        // Navigation properties
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Course Course { get; set; } // Represents the course to which the lesson belongs
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ICollection<Quiz> Quizzes { get; set; } // Represents quizzes associated with the lesson
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}