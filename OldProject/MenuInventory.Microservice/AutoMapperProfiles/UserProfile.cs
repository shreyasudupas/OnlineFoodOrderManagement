using AutoMapper;
using Common.Utility.Models;

namespace MenuInventory.Microservice.AutoMapperProfiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddress, Common.Mongo.Database.Data.Models.UserAddress>();
        }
    }
}
