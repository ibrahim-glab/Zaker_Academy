using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CourseCreationDTO courseCreationDTO )
        {
            if (!ModelState.IsValid)           
                return BadRequest(ModelState);
            if (courseCreationDTO.CategoryId == 0)
                return BadRequest();
            if (courseCreationDTO.SubCategoryId == 0)
                return BadRequest();
            try
            {
                var res = await courseService.Create(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!, courseCreationDTO);
                if (!res.succeeded)
                    return BadRequest(res);
                return Ok();
            }
            catch (Exception)
            {

                return StatusCode(500); ;
            }                    
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
         if (id == 0)
                return BadRequest();
            
            try
            {
                var res = await courseService.GetCourse(id);
                if (!res.succeeded)
                    return NotFound(res);
                return Ok(res);
            }
            catch (Exception)
            {

                return StatusCode(500); ;
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var res = await courseService.GetAllCourse();             
                return Ok(res);
            }
            catch (Exception)
            {
                return StatusCode(500); ;
            }
        }
        [Authorize(Roles ="Instructor")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id , [FromBody] CourseBasicUpdateDto courseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var res = await courseService.Update(id , courseDto);
                if (!res.succeeded)
                    return BadRequest(res);
                return Ok(res);
            }
            catch (Exception)
            {
                return StatusCode(500); ;
            }
        }
        [Authorize(Roles = "Instructor")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var res = await courseService.Delete(id);
                if (!res.succeeded)
                    return NotFound(res);
                return Ok(res);
            }
            catch (Exception e)
            {
                return StatusCode(500); ;
            }
        }


        [Authorize(Roles = "Instructor")]
        [HttpPost("{id:int}/Lessons")]
        public async Task<IActionResult> Post(int id , LessonCreationDto Lesson)
        {
            if (id <= 0)
                return BadRequest(error: "Invalid  id of Course");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var res = await courseService.AddLesson(id , Lesson);
                if (!res.succeeded)
                    return NotFound(res);
                return Ok(res);
            }
            catch (Exception e)
            {
                return StatusCode(500); ;
            }
        }
        [HttpGet("{id:int}/Lessons")]
        public async Task<IActionResult> GetLessons(int id)
        {
            if (id <= 0)
                return BadRequest(error: "Invalid  id of Course");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var res = await courseService.GetRelatedLessons(id);
                if (!res.succeeded)
                    return NotFound(res);
                return Ok(res);
            }
            catch (Exception e)
            {
                return StatusCode(500); ;
            }
        }

        [Authorize(Roles = "Instructor")]
        [HttpPut("{id:int}/Lessons")]
        public async Task<IActionResult> Put(int id, [FromBody] LessonDto lessonDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var res = await courseService.UpadteRelatedLesson(id, lessonDto);
                if (!res.succeeded)
                    return NotFound(res);
                return Ok(res);
            }
            catch (Exception)
            {
                return StatusCode(500); ;
            }
        }
    }
}