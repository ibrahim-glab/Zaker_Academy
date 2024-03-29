﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.Service.DTO_s
{
    public class CourseDto
    {

        public int Id { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }
             
        public int EnrollmentCapacity { get; set; } = 0;
     
        public string CourseStatus { get; set; }
       
        public int CourseDurationInHours { get; set; } = 0;

        public string ImageUrl { get; set; }

        public decimal Price { get; set; } = 0;

        public decimal Discount { get; set; } = 0;
        public bool Is_paid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public object Insructor { get; set; }
        public object Category { get; set; }
        public object SubCategory { get; set; }
    }
}
