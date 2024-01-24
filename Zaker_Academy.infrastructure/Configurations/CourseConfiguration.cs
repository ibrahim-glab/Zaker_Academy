using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zaker_Academy.infrastructure.Entities;

namespace Zaker_Academy.infrastructure.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasOne(i => i.Instructor).WithMany().HasForeignKey(i => i.InstructorId);
        }
    }
}