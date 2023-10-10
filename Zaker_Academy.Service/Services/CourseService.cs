using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;

namespace Zaker_Academy.Service.Services
{
    public class CourseService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserStore<Instructor> instructorStore;

        public CourseService(IUnitOfWork unitOfWork, UserStore<Instructor> userStore)
        {
            this.unitOfWork = unitOfWork;
            instructorStore = userStore;
        }

        public async Task<bool> CreateCourse(CourseCreationDTO course)
        {
            if (course == null)
                return false;

            Category category = await unitOfWork.CategoryRepository.GetByIdAsync(course.CategoryId);
            if (category == null)
                return false;
            Instructor? instructor = await instructorStore.FindByIdAsync(course.InstructorId);
            if (instructor == null)
                return false;
            Course Course = new Course();
            return true;
        }
    }
}