using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Controllers.User
{
    [Authorize]
    public class HomeDashboardController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _client;
        private readonly IResourceStore _resources;

        public HomeDashboardController(IIdentityServerInteractionService interaction, IClientStore client, IResourceStore resources)
        {
            _interaction = interaction;
            _client = client;
            _resources = resources;
        }

        public async Task<IActionResult> Index()
        {
            var sub = User.GetSubjectId();
           
            

            return View();
        }
    }
}
