using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Registration.Data;
using Registration.Models;
using System.Security.Claims;
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
                return Ok(new { message = "Login successful!" });
            }

            return Unauthorized(new { error = "Invalid login credentials." });
        }
        [HttpGet("user/details")]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId = GetUserIdFromToken();

            if (userId == null)
            {
                return Unauthorized(new { error = "Invalid or missing token." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound(new { error = "User details not found." });
        }
        private int? GetUserIdFromToken()
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }

            return null;
        }


    }
}
