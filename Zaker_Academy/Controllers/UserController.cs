using Microsoft.AspNetCore.Http;
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
        private readonly IUserService<Student> _userService;

        public UserController(IUserService<Student> userService)
        {
            _userService = userService;
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
                ServiceResult result = await _userService.Register(user);
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