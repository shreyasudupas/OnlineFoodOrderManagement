using Identity.MicroService.Data;
using Identity.MicroService.Data.DatabaseContext;
using Identity.MicroService.Features.UserFeature.Queries;
using Identity.MicroService.Models.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.MicroService.Features.UserFeature.Commands
{
    public class UserAuthenticationHandler : IRequestHandler<GetAuthorizationTokenForUser, AuthenticationResponse>
    {
        private readonly UserContext _dbContext;
        private IConfiguration _configuration; 

        public UserAuthenticationHandler(UserContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _configuration = config;
        }

        public async Task<AuthenticationResponse> Handle(GetAuthorizationTokenForUser request, CancellationToken cancellationToken)
        {
            //check if the user name is present in the database
            var UserExists = await _dbContext.Users.Include(x=>x.Role).Where(x => x.UserName.Contains(request.UserName)).FirstOrDefaultAsync();

            if(UserExists == null)
            {
                return new AuthenticationResponse();
            }

            var token = generateJwtToken(UserExists);
            return new AuthenticationResponse 
            { 
                Id = UserExists.Id,
                RoleName = UserExists.Role.Rolename,
                Username = UserExists.UserName,
                Nickname = UserExists.FullName,
                PictureLocation = UserExists.PictureLocation,
                Token = token
            };
        }

        private string generateJwtToken(User user)
        {
            var scope = "openid profile email profile:" + user.Role.Rolename;

            var key = _configuration["Jwt:Secret"]??"This is a test secret";
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("scope", scope),
                    new Claim("permissions", "profile:"+user.Role.Rolename)
                }),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration["Auth0:Audience"],
                Issuer = $"https://{_configuration["Auth0:Domain"]}/"
        };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
