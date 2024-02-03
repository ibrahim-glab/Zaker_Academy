using System.ComponentModel.DataAnnotations;
using Zaker_Academy.core.Entities;

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
        public int courseDurationInHours { get; set; }
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
        public int SubCategoryId { get; set; }
        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }

        public ICollection<Chapter>? Chapters { get; set; }
        public ICollection<Comment>? Comments { get; set; }

        public ICollection<applicationUser>? Students { get; set; } 

        public ICollection<Review>? Reviews { get; set; } 
    }
}