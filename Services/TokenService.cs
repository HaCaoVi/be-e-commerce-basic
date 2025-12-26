using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using e_commerce_basic.Dtos.Token;
using e_commerce_basic.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace e_commerce_basic.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            var signingKey = _config["JWT:SigningKey"] ?? throw new InvalidOperationException("JWT:SigningKey is missing");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }

        public string CreateAccessToken(NewTokenDto newTokenDto)
        {
            var email = newTokenDto.Email ?? throw new InvalidOperationException("Email is missing");
            var Id = newTokenDto.Id;
            var roleName = newTokenDto.RoleName ?? throw new InvalidOperationException("Role name is missing");
            var username = newTokenDto.Username ?? throw new InvalidOperationException("Username is missing");

            int accessTokenExpire = int.Parse(_config["JWT:AccessTokenExpire"] ?? throw new InvalidOperationException("JWT:AccessTokenExpire is missing"));

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier,Id.ToString()),
                new (ClaimTypes.Email,email),
                new (ClaimTypes.GivenName,username),
                new (ClaimTypes.Role, roleName)
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(accessTokenExpire),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string CreateRefreshToken(NewTokenDto newTokenDto)
        {
            var email = newTokenDto.Email ?? throw new InvalidOperationException("Email is missing");
            var Id = newTokenDto.Id;
            var roleName = newTokenDto.RoleName ?? throw new InvalidOperationException("Role name is missing");
            var username = newTokenDto.Username ?? throw new InvalidOperationException("Username is missing");
            int refreshTokenExpire = int.Parse(_config["JWT:RefreshTokenExpire"] ?? throw new InvalidOperationException("JWT:RefreshTokenExpire is missing"));

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier,Id.ToString()),
                new (ClaimTypes.Email,email),
                new (ClaimTypes.GivenName,username),
                new (ClaimTypes.Role, roleName)
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(refreshTokenExpire),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}