using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.infrastructure.Configurations;
using Zaker_Academy.infrastructure.Entities;

namespace Zaker_Academy.infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<applicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new StudentConfigration());
            modelBuilder.Entity<Comment>().HasMany(s => s.Replies).WithOne().HasForeignKey(f => f.applicationUserId).HasForeignKey(f => f.CommentId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Course>().HasMany(c => c.Comments).WithOne().HasForeignKey(f => f.CourseId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        internal Task<object> FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        public DbSet<Category> categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizeQuestion> QuizQuestions { get; set; }
        public DbSet<QuestionOptions> QuestionOptions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentQuizScore> studentQuizScores { get; set; }
        public DbSet<EnrollmentCourses> EnrollmentCourses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
    }
}