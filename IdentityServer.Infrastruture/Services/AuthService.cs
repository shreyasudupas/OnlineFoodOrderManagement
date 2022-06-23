using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Request;
using IdenitityServer.Core.Domain.Response;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.Services
{
    public class AuthService: IAuthService
    {
        public AuthService()
        {

        }
        public LoginResponse Login(LoginRequest login)
        {
            return new LoginResponse();
        }
    }
}
