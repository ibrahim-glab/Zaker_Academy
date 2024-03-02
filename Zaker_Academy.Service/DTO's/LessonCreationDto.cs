using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.Service.DTO_s
{
    public class LessonCreationDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
        [Range(1 , 300)]
        public int LessonDuration { get; set; } // Duration of the lesson in minutes
        [Range(1, 500)]

        public string ResourcesUrl { get; set; } // URL for additional resources
    }
}
