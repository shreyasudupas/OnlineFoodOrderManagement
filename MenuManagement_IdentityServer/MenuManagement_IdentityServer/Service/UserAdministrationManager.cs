using AutoMapper;
using MenuManagement_IdentityServer.Data;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service
{
    public class UserAdministrationManager : IUserAdministrationManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public UserAdministrationManager(UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , ApplicationDbContext context
            , IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            this.mapper = mapper;
        }


        public async Task<EditUserGet> EditUserInfo(ApplicationUser user)
        {
            //EditUser editUser = new EditUser();
            EditUserGet editUser = new EditUserGet();

            var GetUser = await _userManager.FindByIdAsync(user.Id);

            if (GetUser == null)
            {
                editUser.ErrorDescription.Add("User not found in database");
            }
            else
            {
                GetUser.UserName = user.UserName;
                GetUser.Email = user.Email;
                GetUser.PhoneNumber = user.PhoneNumber;
                GetUser.Address = user.Address;
                GetUser.City = user.City;
                GetUser.IsAdmin = user.IsAdmin;

                editUser.Users = GetUser;

                //Get Roles and claims as well
                var roles = await _userManager.GetRolesAsync(GetUser);
                var claims = await _userManager.GetClaimsAsync(GetUser);
                editUser.Roles = roles.ToList();
                editUser.Claims = claims.Select(x => x.Value).ToList();

                var result = await _userManager.UpdateAsync(GetUser);
                if (result.Succeeded)
                {
                    //editUser.User = user;
                    //return editUser;
                    return editUser;
                }

                foreach (var err in result.Errors)
                {
                    editUser.ErrorDescription.Add(err.Description);
                }
            }
            return editUser;
        }

        public IEnumerable<ApplicationUser> GetAllApplicationUsers()
        {
            var Users = _userManager.Users.ToList();
            return Users;
        }

        public async Task<EditUserGet> GetApplicationUserInfo(string Id)
        {
            EditUserGet editUser = new EditUserGet();

            var User = await _userManager.FindByIdAsync(Id);
            if(User == null)
            {
                editUser.ErrorDescription.Add("User not found in database");
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(User);
                var claims = await _userManager.GetClaimsAsync(User);

                editUser.Claims = claims.Select(x=>x.Value).ToList();
                editUser.Roles = roles.ToList();

                editUser.Users = User;
            }
            return editUser;
        }

        public async Task<ManagerUserRole> GetManageRoleInformation(string UserId)
        {
            ManagerUserRole managerUser = new ManagerUserRole();
            managerUser.UserId = UserId;

            var userProfile = await _userManager.FindByIdAsync(UserId);

            if(userProfile == null)
            {
                managerUser.status = CrudEnumStatus.failure;
                managerUser.ErrorDescription.Add($"user with {UserId} not present in the database");
            }
            else
            {
                //Existing role of user
                var UserRole = await _userManager.GetRolesAsync(userProfile);
                var GetAllRoles = _roleManager.Roles.ToList();

                //if no roles then add
                GetAllRoles.ForEach(role =>
                {
                    //see if the role is present in UserRoles
                    var isRolePresent = UserRole.Any(x => x == role.Name);

                    managerUser.Role.Add(new UserRoleSelection { 
                        RoleName = role.Name,
                        IsSelected = isRolePresent,
                        RoleId = role.Id
                    });
                });

                managerUser.status = CrudEnumStatus.success;
            }

            return managerUser;
        }

        public async Task<ManagerUserRole> SaveManageRoleInformation(List<ManageUserPost> model)
        {
            ManagerUserRole managerUserRole = new ManagerUserRole();

            //rebuid the View Model
            model.ForEach(item => 
            {
                managerUserRole.Role.Add(new UserRoleSelection
                {
                    RoleName = item.RoleName,
                    IsSelected = item.IsSelected
                });
            });

            var UserId = model.Select(x => x.UserId).First();
            var GetUser = await _userManager.FindByIdAsync(UserId);

            if(GetUser != null)
            {
                //Get User in existing role
                var UserInRole = await _userManager.GetRolesAsync(GetUser);

                //check if the role is present
                var RoleAddedName = model.Where(role=>role.IsSelected).Select(x => x.RoleName).Except(UserInRole);
                var RoleRemovedName = UserInRole.Except(model.Where(role => role.IsSelected).Select(x => x.RoleName));

                if (RoleAddedName.Any())
                {
                    var result = await _userManager.AddToRolesAsync(GetUser, RoleAddedName);
                    if (result.Succeeded)
                    {
                        managerUserRole.status = CrudEnumStatus.success;
                    }
                    else
                    {
                        managerUserRole.status = CrudEnumStatus.failure;
                        result.Errors.ToList().ForEach(err=> {
                            managerUserRole.ErrorDescription.Add(err.Description);
                        });
                        
                        return managerUserRole;
                    }
                }

                if (RoleRemovedName.Any())
                {
                    var result = await _userManager.RemoveFromRolesAsync(GetUser, RoleRemovedName);
                    if (result.Succeeded)
                    {
                        managerUserRole.status = CrudEnumStatus.success;
                    }
                    else
                    {
                        managerUserRole.status = CrudEnumStatus.failure;
                        result.Errors.ToList().ForEach(err => {
                            managerUserRole.ErrorDescription.Add(err.Description);
                        });

                        return managerUserRole;
                    }
                }
            }
            return managerUserRole;
        }

        public ManagerUserClaim ManageUserClaimGet(string UserId)
        {
            ManagerUserClaim managerUser = new ManagerUserClaim();

            //var DropdownValues = _context.ClaimDropDowns.Select(x=> new KeyValuePair<string,string>(x.Name,x.Value)).ToDictionary(x=>x.Key,l=>l.Value);

            //check if the user has already has the similar claim
            var User = _userManager.FindByIdAsync(UserId).GetAwaiter().GetResult();
            var UserClaims = _userManager.GetClaimsAsync(User).GetAwaiter().GetResult();
            var UserClaimsTypes = UserClaims.Select(x => x.Type).ToArray();

            //Filter the drop down claim if it already present in db
            //var DropdownValues = _context.ClaimDropDowns.Where(c=> !UserClaimsTypes.Contains(c.Value))
            //    .Select(x => new KeyValuePair<string, string>(x.Name, x.Value)).ToDictionary(x => x.Key, l => l.Value);
            var DropdownValues = _context.ClaimDropDowns.Where(c => !UserClaimsTypes.Contains(c.Value))
                .Select(x => new ClaimModel
                {
                    ClaimType = x.Name,
                    ClaimValue = x.Value
                }).ToList();

            if (DropdownValues.Count >0)
            {
                managerUser.status = CrudEnumStatus.success;
                managerUser.UserClaims = new SelectList(DropdownValues, nameof(ClaimModel.ClaimType),nameof(ClaimModel.ClaimValue));
            }
            else
            {
                managerUser.ErrorDescription = "DropeDown No values";
                managerUser.status = CrudEnumStatus.success;
            }
            return managerUser;
        }

        public async Task<ManagerUserClaimViewModel> ManageUserClaimPost(ManagerUserClaimViewModelPost model)
        {
            ManagerUserClaimViewModel resultModel = new ManagerUserClaimViewModel();

            var DropdownValues = _context.ClaimDropDowns.Select(x => new ClaimModel
            {
                ClaimType = x.Name,
                ClaimValue = x.Value
            }).ToList();

            resultModel.SelectionUserClaims = new SelectList(DropdownValues, nameof(ClaimModel.ClaimType), nameof(ClaimModel.ClaimValue)); ;
            resultModel.UserClaimValue = model.UserClaimValue;

            //Here getting claim value is the actual claim type on AspNetUSerClaim table
            var GetClaimType = DropdownValues.Where(x => x.ClaimValue == model.ClaimTypeSelected).Select(claim=>claim.ClaimValue).FirstOrDefault();

            //store the values in the database
            var user = await _userManager.FindByIdAsync(model.UserId);
            var UserClaim = new Claim(GetClaimType,model.UserClaimValue);

            var result = await _userManager.AddClaimAsync(user,UserClaim);
            if(result.Succeeded)
            {
                resultModel.status = CrudEnumStatus.success;
            }
            else
            {
                resultModel.status = CrudEnumStatus.failure;
                result.Errors.ToList().ForEach(ele=>
                {
                    resultModel.ErrorDescription.Add(ele.Description);
                });
            }
            
            return resultModel;
        }

        public async Task<DeleteUserClaimGet> GetDeleteUserClaimInfo(string UserId)
        {
            DeleteUserClaimGet deleteUserClaimGet = new DeleteUserClaimGet();
            deleteUserClaimGet.UserId = UserId;

            var user = await _userManager.FindByIdAsync(UserId);

            if(user != null)
            {
                //get all user claims
                var claims = await _userManager.GetClaimsAsync(user);

                if(claims.Count > 0)
                {
                    //map the UserClaim List
                    var MapUserClaim = mapper.Map<List<UserClaimList>>(claims);

                    deleteUserClaimGet.UserClaims = MapUserClaim;

                    deleteUserClaimGet.status = CrudEnumStatus.success;
                }
                else
                {
                    deleteUserClaimGet.ErrorDescription.Add($"User with {UserId} does not have any claims in database");
                    deleteUserClaimGet.status = CrudEnumStatus.failure;
                }
                
            }
            else
            {
                deleteUserClaimGet.ErrorDescription.Add($"User with {UserId} not present in database");
                deleteUserClaimGet.status = CrudEnumStatus.failure;
            }

            return deleteUserClaimGet;
        }

        public async Task<DeleteUserClaimGet> DeleteUserClaimInfo(List<DeleteUserClaimViewModel> model,string UserId)
        {
            DeleteUserClaimGet deleteUserClaimGet = new DeleteUserClaimGet();
            deleteUserClaimGet.UserId = UserId;

            if (!string.IsNullOrEmpty(UserId))
            {
                var user = await _userManager.FindByIdAsync(UserId);
                if (user != null)
                {
                    var GetClaims = await _userManager.GetClaimsAsync(user);

                    //First Remove All claims
                    var resultClaimRemove = await _userManager.RemoveClaimsAsync(user,GetClaims);
                    //Get all claim type which are remaining 
                    var GetOnlyClaimType = model.Where(x=>!x.IsDeleteSelection).Select(x => x.ClaimType).ToArray();

                    var RemainingClaims = GetClaims.Where(rc => GetOnlyClaimType.Contains(rc.Type));
                    var AddRemainingClaims = await _userManager.AddClaimsAsync(user, RemainingClaims);
                }
                else
                {
                    deleteUserClaimGet.ErrorDescription.Add($"User with {UserId} does not have any claims in database");
                    deleteUserClaimGet.status = CrudEnumStatus.failure;
                }

            }
            else
            {
                deleteUserClaimGet.ErrorDescription.Add($"User with {UserId} not present in database");
                deleteUserClaimGet.status = CrudEnumStatus.failure;
            }
            return deleteUserClaimGet;
        }

        public async Task<UserDashboard> GetUserDashBoardInformation(string UserId)
        {
            UserDashboard userDashboard = new UserDashboard();
            //get User
            var User = await _userManager.FindByIdAsync(UserId);

            if(User != null)
            {
                userDashboard.User = User;

                //get all Roles of User
                var Roles = await _userManager.GetRolesAsync(User);
                if(Roles != null)
                {
                    userDashboard.Roles = Roles.ToList();
                }

                //Get All Claims
                var GetClaims = await _userManager.GetClaimsAsync(User);
                if(GetClaims != null)
                {
                    userDashboard.Claims = GetClaims.Select(claim => new UserClaim
                    {
                        ClaimType = claim.Type,
                        ClaimValue = claim.Value
                    }).ToList();
                }
                
            }
            else
            {
                userDashboard.ErrorDescription.Add($"No User with this Username: {UserId} is present in the database");
                userDashboard.status = CrudEnumStatus.failure;
            }

            return userDashboard;
        }
    }
}
