using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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