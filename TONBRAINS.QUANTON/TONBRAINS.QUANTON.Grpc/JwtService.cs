using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TONBRAINS.QUANTON.Core.DAL;

namespace TONBRAINS.QUANTON.Grpc
{
    public class JwtService
    {
        private const string _secret = "asdv234234^&%&^%&^hjsdfb2%%%";
        public static string CreateToken(User userModel)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userModel.Id),
                    new Claim(ClaimTypes.GivenName, userModel.Name),
                    new Claim(ClaimTypes.Email, userModel.ExternalData),
                    new Claim(ClaimTypes.Name, userModel.SmartAccountId),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool ValidateToken(string token)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = mySecurityKey,
                    //RequireSignedTokens = true,
                    //RequireExpirationTime = true,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static User ParseToken(string token)
        {
            var tokenHandler = new JsonWebTokenHandler();

            var jsonWebToken = tokenHandler.ReadJsonWebToken(token);

            return new User
            {
                ExternalData = jsonWebToken.GetClaim("email").Value,
                Id = jsonWebToken.GetClaim("nameid").Value,
                Name = jsonWebToken.GetClaim("given_name").Value,
                SmartAccountId = jsonWebToken.GetClaim("unique_name").Value,
            };
            

        }
    }
}
