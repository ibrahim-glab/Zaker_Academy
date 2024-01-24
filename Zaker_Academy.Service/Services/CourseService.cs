using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Service.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserStore<applicationUser> instructorStore;
        private readonly IMapper mapper;

        public CourseService(IUnitOfWork unitOfWork, UserStore<applicationUser> userStore, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            instructorStore = userStore;
            this.mapper = mapper;
        }

        public async Task<bool> Create(CourseCreationDTO course)
        {
            if (course == null)
                return false;

            var category = await unitOfWork.CategoryRepository.GetByIdAsync(course.CategoryId);
            if (category == null)
                return false;
            applicationUser? instructor = await instructorStore.FindByIdAsync(course.InstructorId);
            if (instructor == null)
                return false;
            Course Course = mapper.Map<Course>(course);
            try
            {
                await unitOfWork.CourseRepository.Add(Course);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}