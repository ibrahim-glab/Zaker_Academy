using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.infrastructure.Entities;

namespace Zaker_Academy.core.Interfaces
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public ICommentRepository CommentRepository { get; }
        public ICourseRepository CourseRepository { get; }
        public IEnrollmentCoursesRepository EnrollmentCoursesRepository { get; }
        public ILessonRepository LessonRepository { get; }
        public IQuestionOptionRepository QuestionOptionsRepository { get; }
        public IQuizRepository QuizRepository { get; }
        public IQuizQuestionRepository QuizeQuestionRepository { get; }
        public IReplyRepository ReplyRepository { get; }
        public IStudentQuizScoreRepository StudentQuizScoreRepository { get; }
        public IInstructorRepository InstructorRepository { get; }
        public IStudentRepository StudentRepository { get; }
        public IReviewRepository ReviewRepository { get; }

        IDbTransaction BeginTransaction();

        Task SaveChanges();
    }
}