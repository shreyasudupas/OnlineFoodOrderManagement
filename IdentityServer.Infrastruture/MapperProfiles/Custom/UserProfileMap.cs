using IdenitityServer.Core.Domain.DBModel;
using IdentityServer.Infrastruture.Database;
using System.Collections.Generic;
using System.Linq;

namespace IdenitityServer.Core.MapperProfiles.Custom
{
    public static class UserProfileMap
    {
        public static UserProfile MapToProfile(this ApplicationUser applicationUser, ApplicationDbContext context)
        {
            var result = new UserProfile
            {
                Id = applicationUser.Id,
                LockoutEnabled = applicationUser.LockoutEnabled,
                TwoFactorEnabled = applicationUser.TwoFactorEnabled,
                PhoneNumber = applicationUser.PhoneNumber,
                PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed,
                ConcurrencyStamp = applicationUser.ConcurrencyStamp,
                SecurityStamp = applicationUser.SecurityStamp,
                PasswordHash = applicationUser.PasswordHash,
                EmailConfirmed = applicationUser.EmailConfirmed,
                NormalizedEmail = applicationUser.NormalizedEmail,
                Email = applicationUser.Email,
                NormalizedUserName = applicationUser.NormalizedUserName,
                UserName = applicationUser.UserName,
                LockoutEnd = applicationUser.LockoutEnd,
                AccessFailedCount = applicationUser.AccessFailedCount,
                Address = GetUserAddressInfo(applicationUser, context),
                IsAdmin = applicationUser.IsAdmin,
                ImagePath = applicationUser.ImagePath,
                CartAmount = applicationUser.CartAmount,
                Points = applicationUser.Points
            };
            return result;
        }

        private static List<UserProfileAddress> GetUserAddressInfo(ApplicationUser applicationUser, ApplicationDbContext context)
        {
            var profileAdderess = new List<UserProfileAddress>();
            foreach (var address in applicationUser.Address)
            {
                profileAdderess.Add(new UserProfileAddress
                {
                    Id = address.Id,
                    Area =address.Area,
                    City = address.City,
                    FullAddress = address.FullAddress,
                    IsActive = address.IsActive,
                    State = address.State,
                    MyStates = context.States.Select(state=> new Domain.Model.DropdownModel
                    {
                        Label = state.Name,
                        Value = state.Id
                    }).ToList()
                });
            }

            return profileAdderess;
        }
    }
}
