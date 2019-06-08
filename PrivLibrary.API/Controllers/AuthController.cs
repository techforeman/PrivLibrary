using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PrivLibrary.API.Data;
using PrivLibrary.API.DTOs;
using PrivLibrary.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace PrivLibrary.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDTO)
        {


            //validation will be added soon
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            userForRegisterDTO.Username = userForRegisterDTO.Username.ToLower();
            if (await _repo.UserExist(userForRegisterDTO.Username))
                return BadRequest("Username already exist");

            var userToCreate = new User
            {
                Username = userForRegisterDTO.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDTO.Password);

            return StatusCode(201);


        }

        [HttpPost ("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);


            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds

                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new {
                    token = tokenHandler.WriteToken(token)
                });
        }
    }

}