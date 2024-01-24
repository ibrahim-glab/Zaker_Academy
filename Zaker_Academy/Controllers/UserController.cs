using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.ErrorHandling;
using Zaker_Academy.Service.Interfaces;
using IAuthorizationService = Zaker_Academy.Service.Interfaces.IAuthorizationService;

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
            if (!(String.Equals(user.Role, "instructor", StringComparison.OrdinalIgnoreCase) || String.Equals(user.Role, "student", StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("", "Invalid Role");
                return BadRequest(ModelState);
            }
            try
            {
                ServiceResult result = new ServiceResult();
                var callBackUrl = Request.Scheme + "://" + Request.Host + Url.Action("VerifyEmail", "User", new { email = user.Email, token = result.Details });
                result = await userService.Register(user, callBackUrl);
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

        [HttpGet("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {


            if (email.IsNullOrEmpty() || token.IsNullOrEmpty())
            {
                return BadRequest("Invalid Payload");
            }
            string decodedCode = Uri.UnescapeDataString(token);
            var res = await authorizationService.VerifyEmailAsync(email, decodedCode);
            if (!res.succeeded)
                return BadRequest(res);
            return Ok(new ServiceResult { succeeded = true, Message = "Verification Succeeded , Now You can Login " });
        }
        [Authorize]
        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (email.IsNullOrEmpty())
            {
                return BadRequest("Invalid Payload");
            }
            var res = await authorizationService.CreatePasswordTokenAsync(email);
            if (!res.succeeded)
                return NotFound(res);
            res = await authorizationService.SendResetPasswordAsync(email, res.Details?.ToString()!);
            if (!res.succeeded)
                return BadRequest(res);
            return Ok(res);
        }
        [Authorize]
        [HttpPost("ConfirmResetPassword")]
        public async Task<IActionResult> ConfirmResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Payload ");
            }
            var res = await authorizationService.ConfirmResetPasswordAsync(resetPasswordDto);
            if (!res.succeeded)
                return BadRequest(res);
            return Ok(res);
        }
    }
}