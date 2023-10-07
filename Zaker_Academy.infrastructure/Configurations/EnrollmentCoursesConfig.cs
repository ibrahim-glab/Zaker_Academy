using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.infrastructure.Entities;

namespace Zaker_Academy.infrastructure.Configurations
{
    public class EnrollmentCoursesConfig : IEntityTypeConfiguration<EnrollmentCourses>
    {
        public void Configure(EntityTypeBuilder<EnrollmentCourses> builder)
        {
        }
    }
}