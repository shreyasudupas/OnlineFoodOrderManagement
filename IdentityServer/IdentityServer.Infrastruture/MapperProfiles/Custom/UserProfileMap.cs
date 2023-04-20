using IdenitityServer.Core.Domain.DBModel;
using IdentityServer.Infrastruture.Database;
using System;
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
                //IsAdmin = applicationUser.IsAdmin,
                ImagePath = applicationUser.ImagePath,
                CartAmount = applicationUser.CartAmount,
                Points = applicationUser.Points,
                CreatedDate = applicationUser.CreatedDate,
                Enabled = applicationUser.Enabled,
                UserType = applicationUser.UserType
            };
            return result;
        }

        private static List<UserProfileAddress> GetUserAddressInfo(ApplicationUser applicationUser, ApplicationDbContext context)
        {
            var profileAdderess = new List<UserProfileAddress>();
            var addressList = applicationUser.Address;

            if(addressList != null)
            {
                foreach (var address in addressList)
                {
                    var stateId = context.States.Where(x => x.Name == address.State).Select(s => s.Id.ToString()).FirstOrDefault();

                    var cities = context.Cities.Where(x => x.StateId == Convert.ToInt32(stateId))
                    .Select(city => new Domain.Model.DropdownModel
                    {
                        Label = city.Name,
                        Value = city.Id.ToString()
                    }).ToList();

                    var cityId = context.Cities.Where(x => x.Name == address.City && x.StateId == Convert.ToInt32(stateId))
                    .Select(x => x.Id.ToString()).FirstOrDefault();

                    var areaId = context.LocationAreas.Where(x => x.CityId == Convert.ToInt32(cityId) && x.AreaName == address.Area).Select(x => x.Id.ToString()).FirstOrDefault();
                    var areas = context.LocationAreas.Where(x => x.CityId == Convert.ToInt32(cityId))
                    .Select(area => new Domain.Model.DropdownModel
                    {
                        Label = area.AreaName,
                        Value = area.Id.ToString()
                    }).ToList();

                    profileAdderess.Add(new UserProfileAddress
                    {
                        Id = address.Id,
                        Area = address.Area,
                        City = address.City,
                        FullAddress = address.FullAddress,
                        IsActive = address.IsActive,
                        State = address.State,
                        StateId = stateId,
                        CityId = cityId,
                        MyStates = context.States.Select(state => new Domain.Model.DropdownModel
                        {
                            Label = state.Name,
                            Value = state.Id.ToString()
                        }).ToList(),
                        MyCities = cities,
                        MyAreas = areas,
                        AreaId = areaId
                    });
                }
            }
            

            return profileAdderess;
        }
    }
}
