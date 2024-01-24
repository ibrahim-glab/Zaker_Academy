using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Entities;

namespace Zaker_Academy.infrastructure.Repository
{
    public class StudentQuizScoreRepository : GenericRepository<StudentQuizScore>, IStudentQuizScoreRepository
    {
        public StudentQuizScoreRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}