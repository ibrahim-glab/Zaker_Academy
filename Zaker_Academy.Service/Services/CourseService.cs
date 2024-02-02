using AutoMapper;
using EllipticCurve;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.ErrorHandling;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Service.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<applicationUser> instructorStore;
        private readonly IMapper mapper;

        public CourseService(IUnitOfWork unitOfWork, UserManager<applicationUser> userStore, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            instructorStore = userStore;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<CourseDto>> Create(string userid, CourseCreationDTO course)
        {
            if (await unitOfWork.CategoryRepository.GetByIdAsync(course.CategoryId) is null)
                return new ServiceResult<CourseDto> { succeeded = false, Error = "Invalid Category Id" };
            if (course.SubCategoryId is not null)
                if (await unitOfWork.CategoryRepository.GetByIdAsync(course.CategoryId) is null)
                    return new ServiceResult<CourseDto> { succeeded = false, Error = "Invalid SubCategory Id" };
            var Course = mapper.Map<Course>(course);
            Course.InstructorId = userid;
            await unitOfWork.CourseRepository.Add(Course);

            return new ServiceResult<CourseDto> { succeeded = true };
        }

        public async Task<ServiceResult<string>> Delete(int CourseId)
        {
            var Course = await unitOfWork.CourseRepository.GetByIdAsync(CourseId);
            if (Course is null)
                return new ServiceResult<string> { succeeded = false, Error = "The Course not found" };
            await unitOfWork.CourseRepository.Delete(Course);
            await unitOfWork.SaveChanges();
            return new ServiceResult<string> { succeeded = true };

        }

        public async Task<ServiceResult<IEnumerable<CourseDto>>> GetAllCourse()
        {
            try
            {
                var Courses = await unitOfWork.CourseRepository.GetAll(course => new CourseDto
                {
                    Id = course.CourseId,
                    Title = course.Title,
                    Description = course.Description,
                    EnrollmentCapacity = course.enrollmentCapacity,
                    CourseStatus = course.courseStatus,
                    CourseDurationInHours = course.courseDurationInHours,
                    ImageUrl = course.imageUrl,
                    Is_paid = course.Is_paid,
                    Price = course.price,
                    Discount = course.discount,
                    StartDate = course.startDate,
                    EndDate = course.endDate,
                    UpdatedAt = course.UpdatedAt,

                    // Include only specific columns from the Instructor navigation property
                    Insructor = new
                    {
                        Id = course.Instructor.Id,
                        InstructorName = $"{course.Instructor.FirstName} {course.Instructor.LastName}",
                    },
                    Category = new { Id = course.Category.Id, Name = course.Category.Name },
                    SubCategory = new { Id = course.SubCategoryId!, Name = course.SubCategory.Name },
                }, new[] { "Category", "SubCategory", "Instructor" });
                return new ServiceResult<IEnumerable<CourseDto>> { succeeded = true, Data = Courses };
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<ServiceResult<CourseDto>> GetCourse(int Course_id)
        {
            var coures = await unitOfWork.CourseRepository.GetByCondition(s => s.CourseId == Course_id, new[] { "Category", "SubCategory" });
            if (!coures.Any())
                return new ServiceResult<CourseDto> { succeeded = false, Message = "The Course Not found" };
            var Course = coures.First();
            var instructor = await unitOfWork.InstructorRepository.GetByCondition(s => s.Id == Course.InstructorId, s => new { s.Id, s.FirstName, s.LastName });
            var courseDto = mapper.Map<CourseDto>(coures.FirstOrDefault());
            courseDto.Insructor = instructor;
            courseDto.Category = new { id = Course.Category.Id, Name = Course.Category.Name };
            courseDto.SubCategory = new { id = Course.SubCategoryId, Course.SubCategory.Name };
            return new ServiceResult<CourseDto> { Data = courseDto, succeeded = true };
        }

        public async Task<ServiceResult<string>> Update(int course_id, CourseBasicUpdateDto courseUpdate)
        {
            var course = await unitOfWork.CourseRepository.GetByIdAsync(course_id);
            if (course is null)
                return new ServiceResult<string> { succeeded = false, Message = "The Course Not found" };
            UpdateCourseProperties(course, courseUpdate);
            course.UpdatedAt = DateTime.UtcNow;
            await unitOfWork.SaveChanges();
            return new ServiceResult<string> { succeeded = true };
        }
        private void UpdateCourseProperties(Course course, CourseBasicUpdateDto courseUpdate)
        {
            course.Title = courseUpdate.Title;
            course.Description = courseUpdate.Description;
            course.enrollmentCapacity = courseUpdate.EnrollmentCapacity;
            course.courseStatus = courseUpdate.CourseStatus;
            course.courseDurationInHours = courseUpdate.CourseDurationInHours;
            course.imageUrl = courseUpdate.ImageUrl;
            course.Is_paid = courseUpdate.Is_paid.HasValue ? courseUpdate.Is_paid.Value : course.Is_paid; // Use the existing value if null
            course.price = courseUpdate.Price;
            course.discount = courseUpdate.Discount;
        }

        }
}