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
            //var Course = new Course { 
            //    CategoryId = course.CategoryId ,
            //    InstructorId = userid ,
            //    endDate =  course.EndDate.ToDateTime(TimeOnly.MinValue) ,
            //    startDate = course.StartDate.ToDateTime(TimeOnly.MinValue) ,
            //    courseDurationInHours = course.CourseDurationInHours,
            //    courseStatus = course.CourseStatus,
            //    Description = course.Description
            //}
            var Course = mapper.Map<Course>(course);
            Course.InstructorId = userid;
            await unitOfWork.CourseRepository.Add(Course);

            return new ServiceResult<CourseDto> { succeeded = true };
        }

        public async Task<ServiceResult<CourseDto>> GetCourse(int Course_id)
        {
            var coures = await unitOfWork.CourseRepository.GetByCondition(s=>s.CourseId == Course_id, new[] {"Category" , "SubCategory");
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