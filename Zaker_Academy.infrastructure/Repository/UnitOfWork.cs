using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Entities;

namespace Zaker_Academy.infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenericRepository<Category> CategoryRepository { get; private set; }
        public IGenericRepository<Comment> CommentRepository { get; private set; }
        public IGenericRepository<Course> CourseRepository { get; private set; }
        public IGenericRepository<EnrollmentCourses> EnrollmentCoursesRepository { get; private set; }
        public IGenericRepository<Lesson> LessonRepository { get; private set; }
        public IGenericRepository<QuestionOptions> QuestionOptionsRepository { get; private set; }
        public IGenericRepository<Quiz> QuizRepository { get; private set; }
        public IGenericRepository<QuizeQuestion> QuizeQuestionRepository { get; private set; }
        public IGenericRepository<Reply> ReplyRepository { get; private set; }
        public IGenericRepository<StudentQuizScore> StudentQuizScoreRepository { get; private set; }
        public IGenericRepository<Instructor> InstructorRepository { get; private set; }
        public IGenericRepository<Student> StudentRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepository = new GenericRepository<Category>(_context);
            CommentRepository = new GenericRepository<Comment>(_context);
            EnrollmentCoursesRepository = new GenericRepository<EnrollmentCourses>(_context);
            LessonRepository = new GenericRepository<Lesson>(_context);
            QuestionOptionsRepository = new GenericRepository<QuestionOptions>(_context);
            QuizeQuestionRepository = new GenericRepository<QuizeQuestion>(_context);
            ReplyRepository = new GenericRepository<Reply>(_context);
            StudentQuizScoreRepository = new GenericRepository<StudentQuizScore>(_context);
            QuizRepository = new GenericRepository<Quiz>(_context);
            InstructorRepository = new GenericRepository<Instructor>(_context);
            StudentRepository = new GenericRepository<Student>(_context);
        }
    }
}