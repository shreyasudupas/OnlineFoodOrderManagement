using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdentityServer.Infrastruture.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.Services
{
    public class UserServices : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UserServices(
            UserManager<ApplicationUser> userManager,
            ILogger<UserServices> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserProfile> GetUserInformationById(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if(user != null)
            {
                var modelMap = _mapper.Map<UserProfile>(user);
                return modelMap;
            }
            else
            {
                _logger.LogInformation($"{UserId} is not present in the database");
                return null;
            }
        }
    }
}
