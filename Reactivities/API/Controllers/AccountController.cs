using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;

namespace API.Controllers
{
    namespace API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AccountController : ControllerBase
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly TokenService _tokenService;
            private readonly DataContext _context;

            public AccountController(UserManager<AppUser> userManager, TokenService tokenService, DataContext context)
            {
                _tokenService = tokenService;
                _context = context;
                _userManager = userManager;
            }

            [AllowAnonymous]
            [HttpPost("login")]
            public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
            {
                var user = await _userManager.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Email == loginDto.Email);

                if (user == null) return Unauthorized();

                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (result)
                {
                    return CreateUserObject(user);
                }

                return Unauthorized();
            }

            [AllowAnonymous]
            [HttpPost("register")]
            public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
            {
                if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
                {
                    //return BadRequest("Username is already taken");
                    ModelState.AddModelError("username", "Username taken");
                    return ValidationProblem();
                }

                if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
                {
                    //return BadRequest("Email is already taken");
                    ModelState.AddModelError("email", "Email taken");
                    return ValidationProblem();
                }

                var user = new AppUser
                {
                    DisplayName = registerDto.DisplayName,
                    Email = registerDto.Email,
                    UserName = registerDto.Username
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    return CreateUserObject(user);
                }

                return BadRequest(result.Errors);
            }

            [Authorize]
            [HttpGet]
            public async Task<ActionResult<UserDto>> GetCurrentUser()
            {
                var user = await _userManager.Users.Include(u => u.Photos)
                    .FirstOrDefaultAsync(u => u.Email == (User.FindFirstValue(ClaimTypes.Email)));

                return CreateUserObject(user);
            }

            private UserDto CreateUserObject(AppUser user)
            {
                return new UserDto
                {
                    DisplayName = user.DisplayName,
                    Image = user.Photos?.FirstOrDefault(p => p.IsMain)?.Url,
                    Token = _tokenService.CreateToken(user),
                    Username = user.UserName
                };
            }
        }
    }
}
