using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Quan.Word.Web.Server
{
    public class AuthorizeTokenAttribute : AuthorizeAttribute
    {
        public AuthorizeTokenAttribute()
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }


    /// <summary>
    /// Manages the Web API calls
    /// </summary>
    public class ApiController : Controller
    {
        [Route("api/login")]
        public IActionResult LogIn()
        {
            // TODO: Get users login information and check it is correct

            var username = "quanljh";
            var email = "quanljh@gmail.com";

            // Generate a token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim("my key","my value"),
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IoCContainer.Configuration["Jwt:SecretKey"])),
                SecurityAlgorithms.HmacSha256);

            // Generate the Jwt token
            var token = new JwtSecurityToken(
                IoCContainer.Configuration["Jwt:Issuer"],
                IoCContainer.Configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMonths(3),
                signingCredentials: credentials);

            // Return token to user
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [AuthorizeToken]
        [Route("api/private")]
        public IActionResult Private()
        {
            var user = HttpContext.User;

            return Ok(new { privateData = $"some secret for {user.Identity.Name}" });
        }
    }
}
