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
      
        public async Task<ServiceResult<string>> AddLesson(int course_id, LessonCreationDto lesson)
        {
            var transction =  unitOfWork.BeginTransaction();
            try
            {
                var course = await unitOfWork.CourseRepository.GetByCondition(c => c.CourseId == course_id, sel => new { sel.CourseId });
                if (!course.Any())
                    return new ServiceResult<string> { succeeded = false, Error = "The Course Not Found" };
             
                var Lesson = mapper.Map<Lesson>(lesson);
                Lesson.CourseId = course_id;
                await unitOfWork.LessonRepository.Add(Lesson);
                await unitOfWork.SaveChanges();
                transction.Commit();
                return new ServiceResult<string> { succeeded = true };
            }
            catch (Exception)
            {
                transction.Rollback();
                throw;
            }
            
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

        public async Task<ServiceResult<ICollection<LessonDto>>> GetRelatedLessons(int course_id)
        {
            var course = await unitOfWork.CourseRepository.GetByCondition(c => c.CourseId == course_id, sel => new { sel.CourseId });
            if (!course.Any())
                return new ServiceResult<ICollection<LessonDto>> { succeeded = false, Error = "The Course Not Found" };
            var Lessons = await unitOfWork.LessonRepository.GetByCondition(Les => Les.CourseId == course_id, sel => mapper.Map<LessonDto>(sel));
            return new ServiceResult<ICollection<LessonDto>> { succeeded = true, Data = Lessons };
           
        }

        public async Task<ServiceResult<LessonDto>> UpadteRelatedLesson(int course_id, LessonDto lessonDto)
        {

            var transction = unitOfWork.BeginTransaction();
            try
            {
                var les = await unitOfWork.LessonRepository.GetByCondition(l => l.Id == lessonDto.Id && l.CourseId == course_id);
                if (!les.Any())
                    return new ServiceResult<LessonDto> { succeeded = false, Error = "The Lesson Not Found" };
               var lesson1 = les.First();
               var lesson = mapper.Map<LessonDto, Lesson>(lessonDto, les.First());
                await unitOfWork.LessonRepository.Update(lesson);
                await unitOfWork.SaveChanges();
                transction.Commit();
                return new ServiceResult<LessonDto> { succeeded = true, Data = lessonDto };
            }
            catch (Exception)
            {
                transction.Rollback();
                throw;
            }
           

                
                    
        }

        public async Task<ServiceResult<string>> Update(int course_id, CourseBasicUpdateDto courseUpdate)
        {
            var course = await unitOfWork.CourseRepository.GetByIdAsync(course_id);
            if (course is null)
                return new ServiceResult<string> { succeeded = false, Message = "The Course Not found" };
            var Course = mapper.Map<CourseBasicUpdateDto , Course>(courseUpdate , course);
            await unitOfWork.CourseRepository.Update(Course);
            course.UpdatedAt = DateTime.UtcNow;
            await unitOfWork.SaveChanges();
            return new ServiceResult<string> { succeeded = true };
        }

        public async Task<ServiceResult<List<LessonDto>>> UpdateRelatedLessons(int course_id, List<LessonDto> lessonDto)
        {
            var newlessons = new List<Lesson>();
            foreach (var item in lessonDto)
            {
                var lesson = await unitOfWork.LessonRepository.GetByCondition(l => l.CourseId == course_id && l.Id == item.Id);
                if (lesson.Any())
                {
                    var Lesson = mapper.Map<LessonDto, Lesson>(item, lesson.First());
                    newlessons.Add(Lesson);
                }
            }
            if (newlessons.Count == 0)
                return new ServiceResult<List<LessonDto>> { succeeded = false };
            await unitOfWork.LessonRepository.BulkUpdate(newlessons);
            return new ServiceResult<List<LessonDto>> { succeeded = true, Data = lessonDto };
        }
    }
}