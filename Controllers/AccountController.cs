using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContentGenerator.Models.Authentication;
using ContentGenerator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ContentGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        public AccountController(IUserService userService, ILogger<AccountController> logger, IConfiguration configuration)
        {
            _userService =  userService;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddOrUpdateAppUserModel model)
        {
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                var existedUser = await _userService.IsUserNameFree(model.UserName);
                if (!existedUser)
                {
                    ModelState.AddModelError("", "User name is already taken");
                    return BadRequest(ModelState);
                }
                // Create a new user object
                var user = new AppUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserRole = UserRoles.User
                };
                // Try to save the user
                await _userService.CreateUser(user, model.Password);
                // If the user is successfully created, return Ok
                var token = GenerateToken(model.UserName, UserRoles.User);
                return Ok(new { token });
            }
            // If we got this far, something failed, redisplay form
            return BadRequest(ModelState);
        }

        // Create a Login action to validate the user credentials and generate the JWT token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Get the secret in the configuration

            // Check if the model is valid
            if (ModelState.IsValid)
            {
                if (await _userService.CheckPassword(model.UserName, model.Password))
                {
                    var token = GenerateToken(model.UserName, UserRoles.Administrator);
                    return Ok(new { token });
                }
                // If the user is not found, display an error message
                ModelState.AddModelError("", "Invalid username or password");
            }
            return BadRequest(ModelState);
        }

        private string? GenerateToken(string userName, string userRole)
        {
            var secret = _configuration["JwtConfig:Secret"];
            var issuer = _configuration["JwtConfig:ValidIssuer"];
            var audience = _configuration["JwtConfig:ValidAudiences"];
            if (secret is null || issuer is null || audience is null)
            {
                throw new ApplicationException("Jwt is not set in the configuration");
            }
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>{
                new (ClaimTypes.Name, userName),
                new (ClaimTypes.Role, userRole),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            //var jwtToken = new JwtSecurityToken(
            //    issuer: issuer,
            //    audience: audience,
            //    claims: new[]{
            //        new Claim(ClaimTypes.Name, userName)
            //    },
            //    expires: DateTime.UtcNow.AddDays(1),
            //    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            //);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
