using Microsoft.Extensions.DependencyInjection;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Repository;

namespace Zaker_Academy.infrastructure
{
    public static class Services
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Register your custom services here
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // Replace with your actual service registration
            services.AddTransient(typeof(ICategoryRepository), typeof(CategoryRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(ISubCategoryRepository), typeof(SubCategoryRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(ICommentRepository), typeof(CommentRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(ICourseRepository), typeof(CourseRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(IEnrollmentCoursesRepository), typeof(EnrollmentCoursesRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(IInstructorRepository), typeof(InstructorRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(ILessonRepository), typeof(LessonRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(IQuizQuestionRepository), typeof(QuizQuestionRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(IReplyRepository), typeof(ReplyRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(IReviewRepository), typeof(ReviewRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(IReplyRepository), typeof(ReplyRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(IStudentQuizScoreRepository), typeof(StudentQuizScoreRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(IQuizRepository), typeof(QuizRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(IQuestionOptionRepository), typeof(QuestionOptionRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(IStudentRepository), typeof(StudentRepository)); // Replace with your actual service registration
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            return services;
        }
    }
}