using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using AutoMapper;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.Services;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IUnitOfWork = Zaker_Academy.core.Interfaces.IUnitOfWork;

namespace Zaker_Academy.Tests
{
    [TestClass]
    public class CourseServiceTests
    {
        [TestMethod]
        public async Task CreateCourse_ValidData_ReturnsTrue()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var instructorStoreMock = new Mock<UserStore<Instructor>>(/* Add constructor parameters if needed */);
            var mapperMock = new Mock<IMapper>();

            var courseService = new CourseService(unitOfWorkMock.Object, instructorStoreMock.Object, mapperMock.Object);

            var courseCreationDTO = new CourseCreationDTO
            {
                // Set the properties of the course creation DTO for a valid test case
            };

            unitOfWorkMock.Setup(u => u.CategoryRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Category()); // Simulate a valid category

            instructorStoreMock.Setup(i => i.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Instructor()); // Simulate a valid instructor

            unitOfWorkMock.Setup(u => u.CourseRepository.Add(It.IsAny<Course>()))
                .Returns(Task.CompletedTask); // Simulate a successful course addition

            // Act
            var result = await courseService.CreateCourse(courseCreationDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CreateCourse_NullCourse_ReturnsFalse()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var instructorStoreMock = new Mock<UserStore<Instructor>>(/* Add constructor parameters if needed */);
            var mapperMock = new Mock<IMapper>();

            var courseService = new CourseService(unitOfWorkMock.Object, instructorStoreMock.Object, mapperMock.Object);

            // Act
            var result = await courseService.CreateCourse(null);

            // Assert
            Assert.IsFalse(result);
        }

        // Add more test cases to cover other scenarios, such as invalid category, invalid instructor, exceptions, etc.
    }
}