using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.infrastructure.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public int enrollmentCapacity { get; set; }
        public string courseStatus { get; set; }
        public string courseDurationInHours { get; set; }
        public string imageUrl { get; set; }
        public decimal price { get; set; }
        public decimal discount { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Required]
        public int InstructorId { get; set; }

        // Navigation properties
        public Instructor Instructor { get; set; }

        public Category? Category { get; set; }

        //  public Category category { get; set; }

        public ICollection<Lesson> Lessons { get; set; } // Represents the lessons/modules within the course
        public ICollection<Student>? Students { get; set; } // Represents the students enrolled in the course

        public ICollection<Review>? Reviews { get; set; } // Represents reviews and ratings for the course
        // Add more navigation properties as needed
    }
}