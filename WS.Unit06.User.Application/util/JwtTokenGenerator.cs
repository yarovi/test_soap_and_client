using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WS.Unit06.User.Application.util
{
    public class JwtTokenGenerator
    {

        public string GenerateJwtToken(string username)
        {
            var secretKey = GenerateSecretKey();
            // Configurar las claims del token
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, username));
            claims.AddClaim(new Claim(ClaimTypes.GroupSid, "1"));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            string tokenCreado = tokenHandler.WriteToken(tokenConfig);
            return tokenCreado;
        }
        public ClaimsPrincipal ValidateJwtToken(string jwtToken )
        {
            var secretKey = GenerateSecretKey();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false, // Puedes ajustar esto según tus necesidades
                ValidateAudience = false, // Puedes ajustar esto según tus necesidades
                ClockSkew = TimeSpan.Zero // No se permite desfase en el tiempo
            };

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var validatedToken);
                return claimsPrincipal;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error al validar el token: " + ex.Message);
                return null;
            }
        }
        public string GenerateSecretKey()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
            string value = config["JwtSetting:key"];
            return value;
        }
    }
}
