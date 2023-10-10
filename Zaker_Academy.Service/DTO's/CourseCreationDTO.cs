using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.Service.DTO_s
{
    public class CourseCreationDTO
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public string InstructorId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int EnrollmentCapacity { get; set; }

        [Required]
        public string CourseStatus { get; set; }

        [Required]
        public string CourseDurationInHours { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}