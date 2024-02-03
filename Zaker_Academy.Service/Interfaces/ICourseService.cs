using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.ErrorHandling;

namespace Zaker_Academy.Service.Interfaces
{
    public interface ICourseService
    {
        Task<ServiceResult<CourseDto>> Create(string userid,CourseCreationDTO creationDTO);
        Task<ServiceResult<CourseDto>> GetCourse(int Course_id);
        Task<ServiceResult<IEnumerable<CourseDto>>> GetAllCourse();
        Task<ServiceResult<string>> Delete(int CourseId);
        Task<ServiceResult<string>> Update(int course_id , CourseBasicUpdateDto courseUpdate );
        Task<ServiceResult<string>> AddLesson(int course_id , LessonCreationDto lesson );
        Task<ServiceResult<ICollection<LessonDto>>> GetRelatedLessons(int course_id );
        Task<ServiceResult<LessonDto>> UpadteRelatedLesson(int course_id , LessonDto lessonDto );
    }
}