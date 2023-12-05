using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
        private readonly IUserService userService;
        private readonly IAuthorizationService authorizationService;

        public UserController(IUserService userService, IAuthorizationService authorizationService)
        {
            this.userService = userService;
            this.authorizationService = authorizationService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            if (user is null)
                return BadRequest();
            if (!ModelState.IsValid)
                BadRequest(ModelState);

            try
            {
                ServiceResult result = new ServiceResult();

                result = await userService.Login(user);

                if (!result.succeeded)
                    return Unauthorized(result);
                return Ok(result);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserCreationDto user)
        {
            if (user is null)
                return BadRequest();
            if (!ModelState.IsValid)
                BadRequest(ModelState);

            try
            {
                ServiceResult result = new ServiceResult();

                result = await userService.Register(user);

                if (!result.succeeded)
                    return BadRequest(result);
                result = await authorizationService.CreateTokenAsync(user.UserName);
                if (!result.succeeded)
                    return BadRequest(result);
                result = await authorizationService.CreateEmailTokenAsync(user.UserName);
                if (!result.succeeded)
                    return BadRequest(result);

                var callBackUrl = Request.Scheme + "://" + Request.Host + Url.Action("VerifyEmail", "User", new { email = user.Email, token = result.Details });
                result = await authorizationService.SendVerificationEmailAsync(user.Email, callBackUrl);
                if (!result.succeeded)
                    return BadRequest(new ServiceResult { Message = "Registration Failed", Details = "Something Happened, Please Try Again " });

                return Ok(result);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            if (email.IsNullOrEmpty() || token.IsNullOrEmpty())
            {
                return BadRequest("Invalid Payload");
            }
            var codeE = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var res = await authorizationService.VerifyEmailAsync(email, token);
            if (!res.succeeded)
                return BadRequest(res);
            return Ok();
        }
    }
}