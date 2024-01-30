using System.ComponentModel.DataAnnotations;

namespace Zaker_Academy.Service.DTO_s
{
    public class CourseCreationDTO
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; } = 0;
        [Range(1, int.MaxValue)]

        [Required]
        public int EnrollmentCapacity { get; set; } = 0;
        [MaxLength(100)]
        [Required]
        public string CourseStatus { get; set; }
        [Range(1, int.MaxValue)]

        [Required]
        public int CourseDurationInHours { get; set; } = 0;

        [Required]
        public string ImageUrl { get; set; }
        [Range(0, int.MaxValue)]

        [Required]
        public decimal Price { get; set; } = 0;

        [Required]
        [Range(0, int.MaxValue)]

        public decimal Discount { get; set; } = 0;
        [Required]
        public bool Is_paid { get; set; }
        [Required]
        public DateOnly StartDate { get; set; } 
        [Required]
        public DateOnly EndDate { get; set; } 
        public Nullable<int> SubCategoryId { get; set; }
    }
}