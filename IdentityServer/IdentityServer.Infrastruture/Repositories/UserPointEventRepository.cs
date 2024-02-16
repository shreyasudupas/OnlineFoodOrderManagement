using IdenitityServer.Core.Common.Exceptions;
using IdenitityServer.Core.Common.Interfaces.Repository;
using IdenitityServer.Core.Domain.DBModel;
using IdentityServer.Infrastruture.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.Repositories
{
    public class UserPointEventRepository : IUserPointEventRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<UserPointEventRepository> _logger;

        public UserPointEventRepository(ApplicationDbContext applicationDbContext,
            ILogger<UserPointEventRepository> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public async Task<List<UserPointsEvent>> GetAllUserEvents(string userId)
        {
            var events = await _applicationDbContext.UserPointsEvents.Where(x => x.UserId == userId)
                .ToListAsync();

            return events;
        }

        public async Task<UserPointsEvent> GetCurrentUserEvent(string userId)
        {
            var userEvent = await _applicationDbContext.UserPointsEvents.Where(x => x.UserId == userId)
                            .OrderByDescending(x => x.EventCreatedDate)
                            .FirstOrDefaultAsync();

            return userEvent;
        }

        public async Task SaveUserEvent(UserPointsEvent userPointsEvent)
        {
            try
            {
                await _applicationDbContext.UserPointsEvents.AddAsync(userPointsEvent);
                await _applicationDbContext.SaveChangesAsync();

                _logger.LogInformation("Event for userId:{0} is saved", userPointsEvent.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Event Saving Failed {0}",ex.Message);
                throw new ApplicationDbContextException(ex.Message, ex.InnerException);
            }
        }
    }
}
