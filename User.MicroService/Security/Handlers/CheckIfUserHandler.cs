using MicroService.Shared.BuisnessLayer.IBuisnessLayer;
using MicroService.Shared.Models;
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

namespace Identity.MicroService.Security.Handlers
{
    public class CheckIfUserHandler:AuthorizationHandler<UserRequirement>
    {
        private readonly IProfileUser _profile;
        private readonly IMediator _mediator;

        public CheckIfUserHandler(IProfileUser profile,IMediator mediator)
        {
            _profile = profile;
            _mediator = mediator;
            //_userBL = user;
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
                        //UserProfile user = new UserProfile();
                        AddUserRequestModel request = new AddUserRequestModel();

                        var Item = _profile.GetUserDetails();
                        request.Username = Item.Item1;
                        request.PictureLocation = Item.Item2;
                        request.NickName = Item.Item3;
                        request.RoleId = (int)Enum.Parse(typeof(ProfileEnum), UserPermisson.ToLower());

                        //var userPresent = _userBL.AddOrGetUserDetails(user);
                        var userPresent = _mediator.Send(request).GetAwaiter().GetResult();

                        //Debug.WriteLine(getEmailId);
                        if (userPresent != null)
                            //succeded the request
                            context.Succeed(requirement);
                    }
                }
            }
            else
            {
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
