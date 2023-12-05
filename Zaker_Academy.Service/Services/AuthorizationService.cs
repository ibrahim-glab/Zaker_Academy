using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.ErrorHandling;
using Zaker_Academy.Service.Helper;
using Zaker_Academy.Service.Interfaces;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Zaker_Academy.Service.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<applicationUser> userManager;
        private readonly IEmailService emailService;
        private readonly JwtHelper jwt;

        public AuthorizationService(UserManager<applicationUser> userManager, IOptions<JwtHelper> jwt, IEmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.jwt = jwt.Value;
        }

        public async Task<ServiceResult> CreateEmailTokenAsync(string username)
        {
            var res = new ServiceResult();
            var user = await userManager.FindByNameAsync(username);
            if (user is null)
            {
                res.Message = "Registration Field";
                res.Details = "internal Server Error";
                return res;
            }
            var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            res.succeeded = true;
            res.Details = emailToken;
            return res;
        }

        public async Task<ServiceResult> CreateTokenAsync(string username)
        {
            var res = new ServiceResult();
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                res.Message = "Registration Field";
                res.Details = "internal Server Error";
                return res;
            }
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
            var symKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signingCredentials = new SigningCredentials(symKey, SecurityAlgorithms.HmacSha256Signature);
            var SecurityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(jwt.DurationInHours),
                signingCredentials: signingCredentials
                );
            res.succeeded = true;
            res.Details = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            return res;
        }

        public async Task<ServiceResult> SendVerificationEmailAsync(string UserEmail, string callBackUrl)
        {
            var res = new ServiceResult();
            var body = $"Please Confirm your email address <a href = \" {System.Web.HttpUtility.HtmlEncode(callBackUrl)} \">Click Here </a>";
            res = await emailService.sendAsync("Confirmation Email", body, "glabhamada@gmail.com", UserEmail);
            if (res.succeeded)
                return res;
            return res;
        }

        public async Task<ServiceResult> VerifyEmailAsync(string Email, string code)
        {
            var result = new ServiceResult();
            var user = await userManager.FindByEmailAsync(Email);
            if (user is null)
            {
                result.Message = "Verification Field";
                result.Details = "Invalid Email";
                return result;
            }
            var res = await userManager.ConfirmEmailAsync(user, code);
            if (!res.Succeeded)
            {
                result.Message = "Verification Field";
                result.Details = "Your Email Is Not confirmed , Please try again later and check your email";
                return result;
            }
            result.succeeded = true;
            return result;
        }
    }
}