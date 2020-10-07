using Microsoft.IdentityModel.Tokens;
using monitor_core.Settings;
using monitor_infra.Entities;
using monitor_infra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace monitor_infra.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly TokenSettings _tokenSettings;

        public JwtTokenService(TokenSettings tokenSettings)
        {
            _tokenSettings = tokenSettings;
        }

        public string GetToken(User user)
        {
            var token = new JwtSecurityToken(
                                issuer: _tokenSettings.Issuer,
                                audience: _tokenSettings.Audience,
                                expires: DateTime.UtcNow.AddMinutes(_tokenSettings.ExpirationInMinutes),
                                signingCredentials: new SigningCredentials(
                                    new SymmetricSecurityKey(_tokenSettings.GetSecurityKey()), SecurityAlgorithms.HmacSha512Signature),
                                claims: new List<Claim>
                                {
                                        new Claim("userId", user.Id.ToString()),
                                        //new Claim("companyId", user.CompanyId.ToString()),
                                });
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public string RenewToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var oldSecurityToken = tokenHandler.ReadJwtToken(token);

            var newSecurityToken = new JwtSecurityToken(
                 expires: DateTime.UtcNow.AddMinutes(_tokenSettings.ExpirationInMinutes),
                 signingCredentials: new SigningCredentials(
                                         new SymmetricSecurityKey(_tokenSettings.GetSecurityKey()), SecurityAlgorithms.HmacSha512Signature),
                 claims: oldSecurityToken.Claims);

            return tokenHandler.WriteToken(newSecurityToken);
        }

        private IEnumerable<Claim> decodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadJwtToken(token);
            return decodedToken.Claims;
        }

        public int GetUserIdFromToken(string token)
        {
            var claims = decodeToken(token);
            return int.Parse(claims.First(claim => claim.Type == "userId").Value);
        }

        //public int GetUserCompanyIdFromToken(string token)
        //{
        //    var claims = decodeToken(token);
        //    return int.Parse(claims.First(claim => claim.Type == "companyId").Value);
        //}
    }
}
