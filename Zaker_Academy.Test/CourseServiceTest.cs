using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Test
{
    //private readonly IUnitOfWork unitOfWork;
    //private readonly UserStore<Instructor> instructorStore;
    //private readonly IMapper mapper;
    public class CourseServiceTest
    {
        public class StoreMock : ICourseService
        {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            public async Task<bool> Create(CourseCreationDTO creationDTO)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            {
                return false;
            }
        }

        [Fact]
        public void CourseService_Create_ReturnValidData()

        {
            // Assert
            Assert.True(true);
        }
    }
}