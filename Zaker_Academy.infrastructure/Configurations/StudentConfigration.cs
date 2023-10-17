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
    public class StudentConfigration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasMany(s => s.Courses)
             .WithMany(c => c.Students)
             .UsingEntity<EnrollmentCourses>("EnrollmentCourses",
             j => j.HasOne(i => i.Course).WithMany().HasForeignKey(j => j.CourseId),
             i => i.HasOne(i => i.Student).WithMany().HasForeignKey(j => j.StudentId).OnDelete(DeleteBehavior.NoAction),
             k =>
             {
                 k.HasKey(k => k.id);
                 k.Property(k => k.EnrolmentDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
             }
             );
        }
    }
}