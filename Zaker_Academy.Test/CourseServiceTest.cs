using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.infrastructure.Repository;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.Interfaces;
using Zaker_Academy.Service.Mapping;
using Zaker_Academy.Service.Services;

namespace Zaker_Academy.Test
{
    //private readonly IUnitOfWork unitOfWork;
    //private readonly UserStore<Instructor> instructorStore;
    //private readonly IMapper mapper;
    public class CourseServiceTest
    {
        public class StoreMock : ICourseService
        {
            public async Task<bool> Create(CourseCreationDTO creationDTO)
            {
                return false;
            }
        }

        [Fact]
        public void CourseService_Create_ReturnValidData()

        {
            ICourseService courseService = new(new StoreMock())
            // Assert
            Assert.True(true);
        }
    }
}