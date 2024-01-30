using System.ComponentModel.DataAnnotations;

namespace Zaker_Academy.infrastructure.Entities
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public int enrollmentCapacity { get; set; }
        public string courseStatus { get; set; }
        public string courseDurationInHours { get; set; }
        public string imageUrl { get; set; }
        public bool Is_paid { get; set; }
        public decimal price { get; set; }
        public decimal discount { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string InstructorId { get; set; }

        // Navigation properties
        public applicationUser Instructor { get; set; }
        public int CategoryId { get; set; }


        public Category? Category { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Comment> Comments { get; set; } // Represents the lessons/modules within the course

        // Represents the lessons/modules within the course
        public ICollection<applicationUser> Students { get; set; } // Represents the lessons/modules within the course

        public ICollection<Review>? Reviews { get; set; } // Represents reviews and ratings for the course
    }
}