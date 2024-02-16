using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GenerateToken/{Role}")]
        public IActionResult GenerateToken([FromRoute] string Role)
        {

            // Generate JWT token
            var token = GenerateJwtToken( Role );

            // Return the generated token in the HTTP response
            return Ok(new { token });

        }

        private string GenerateJwtToken(string Role)
        {
            
            var authClaims = new List<Claim>
            {
               
                new Claim("Name", "USER"),
                new Claim("UserId", "1"),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

          
                authClaims.Add(new Claim(ClaimTypes.Role, Role));
                authClaims.Add(new Claim("Role", Role));  //For Front-end



            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
