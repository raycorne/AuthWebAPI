using AuthWebAPI.Data;
using AuthWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

/*{
    "firstName": "Nenetrate",
  "lastName": "Piggers",
  "password": "Or@l_cumsh0t",
  "email": "niger@mail.ru",
  "address": "grave"
}*/

namespace AuthWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleDTO roleDTO)
        {
            var response = await _roleManager.CreateAsync(new IdentityRole
            {
                Name = roleDTO.RoleName
            });
            if(response.Succeeded)
            {
                return Ok("New Role Created");
            }
            else
            {
                return BadRequest(response.Errors);
            }
        }

        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserDTO assignRoleToUserDTO)
        {
            var userDetails = await _userManager.FindByEmailAsync(assignRoleToUserDTO.Email);

            if(userDetails != null)
            {
                var userRoleAssignResponse = await _userManager.AddToRoleAsync(userDetails, assignRoleToUserDTO.RoleName);
                if(userRoleAssignResponse.Succeeded)
                {
                    return Ok("Role Assigned to User: " + assignRoleToUserDTO.RoleName);
                }
                else
                {
                    return BadRequest(userRoleAssignResponse.Errors);
                }
            }
            else
            {
                return BadRequest("No user with this email");
            }
        }

        [AllowAnonymous]
        [HttpPost("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser(AuthenticateUserDTO authenticateUser)
        {
            var user = await _userManager.FindByNameAsync(authenticateUser.UserName);
            if (user == null)
                return Unauthorized();

            bool isValidUser = await _userManager.CheckPasswordAsync(user, authenticateUser.Password);
            if (isValidUser)
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var keyDetail = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _configuration["JWT:Issuer"],
                    Audience = _configuration["JWT:Audience"],
                    Expires = DateTime.UtcNow.AddMonths(1),
                    Subject = new ClaimsIdentity(claims),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(keyDetail), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(tokenHandler.WriteToken(token));
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterUserDTO registerUserDTO)
        {
            var userToBeCreated = new User
            {
                FirstName = registerUserDTO.FirstName,
                LastName = registerUserDTO.LastName,
                Email = registerUserDTO.Email,
                UserName = registerUserDTO.Email,
                Address = registerUserDTO.Address,
            };

            var response = await _userManager.CreateAsync(userToBeCreated, registerUserDTO.Password);

            if(response.Succeeded)
            {
                return Ok("User created");
            }
            else
            {
                return BadRequest(response.Errors);
            }
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(DeleteUserDTO deleteUserDTO)
        {

            var existingUser = await _userManager.FindByEmailAsync(deleteUserDTO.Email);
            if(existingUser != null)
            {
                var response = await _userManager.DeleteAsync(existingUser);
                if (response.Succeeded)
                {
                    return Ok("User Deleted");
                }
                else
                {
                    return BadRequest(response.Errors);
                }
            }
            else
            {
                return BadRequest("No User with this email");
            }
        }
    }
}
