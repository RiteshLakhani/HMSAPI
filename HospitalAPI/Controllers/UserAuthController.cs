using HospitalAPI.Data;
using HospitalAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UserAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly string? _jwtKey;
        private readonly string? _JwtIssuer;
        private readonly string? _jwtAudience;
        private readonly int _JwtExpiry;

        public UserAuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signinManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _jwtKey = configuration["Jwt:Key"];
            _JwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
            _JwtExpiry = int.Parse(configuration["Jwt:ExpiryMinutes"]);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            try
            {
                if (registerModel == null || string.IsNullOrEmpty(registerModel.Name)
                    || string.IsNullOrEmpty(registerModel.Email) || string.IsNullOrEmpty(registerModel.Password))
                {
                    return BadRequest(new { message = "Invalid Registration Details" });
                }

                var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
                if (existingUser != null)
                {
                    return Conflict(new { message = "Email Already Exist" });
                }

                var user = new ApplicationUser
                {
                    UserName = registerModel.Email,
                    Email = registerModel.Email,
                    Name = registerModel.Name
                };

                var result = await _userManager.CreateAsync(user, registerModel.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "User Creation Failed", errors = result.Errors });
                }

                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user == null)
            {
                return Unauthorized(new { success = false, message = "Invalid Email or Password" });
            }

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { success = false, message = "Invalid Email or Password" });
            }

            var token = await GenerateJWTToken(user);

            return Ok(new
            {
                success = true,
                message = "Login Successful",
                token = token
            });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return Ok("User Logged out Successfully");
        }

        private async Task<string> GenerateJWTToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var Claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub , user.Id),
        new Claim(JwtRegisteredClaimNames.Email , user.Email),
        new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
        new Claim("Name", user.Name),
        new Claim("Role", userRoles.FirstOrDefault() ?? "User")
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _JwtIssuer,
                audience: _jwtAudience,
                claims: Claims,
                expires: DateTime.Now.AddMinutes(_JwtExpiry),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
