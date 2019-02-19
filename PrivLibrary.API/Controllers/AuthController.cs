using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrivLibrary.API.Data;
using PrivLibrary.API.DTOs;
using PrivLibrary.API.Models;

namespace PrivLibrary.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register (UserForRegisterDto userForRegisterDTO)
        {
            //validation will be added soon
            userForRegisterDTO.Username = userForRegisterDTO.Username.ToLower();
            if(await _repo.UserExist(userForRegisterDTO.Username))
                return BadRequest("Username already exist");

            var userToCreate = new User
            {
                Username = userForRegisterDTO.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDTO.Password);

            return StatusCode(201);


        }

    }   
    
}