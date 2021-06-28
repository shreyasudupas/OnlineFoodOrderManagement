using MicroService.Shared.BuisnessLayer.IBuisnessLayer;
using MicroService.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.MicroService.Data.Enum;
using User.MicroService.Security.Requirments;

namespace User.MicroService.Security.Handlers
{
    public class CheckIfUserHandler:AuthorizationHandler<UserRequirement>
    {
        private readonly IProfileUser _profile;
        private readonly IUser _userBL;

        public CheckIfUserHandler(IProfileUser profile, IUser user)
        {
            _profile = profile;
            _userBL = user;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
        {
            if (context.Resource is Endpoint endpoint)
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
                            UserProfile user = new UserProfile();

                            var Item = _profile.GetUserDetails();
                            user.Username = Item.Item1;
                            user.PictureLocation = Item.Item2;
                            user.NickName = Item.Item3;
                            user.RoleId = (int)Enum.Parse(typeof(ProfileEnum), UserPermisson.ToLower());

                            var userPresent = _userBL.AddOrGetUserDetails(user);

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
            }
            return Task.CompletedTask;
        }
    }
}
