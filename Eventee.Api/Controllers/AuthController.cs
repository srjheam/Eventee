using Eventee.Api.Controllers.Dtos;
using Eventee.Api.Data;
using Eventee.Api.Identity.Models;
using Eventee.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eventee.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        private readonly EventeeContext _eventeeContext;

        public AuthController(
            UserManager<ApplicationIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            EventeeContext eventeeContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _eventeeContext = eventeeContext;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
                return Unauthorized();

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var token = GetToken(authClaims);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return Conflict("User with the given email already exists.");

            var user = new ApplicationIdentityUser { UserName = model.Name, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);

            var createdUser = await _userManager.FindByNameAsync(model.Name);

            var eventeeUser = new User { Id = createdUser.Id, Name = createdUser.UserName, Email = createdUser.Email, HostedGetTogethers = { }, SubscribedGetTogethers = { } };

            _eventeeContext.Add(eventeeUser);
            await _eventeeContext.SaveChangesAsync();

            return CreatedAtAction(nameof(SignUp), new { id = createdUser.Id }, eventeeUser);
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
