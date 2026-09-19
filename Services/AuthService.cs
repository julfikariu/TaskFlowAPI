using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskFlowAPI.Configuration;
using TaskFlowAPI.DTOs;
using TaskFlowAPI.Model;

namespace TaskFlowAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);


            if (user == null)
            {
                return null;
            }

            var passwordValid = await _userManager.CheckPasswordAsync(
                user,
                dto.Password
            );

            if (!passwordValid)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            return await GenerateTokenAsync(user);
        }

        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            // 1. Read Configuration 
            var keyString = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expirationMinutes = Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"]);

            var roles = await _userManager.GetRolesAsync(user);

            // 2. Make Claims list
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id), 
                new(ClaimTypes.Email, user.Email ?? ""),
                new(ClaimTypes.Name, user.FullName ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 4. Generate Key and Credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // ৪. Use Token Descriptor (Modern & Recommended Approach)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            // Close Claim Names, since its not altered
            tokenHandler.InboundClaimTypeMap.Clear();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        } 

    }
}
