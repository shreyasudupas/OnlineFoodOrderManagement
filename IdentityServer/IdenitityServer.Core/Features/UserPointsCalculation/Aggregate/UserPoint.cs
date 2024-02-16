using IdenitityServer.Core.Common.Exceptions;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Common.Interfaces.Repository;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Features.UserPointsCalculation.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.UserPointsCalculation.Aggregate
{
    public class UserPoint
    {
        private readonly IUserPointEventRepository _userPointEventRepository;
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private const string EventAddOperation = "ADD_POINTS";
        private const string EventSpentOperation = "POINTS_SPENT";
        private const string EventAdjustedOperation = "POINTS_ADJUSTED";
        private const string SystemGenerated = "SYSTEM_GENERATED";
        private const string EventAddedResponse = "USER_EVENT_ADDED";
        private const string EventAddFailureResponse = "USER_EVENT_ADD_FAILURE";
        private const string EventSpentResponse = "USER_EVENT_SPENT";
        private const string EventSpentFailureResponse = "USER_EVENT_SPENT_FAILURE";
        private const string EventAdjustedtResponse = "USER_EVENT_ADJUSTED";
        private const string EventAdjustedFailureResponse = "USER_EVENT_SPENT_FAILURE";

        public UserPoint(IUserPointEventRepository userPointEventRepository,
            IUserService userService,
            ILogger<UserProfile> logger)
        {
            _userPointEventRepository = userPointEventRepository;
            _userService = userService;
            _logger = logger;
        }

        public async Task<UserPointResponse> AddPoints(string userId,double points)
        {
            UserPointResponse response = new();
            try
            {
                await ApplyEvent(new UserPointsAdded(
                    userId,
                    points,
                    DateTime.Now
                ));

                response.Result = EventAddedResponse;
            } 
            catch(UserEventException userException)
            {
                response.Result = EventAddFailureResponse;
            }
            catch(ApplicationDbContextException appContextException)
            {
                response.Result = EventAddFailureResponse;
            }
            return response;
        }

        public async Task<UserPointResponse> RemovePoints(string userId, double points)
        {
            UserPointResponse response = new();
            try
            {
                await ApplyEvent(new UserPointsSpent(
                    userId,
                    points,
                    DateTime.Now
                ));

                response.Result = EventSpentResponse;
            } 
            catch(UserEventException userException)
            {
                response.Result = EventSpentFailureResponse;
            }
            catch (ApplicationDbContextException appContextException)
            {
                response.Result = EventSpentFailureResponse;
            }
            return response;
        }

        public async Task<UserPointResponse> AdjustPoints(string userId, double points,string adminId)
        {
            UserPointResponse response = new();
            try
            {
                await ApplyEvent(new UserPointsAdjusted(
                    userId,
                    points,
                    DateTime.Now,
                    adminId
                ));
                response.Result = EventAdjustedtResponse;
            }
            catch(UserEventException userException)
            {
                response.Result = EventAdjustedFailureResponse;
            }
            catch (ApplicationDbContextException appContextException)
            {
                response.Result = EventAdjustedFailureResponse;
            }
            return response;
        }

        public async Task ApplyEvent(IUserPointsEvent evnt)
        {
            switch(evnt)
            {
                case UserPointsAdded userPointsAdded:
                     await Apply(userPointsAdded);
                     break;
                case UserPointsSpent userPointsSpent:
                    await Apply(userPointsSpent);
                    break;
                case UserPointsAdjusted userPointsAdjusted:
                    await Apply(userPointsAdjusted);
                    break;
                default:
                    throw new Exception("No Event found Matching found");
            }
        }

        /// <summary>
        /// Functionalty for User Points Add Event
        /// </summary>
        /// <param name="userPointsAddedEvent"></param>
        private async Task Apply(UserPointsAdded userPointsAddedEvent)
        {
            UserPointsEvent userPointsEventDbModel = new();
            userPointsEventDbModel.UserId = userPointsAddedEvent.userId;
            userPointsEventDbModel.EventCreatedDate = userPointsAddedEvent.createdDate;
            userPointsEventDbModel.AddOrAdjustedUserId = SystemGenerated;
            userPointsEventDbModel.EventOperation = EventAddOperation;

            var user = await _userService.GetUserInformationById(userPointsAddedEvent.userId);
            if(user is null)
            {
                string error = "User not present in the database";
                _logger.LogError(error);
                throw new UserEventException(error);
            }

            var existedEvent = await _userPointEventRepository.GetCurrentUserEvent(userPointsAddedEvent.userId);
            if(existedEvent is not null)
            {
                userPointsEventDbModel.PointsInHand = existedEvent.PointsInHand + userPointsAddedEvent.points; //points adding to existing points
            } 
            else
            {
                //First time user event
                userPointsEventDbModel.PointsInHand = userPointsAddedEvent.points;
            }

            userPointsEventDbModel.PointsAdjusted = userPointsAddedEvent.points; //Points added number

            await _userPointEventRepository.SaveUserEvent(userPointsEventDbModel);
        }

        /// <summary>
        /// Functionalty for User Points Spent Event
        /// </summary>
        /// <param name="userPointsSpentEvent"></param>
        private async Task Apply(UserPointsSpent userPointsSpentEvent)
        {
            UserPointsEvent userPointsEventDbModel = new UserPointsEvent
            {
                UserId = userPointsSpentEvent.userId,
                EventId = 0,
                EventCreatedDate = DateTime.Now,
                AddOrAdjustedUserId = string.Empty,
                EventOperation = EventSpentOperation,
            };
            var existedEvent = await _userPointEventRepository.GetCurrentUserEvent(userPointsSpentEvent.userId);
            if(existedEvent is not null)
            {
                userPointsEventDbModel.PointsInHand = existedEvent.PointsInHand - userPointsSpentEvent.points;
            }
            else
            {
                var error = "User with Id: {0} has no previous Events to remove points";
                _logger.LogError(error, userPointsSpentEvent.userId);
                throw new UserEventException(new System.Text.StringBuilder(error).Replace("{0}", userPointsSpentEvent.userId).ToString());
            }

            await _userPointEventRepository.SaveUserEvent(userPointsEventDbModel);
        }

        /// <summary>
        /// Functionalty for User Points Adjusted Event, its only add operation you cannot remove the user points
        /// </summary>
        /// <param name="userPointsAdjustedEvent"></param>
        private async Task Apply(UserPointsAdjusted userPointsAdjustedEvent)
        {
            UserPointsEvent userPointsEventDbModel = new UserPointsEvent
            {
                UserId = userPointsAdjustedEvent.userId,
                EventId = 0,
                EventCreatedDate = DateTime.Now,
                AddOrAdjustedUserId = userPointsAdjustedEvent.adminId,
                EventOperation = EventAdjustedOperation,
            };
            var existedEvent = await _userPointEventRepository.GetCurrentUserEvent(userPointsAdjustedEvent.userId);
            if (existedEvent is not null)
            {
                //adjusting is just a add operation but add by admin and not by userId
                existedEvent.PointsInHand = existedEvent.PointsInHand + userPointsAdjustedEvent.points;
                existedEvent.PointsAdjusted = userPointsAdjustedEvent.points;
            }
            else
            {
                var error = "User with Id: {0} has no previous Events to adjust points";
                _logger.LogError(error, userPointsAdjustedEvent.userId);
                throw new UserEventException(new System.Text.StringBuilder(error).Replace("{0}", userPointsAdjustedEvent.userId).ToString());
            }

            await _userPointEventRepository.SaveUserEvent(userPointsEventDbModel);
        }
    }
}
