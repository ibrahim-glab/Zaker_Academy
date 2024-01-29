using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using Zaker_Academy.core.Interfaces;

namespace Zaker_Academy.infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ICategoryRepository CategoryRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }
        public ICourseRepository CourseRepository { get; private set; }
        public IEnrollmentCoursesRepository EnrollmentCoursesRepository { get; private set; }
        public ILessonRepository LessonRepository { get; private set; }
        public IQuestionOptionRepository QuestionOptionsRepository { get; private set; }
        public IQuizRepository QuizRepository { get; private set; }
        public IQuizQuestionRepository QuizeQuestionRepository { get; private set; }
        public IReplyRepository ReplyRepository { get; private set; }
        public IStudentQuizScoreRepository StudentQuizScoreRepository { get; private set; }
        public IInstructorRepository InstructorRepository { get; private set; }
        public IStudentRepository StudentRepository { get; private set; }
        public IReviewRepository ReviewRepository { get; private set; }

        public ISubCategoryRepository SubCategoryRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(_context);
            CommentRepository = new CommentRepository(_context);
            CourseRepository = new CourseRepository(_context);
            EnrollmentCoursesRepository = new EnrollmentCoursesRepository(_context);
            LessonRepository = new LessonRepository(_context);
            QuestionOptionsRepository = new QuestionOptionRepository(_context);
            QuizeQuestionRepository = new QuizQuestionRepository(_context);
            ReplyRepository = new ReplyRepository(_context);
            StudentQuizScoreRepository = new StudentQuizScoreRepository(_context);
            QuizRepository = new QuizRepository(_context);
            InstructorRepository = new InstructorRepository(_context);
            StudentRepository = new StudentRepository(_context);
            ReviewRepository = new ReviewRepository(_context);
            SubCategoryRepository = new SubCategoryRepository(_context);
        }

        public IDbTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}