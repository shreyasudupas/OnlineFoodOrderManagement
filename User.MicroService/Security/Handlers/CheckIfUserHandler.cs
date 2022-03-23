using Common.Utility.BuisnessLayer.IBuisnessLayer;
using Common.Utility.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.MicroService.Data.Enum;
using Identity.MicroService.Security.Requirments;
using MediatR;
using Identity.MicroService.Features.UserFeature.Queries;
using Microsoft.Extensions.Logging;

namespace Identity.MicroService.Security.Handlers
{
    public class CheckIfUserHandler:AuthorizationHandler<UserRequirement>
    {
        private readonly IProfileUser _profile;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public CheckIfUserHandler(IProfileUser profile, IMediator mediator, ILogger<CheckIfUserHandler> logger)
        {
            _profile = profile;
            _mediator = mediator;
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var PermissionList = context.User.Claims.FirstOrDefault(c => c.Type == "permissions").Value;
                var permisionSplit = PermissionList.Split(':');
                var UserPermisson = permisionSplit[1];
                if (UserPermisson.ToLower() == requirement.User.ToLower())
                {
                    //Userprofile value is got from Middleware where the header value is stored
                    if (!string.IsNullOrEmpty(_profile.GetUserDetails().Item1))
                    {
                        GetUsernameRoleRequest request = new GetUsernameRoleRequest();

                        var Item = _profile.GetUserDetails();
                        request.UserName = Item.Item1;
                        request.RoleId = (int)Enum.Parse(typeof(ProfileEnum), UserPermisson.ToLower());

                        //var userPresent = _userBL.AddOrGetUserDetails(user);
                        var userPresent = _mediator.Send(request).GetAwaiter().GetResult();

                        //Debug.WriteLine(getEmailId);
                        if (userPresent == true)
                        {
                            //succeded the request
                            context.Succeed(requirement);
                            _logger.LogInformation("Identity {0} success",_profile.GetUserDetails().Item1);
                        }
                        else
                        {
                            _logger.LogInformation("user {0} not present", request.UserName);
                        }
                            
                    }
                    else
                    {
                        _logger.LogInformation("Header value not present");
                    }
                }
            }
            else
            {
                _logger.LogInformation("Token Expired");
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
