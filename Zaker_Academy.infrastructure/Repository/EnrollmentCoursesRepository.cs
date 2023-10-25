﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Entities;

namespace Zaker_Academy.infrastructure.Repository
{
    public class EnrollmentCoursesRepository : GenericRepository<EnrollmentCourses>, IEnrollmentCoursesRepository
    {
        public EnrollmentCoursesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}