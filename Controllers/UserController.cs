using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Registration.Data;
using Registration.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Registration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Registration successful!" });
            }

            return BadRequest(new { error = "Invalid registration data." });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);

            if (user != null)
            {
                return Ok(new { message = "Login successful!", username = user.Username, email = user.Email });
            }

            return Unauthorized(new { error = "Invalid login credentials." });
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Perform any necessary logout actions
            // For example, clearing authentication cookies or tokens

            return Ok(new { message = "Logout successful" });
        }

    }
}