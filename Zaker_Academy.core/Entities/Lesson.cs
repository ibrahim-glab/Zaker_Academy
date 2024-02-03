using System.ComponentModel.DataAnnotations;

namespace Zaker_Academy.infrastructure.Entities
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public int CourseId { get; set; } // Foreign key to link the lesson to a course

        public int LessonDuration { get; set; } // Duration of the lesson in minutes

        public int OrderInCourse { get; set; } // Order of the lesson within the course

        public string ResourcesUrl { get; set; } // URL for additional resources

        // Navigation properties
        public Course Course { get; set; } // Represents the course to which the lesson belongs

        public ICollection<Quiz> Quizzes { get; set; } // Represents quizzes associated with the lesson
    }
}