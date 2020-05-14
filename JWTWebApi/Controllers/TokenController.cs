using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTWebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class TokenController : Controller
    {
        private const string SECRET_KEY = "";

        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenController.SECRET_KEY));



        [HttpGet]
        [Route("api/Token/{username}/{password}")]
        public IActionResult Get(string username, string password)
        {
            /* Pass same value for username & password */
            if (username == password)
            {
                return new ObjectResult(GenerateToken(username));
            }
            else
            {
                return BadRequest();
            }
        }


        /* Generate token for user */
        private object GenerateToken(string username)
        {
            var token = new JwtSecurityToken(
                claims: new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                },
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                signingCredentials: new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}