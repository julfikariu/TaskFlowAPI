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

            return await GenerateTokenAsync(user);
        }

        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            // ১. Configuration রিড করার সবচেয়ে নিরাপদ উপায়
            var keyString = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expirationMinutes = Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"]);

            var roles = await _userManager.GetRolesAsync(user);

            // ২. Claims লিস্ট তৈরি
            var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, user.Id), // Sub এর বদলে NameIdentifier ব্যবহার করা ASP.NET Core এর জন্য স্ট্যান্ডার্ড
        new(ClaimTypes.Email, user.Email ?? ""),
        new(ClaimTypes.Name, user.FullName ?? "")
    };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // ৩. Key এবং Credentials তৈরি
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // ৪. Token Descriptor ব্যবহার করা (Modern & Recommended Approach)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            // Claim Names যেন বদলে না যায় তার জন্য এটি বন্ধ রাখা ভালো
            tokenHandler.InboundClaimTypeMap.Clear();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
