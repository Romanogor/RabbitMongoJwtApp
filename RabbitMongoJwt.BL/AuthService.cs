using Interfaces;
using Microsoft.IdentityModel.Tokens;
using RabbitMongoJwt.BL.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RabbitMongoJwt.BL
{
    public class AuthService : IAuthService
    {
        private IAppSettings _appSettings;
        public AuthService(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_appSettings.Issuer,
              _appSettings.Issuer,
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string AuthenticateUser(LoginDto loginInfo)
        {
            string token = string.Empty;

            if (loginInfo.UserName == _appSettings.UserName && loginInfo.Password == _appSettings.Password)
            {
                token = GenerateJSONWebToken();
            }
            return token;
        }
    }
}
