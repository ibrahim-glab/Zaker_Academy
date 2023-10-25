﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.ErrorHandling;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IInstructorService instructorService;
        private readonly IStudentService studentService;

        public UserController(IInstructorService instructorService, IStudentService studentService)
        {
            this.instructorService = instructorService;
            this.studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserCreationDto user)
        {
            if (user is null)
                return BadRequest();
            if (!ModelState.IsValid)
                BadRequest(ModelState);

            try
            {
                if (!(string.Equals(user.Role, "instructor", StringComparison.CurrentCultureIgnoreCase) || string.Equals(user.Role, "student", StringComparison.CurrentCultureIgnoreCase)))
                    throw new Exception(message: "Invalid Role ");
                ServiceResult result = new ServiceResult();
                if (string.Equals(user.Role, "instructor", StringComparison.CurrentCultureIgnoreCase))
                {
                    result = await instructorService.Register(user);
                }
                else
                {
                    result = await studentService.Register(user);
                }

                if (!result.succeeded)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(ModelState);
            }
        }
    }
}