using AutoMapper;
using Common.Utility.BuisnessLayer.IBuisnessLayer;
using Common.Utility.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Utility.BuisnessLayer
{
    public class UserBuisness : IUser
    {
        private readonly MenuOrderManagementContext _dbContext;
        private readonly IMapper _mapper;
        public UserBuisness(MenuOrderManagementContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Users> AddOrGetUserDetails(UserProfile userProfile)
        {
            Users UserProfile = new Users();

            var user = await _dbContext.tblUsers.Where(x => x.UserName == userProfile.Username).FirstOrDefaultAsync();
            if (user == null)
            {
                user.UserName = userProfile.Username;
                user.FullName = userProfile.NickName;
                user.PictureLocation = userProfile.PictureLocation;
                user.RoleId = userProfile.RoleId;
                user.Points = 0;
                user.CartAmount = 0;
                user.CreatedDate = DateTime.Now;

                _dbContext.tblUsers.Add(user);
                _dbContext.SaveChanges();
            }
            //throw new Exception("I am throwing exceptions");

            UserProfile = _mapper.Map<Users>(user);

            return UserProfile;
        }
    }
}
