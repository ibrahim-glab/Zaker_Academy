using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.Service.DTO_s
{
    public class CourseBasicUpdateDto
    {
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [Range(1, int.MaxValue)]
      
        public int EnrollmentCapacity { get; set; } = 0;
        [MaxLength(100)]
        public string CourseStatus { get; set; }
        [Range(1, int.MaxValue)]

        public int CourseDurationInHours { get; set; } = 0;

        public string ImageUrl { get; set; }
        [Range(1.0, int.MaxValue)]

        public decimal Price { get; set; } = 0;

        [Range(1.0, int.MaxValue)]

        public decimal Discount { get; set; } = 0;
        [Required]
        public Nullable<bool> Is_paid { get; set; }
      
    }
}
