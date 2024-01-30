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
                var res = await courseService.Create(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, courseCreationDTO);
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
        public async Task<IActionResult> Post(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id == 0)
                return BadRequest();
            
            try
            {
                var res = await courseService.GetCourse(id);
                if (!res.succeeded)
                    return BadRequest(res);
                return Ok(res);
            }
            catch (Exception)
            {

                return StatusCode(500); ;
            }
        }

    }
}