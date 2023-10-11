using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.infrastructure.Entities;

namespace Zaker_Academy.core.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Category> CategoryRepository { get; }
        public IGenericRepository<Comment> CommentRepository { get; }
        public IGenericRepository<Course> CourseRepository { get; }
        public IGenericRepository<EnrollmentCourses> EnrollmentCoursesRepository { get; }
        public IGenericRepository<Lesson> LessonRepository { get; }
        public IGenericRepository<QuestionOptions> QuestionOptionsRepository { get; }
        public IGenericRepository<Quiz> QuizRepository { get; }
        public IGenericRepository<QuizeQuestion> QuizeQuestionRepository { get; }
        public IGenericRepository<Reply> ReplyRepository { get; }
        public IGenericRepository<StudentQuizScore> StudentQuizScoreRepository { get; }
        public IGenericRepository<Instructor> InstructorRepository { get; }
        public IGenericRepository<Student> StudentRepository { get; }
    }
}