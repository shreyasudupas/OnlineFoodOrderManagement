﻿using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Features.Login;
using IdenitityServer.Core.Features.Logout;
using IdenitityServer.Core.Features.Register;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginCommand login);
        Task<PreLogoutResponse> PreLogout(LogoutQuery logoutCommand);
        Task Register(RegisterCommand reqisterCommand);
        Task<RegisterAdminResponse> RegisterAdmin(RegisterAdminResponse registerAdminResponse);
    }
}
