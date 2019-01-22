using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrivLibrary.API.Data;
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
        public async Task<IActionResult> Register (string username, string password)
        {
            //validation will be added soon
            username = username.ToLower();
            if(await _repo.UserExist(username))
                return BadRequest("Username already exist");

            var userToCreate = new User
            {
                Username = username
            };

            var createdUser = await _repo.Register(userToCreate, password);

            return StatusCode(201);


        }

    }   
    
}