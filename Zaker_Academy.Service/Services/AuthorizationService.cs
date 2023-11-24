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

        private readonly JwtHelper jwt;

        public AuthorizationService(UserManager<applicationUser> userManager, IOptions<JwtHelper> jwt)
        {
            this.userManager = userManager;
            this.jwt = jwt.Value;
        }

        public async Task<ServiceResult> CreateToken(string userName)
        {
            var res = new ServiceResult();
            var user = await userManager.FindByNameAsync(userName);
            if (user is null)
            {
                res.Message = "Authentication Failed";
                res.Details = "User doesn't exist";
                return res;
            }
            var roles = await userManager.GetRolesAsync(user);
            var Cliams = new List<Claim>();
            foreach (var role in roles)
                Cliams.Add(new Claim("role", role));
            Cliams.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName!));
            Cliams.Add(new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()));
            Cliams.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
            var symKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var symAlgosign = new SigningCredentials(symKey, SecurityAlgorithms.HmacSha256);
            var SecurityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: Cliams,
                expires: DateTime.UtcNow.AddHours(jwt.DurationInHours),
                signingCredentials: symAlgosign
                );

            res.succeeded = true;
            res.Message = "Authentication succeeded";
            res.Details = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            return res;
        }
    }
}