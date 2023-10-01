using AuthWebAPI.Core.Data;
using AuthWebAPI.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MobileAppWebAPI.Controllers
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
            if (response.Succeeded)
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

            if (userDetails != null)
            {
                var userRoleAssignResponse = await _userManager.AddToRoleAsync(userDetails, assignRoleToUserDTO.RoleName);
                if (userRoleAssignResponse.Succeeded)
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
        public async Task<IActionResult> AuthenticateUser(AuthenticateUser authenticateUser)
        {
            var user = await _userManager.FindByNameAsync(authenticateUser.UserName);
            if (user == null) return Unauthorized();

            bool isValidUser = await _userManager.CheckPasswordAsync(user, authenticateUser.Password);

            if (isValidUser)
            {
                string accessToken = GenerateAccessToken(user);
                var refreshToken = GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                await _userManager.UpdateAsync(user);

                var response = new AuthMainResponse
                {
                    Content = new AuthenticationResponse
                    {
                        RefreshToken = refreshToken,
                        AccessToken = accessToken
                    },
                    IsSuccess = true,
                    ErrorMessage = ""
                };
                return Ok(response);
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

            if (response.Succeeded)
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
            if (existingUser != null)
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

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var response = new AuthMainResponse();
            if (refreshTokenRequest is null)
            {
                response.ErrorMessage = "Invalid request";
                response.IsSuccess = false;
                return BadRequest(response);
            }

            var principal = GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken);

            if (principal != null)
            {
                var email = principal.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(email?.Value);
                if (user is null || user.RefreshToken != refreshTokenRequest.RefreshToken)
                {
                    response.ErrorMessage = "Invalid Request";
                    response.IsSuccess = false;
                }

                string newAccessToken = GenerateAccessToken(user);
                string refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                await _userManager.UpdateAsync(user);

                response.IsSuccess = true;
                response.Content = new AuthenticationResponse
                {
                    RefreshToken = refreshToken,
                    AccessToken = newAccessToken
                };
                return Ok(response);
            }
            else
            {
                return ErrorResponse.ReturnErrorResponse("Invalid Token Found");
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var keyDetail = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenValidatorParameter = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = _configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(keyDetail)
            };

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidatorParameter, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid Token");

            return principal;
        }

        private string GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var keyDetail = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} { user.LastName}"),
                    new Claim(ClaimTypes.Email, user.Email),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration["JWT:Audience"],
                Issuer = _configuration["JWT:Issuer"],
                Expires = DateTime.UtcNow.AddMinutes(1),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyDetail), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
