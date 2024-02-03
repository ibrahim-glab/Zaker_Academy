using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zaker_Academy.core.Entities;
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
            modelBuilder.ApplyConfiguration<Course>(new CourseConfiguration());
            modelBuilder.Entity<Comment>().HasMany(s => s.Replies).WithOne();
            modelBuilder.Entity<Comment>().HasOne<applicationUser>().WithMany();
            modelBuilder.Entity<Reply>().HasOne<applicationUser>().WithMany();
            modelBuilder.Entity<Course>().HasMany(c => c.Comments).WithOne();
            modelBuilder.Entity<Lesson>().HasIndex(p => p.OrderInCourse);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Category> categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizeQuestion> QuizQuestions { get; set; }
        public DbSet<QuestionOptions> QuestionOptions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<StudentQuizScore> studentQuizScores { get; set; }
        public DbSet<EnrollmentCourses> EnrollmentCourses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
    }
}