using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
            if (course.SubCategoryId is not null )
                if(await unitOfWork.CategoryRepository.GetByIdAsync(course.CategoryId) is null)
                    return new ServiceResult<CourseDto> { succeeded = false, Error = "Invalid SubCategory Id" };
            var Course = mapper.Map<Course>(course);
            Course.InstructorId = userid;
            await unitOfWork.CourseRepository.Add(Course);

            return new ServiceResult<CourseDto> { succeeded = true };
        }

        public async Task<ServiceResult<ICollection<CourseDto>>> GetAllCourse()
        {
            try
            {
                var Courses = await unitOfWork.CourseRepository.GetByCondition(s => true, course => new CourseDto
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
                return new ServiceResult<ICollection<CourseDto>> { succeeded = true, Data = Courses };
            }
            catch (Exception)
            {

                throw;
            }
           
           
        }

        public async Task<ServiceResult<CourseDto>> GetCourse(int Course_id)
        {
            var coures = await unitOfWork.CourseRepository.GetByCondition(s=>s.CourseId == Course_id, new[] { "Category", "SubCategory" });
            if (coures is null)
                return new ServiceResult<CourseDto> { succeeded = false };
            var Course = coures.First();
            var instructor = await unitOfWork.InstructorRepository.GetByCondition(s => s.Id == Course.InstructorId, s => new { s.Id, s.FirstName, s.LastName });
            var courseDto = mapper.Map<CourseDto>(coures.FirstOrDefault());
            courseDto.Insructor = instructor;
            courseDto.Category = new {id = Course.Category.Id , Name = Course.Category.Name };
            courseDto.SubCategory = new { id = Course.SubCategoryId, Course.SubCategory.Name };
            return new ServiceResult<CourseDto> { Data = courseDto, succeeded = true };
        }
    }
}