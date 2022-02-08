using AutoMapper;
using MenuManagement_IdentityServer.Data;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
using MenuManagement_IdentityServer.Utilities.DropdownItems;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service
{
    public class UserAdministrationManager : IUserAdministrationManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;

        public UserAdministrationManager(UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , ApplicationDbContext context
            , IMapper mapper,
            IWebHostEnvironment hostingEnvironment,
            ILogger<UserAdministrationManager> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            this.mapper = mapper;
            _webHostEnvironment = hostingEnvironment;
            _logger = logger;
        }


        public async Task<EditUserGet> EditUserInfo(UserInfomationModel user)
        {
            //EditUser editUser = new EditUser();
            EditUserGet editUser = new EditUserGet();
            string ImagePath = null;

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
                GetUser.IsAdmin = user.IsAdmin;

                var previousImageName = GetUser.ImagePath;

                //Image upload the image path is got by ImagePath output variable
                ImageUpload(user.Photo, out ImagePath,previousImageName,GetUser);
                GetUser.ImagePath = ImagePath;

                //add phonenumber,image claim

                editUser.Users = mapper.Map<UserInfomationModel>(GetUser);

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

            var User = await _context.Users.Include(x=>x.Address).Where(u=>u.Id == Id).FirstOrDefaultAsync();
            if(User == null)
            {
                editUser.ErrorDescription.Add("User not found in database");
            }
            else
            {
                var Image = GetImagePath();
                var roles = await _userManager.GetRolesAsync(User);
                var claims = await _userManager.GetClaimsAsync(User);

                editUser.Claims = claims.Select(x=>x.Value).ToList();
                editUser.Roles = roles.ToList();

                editUser.Users = mapper.Map<UserInfomationModel>(User);
            }
            return editUser;
        }

        public string GetImagePath()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var ImagePath = "\\images\\";
            string ImagePathLocation = Path.Join(webRootPath, ImagePath);
            
            return ImagePathLocation;
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

                //If any Role added any
                if (RoleAddedName.Any())
                {
                    ManageRoleClaim(RoleAddedName.ToList(),true,false,GetUser, UserInRole.ToList());

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

                //If any Role is removed
                if (RoleRemovedName.Any())
                {
                    ManageRoleClaim(RoleRemovedName.ToList(), false, true, GetUser, UserInRole.ToList());

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

        public void ManageRoleClaim(List<string> NewOrRemoveRoles,bool Add,bool Remove,ApplicationUser user,List<string> UserExsitingRole)
        {
            //If add then add roles in claim, for mvp one role per customer id
            if(Add)
            {
                var roleValue = new StringBuilder();
                foreach (var role in NewOrRemoveRoles)
                {
                    if (UserExsitingRole.Count < 1)
                    {
                        roleValue.Append(role);
                        continue;
                    }

                    roleValue.Append("," + role);
                }

                var roleClaim = new Claim("role", roleValue.ToString());

                var result = _userManager.AddClaimAsync(user,roleClaim).GetAwaiter().GetResult();
                if(result.Succeeded)
                {
                    _logger.LogInformation("role claim updated");
                }
                else
                {
                    _logger.LogInformation("role claim not uodated");
                }
               
            }

            if(Remove)
            {
                var ClaimOfUser = _userManager.GetClaimsAsync(user).GetAwaiter().GetResult();

                var RoleClaim = ClaimOfUser.Where(x => x.Type == "role").FirstOrDefault();

                if(RoleClaim != null)
                {
                    var RemoveRoleClaim = _userManager.RemoveClaimAsync(user, RoleClaim).GetAwaiter().GetResult();

                    if (RemoveRoleClaim.Succeeded)
                    {
                        //If any roles are present only then add
                        if (NewOrRemoveRoles.Count > 0)
                        {
                            var roleValue = new StringBuilder();
                            foreach (var role in NewOrRemoveRoles)
                            {
                                if (UserExsitingRole.Count < 1)
                                {
                                    roleValue.Append(role);
                                    continue;
                                }

                                roleValue.Append("," + role);
                            }

                            var roleClaim = new Claim("role", roleValue.ToString());
                            var result = _userManager.AddClaimAsync(user, roleClaim).GetAwaiter().GetResult();
                            if (result.Succeeded)
                            {
                                _logger.LogInformation("role claim updated");
                            }
                            else
                            {
                                _logger.LogInformation("role claim not uodated");
                            }
                        }
                    }
                }
            }
        }

        public ManagerUserClaim ManageUserClaimGet(string UserId)
        {
            ManagerUserClaim managerUser = new ManagerUserClaim();

            //check if the user has already has the similar claim
            List<ClaimModel> DropdownValues = GetDropDownListValue(UserId);

            if (DropdownValues.Count > 0)
            {
                managerUser.status = CrudEnumStatus.success;
                managerUser.UserClaims = new SelectList(DropdownValues, nameof(ClaimModel.ClaimValue), nameof(ClaimModel.ClaimType));
            }
            else
            {
                managerUser.ErrorDescription = "DropeDown No values";
                managerUser.status = CrudEnumStatus.success;
            }
            return managerUser;
        }

        private List<ClaimModel> GetDropDownListValue(string UserId)
        {
            var User = _userManager.FindByIdAsync(UserId).GetAwaiter().GetResult();
            var UserClaims = _userManager.GetClaimsAsync(User).GetAwaiter().GetResult();
            var UserClaimsTypes = UserClaims.Select(x => x.Type).ToArray();

            //Filter the drop down claim if it already present in db
            
            var DropdownValues = _context.ClaimDropDowns.Where(c => !UserClaimsTypes.Contains(c.Value))
                .Select(x => new ClaimModel
                {
                    ClaimType = x.Name,
                    ClaimValue = x.Value
                }).ToList();

            return DropdownValues;
        }

        public async Task<ManagerUserClaimViewModel> ManageUserClaimPost(ManagerUserClaimViewModelPost model)
        {
            ManagerUserClaimViewModel resultModel = new ManagerUserClaimViewModel();

            var DropdownValues = GetDropDownListValue(model.UserId);

            resultModel.SelectionUserClaims = new SelectList(DropdownValues, nameof(ClaimModel.ClaimValue), nameof(ClaimModel.ClaimType)); ;
            //resultModel.UserClaimValue = model.UserClaimValue;

            //Here getting claim value is the actual claim type on AspNetUSerClaim table
            var GetClaimFromDropDown = DropdownValues.Where(x => x.ClaimValue == model.ClaimTypeSelected).FirstOrDefault();

            //store the values in the database
            var user = await _userManager.FindByIdAsync(model.UserId);
            //var UserClaim = new Claim(GetClaimType,model.UserClaimValue);

            //Get Claim value from AspNetUser table
            //Convert to Json object to get specific column as key value pair
            var Jobj = JObject.Parse(JsonConvert.SerializeObject(user));

            var claimValue = (string)Jobj[GetClaimFromDropDown.ClaimType]; //ClaimType is the column name on aspNetUser table,Hence using claimType as key to search

            if(claimValue != null)
            {
                var UserClaim = new Claim(model.ClaimTypeSelected, claimValue);

                var result = await _userManager.AddClaimAsync(user, UserClaim);

                if (result.Succeeded)
                {
                    resultModel.status = CrudEnumStatus.success;
                    //remove from dropdown 
                    DropdownValues.Remove(GetClaimFromDropDown);
                }
                else
                {
                    resultModel.status = CrudEnumStatus.failure;
                    result.Errors.ToList().ForEach(ele =>
                    {
                        resultModel.ErrorDescription.Add(ele.Description);
                    });
                }
            }
            else
            {
                resultModel.status = CrudEnumStatus.failure;
                resultModel.ErrorDescription.Add("Claim value is null, Please fill required field and then add the claim");
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

        public void ImageUpload(IFormFile uploadFile,out string filePath,string PreviousImageName,ApplicationUser user)
        {
            string uniqueFileName = null;
            if (uploadFile != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                //Delete previous image from location
                ImageDelete(uploadFolder, PreviousImageName,user);

                uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadFile.FileName;

                string uploadFolderPath = Path.Combine(uploadFolder, uniqueFileName);

                //Resize the image
                using(var image = Image.Load(uploadFile.OpenReadStream()))
                {
                    string newSize = Resize(image, 248, 142);
                    string[] aSize = newSize.Split(',');
                    image.Mutate(h=>h.Resize(Convert.ToInt32(aSize[1]),Convert.ToInt32(aSize[0])));
                    image.Save(uploadFolderPath);
                }

                uploadFile.CopyTo(new FileStream(uploadFolderPath, FileMode.Create));
                filePath = uniqueFileName;
            }
            else
            {
                filePath = string.Empty;
            }
        }

        public string Resize(Image image,int maxWidth,int maxHeight)
        {
            if(image.Width > maxWidth || image.Height > maxHeight)
            {
                double widthRatio = (double)image.Width / (double)maxWidth;
                double heightRatio = (double)image.Height / (double)maxHeight;
                double ratio = Math.Max(widthRatio, heightRatio);
                int newWidth = (int)(image.Width / ratio);
                int newHeight = (int)(image.Height / ratio);
                return newHeight.ToString() + "," + newWidth.ToString();
            }
            else
            {
                return image.Height.ToString() + "," + image.Width.ToString();
            }
        }

        public void ImageDelete(string ImagePath,string PreviousImageName,ApplicationUser user)
        {
            if(!string.IsNullOrEmpty(PreviousImageName))
            {
                if (File.Exists(Path.Combine(ImagePath, PreviousImageName)))
                {
                    //create a claim so that old claim will be removed
                    Claim imageclaim = new Claim("picture", PreviousImageName);
                    ClaimDelete(imageclaim, user);

                    //delete the file from location
                    File.Delete(Path.Combine(ImagePath, PreviousImageName));
                    _logger.LogInformation("File with {0} filename is deleted successfully", PreviousImageName);
                }
                else
                {
                    _logger.LogInformation("File with {0} filename is not present", PreviousImageName);
                }
            }
            else
            {
                _logger.LogInformation("No image is present previously");
            }
            
        }

        public void ClaimDelete(Claim claim,ApplicationUser user)
        {
            var result = _userManager.RemoveClaimAsync(user, claim).GetAwaiter().GetResult();
            if(result.Succeeded)
            {
                _logger.LogInformation("Claim successfully removed");
            }
            else
            {
                _logger.LogInformation("Claim error when removing");
            }
        }

        public ClaimsViewModel GetAllDropDownClaims()
        {
            ClaimsViewModel model = new ClaimsViewModel();
            //gets all the claims that can be used in the application
            var GetAllClaimsInApp = _context.ClaimDropDowns.ToList();

            foreach(var claim in GetAllClaimsInApp)
            {
                model.Claims.Add(claim);
            }

            return model;
        }
        public EditClaimViewModel EditClaim(EditClaimViewModel viewModel)
        {
            EditClaimViewModel model = new EditClaimViewModel();

            try
            {
                if (viewModel.ClaimId != null)
                {
                    var ClaimData = _context.ClaimDropDowns.Where(c => c.Id == viewModel.ClaimId).FirstOrDefault();

                    if (ClaimData != null)
                    {
                        ClaimData.Id = viewModel.ClaimId ?? 0;
                        ClaimData.Name = viewModel.ClaimType;
                        ClaimData.Value = viewModel.ClaimValue;

                        _context.SaveChanges();

                        model.status = CrudEnumStatus.success;
                    }
                    else
                    {
                        model.ErrorDescription.Add("Claim not present");
                        model.status = CrudEnumStatus.failure;
                    }
                }
                else if (viewModel.ClaimType != null && viewModel.ClaimValue != null)
                {
                    //new claim to be added
                    var Claim = new ClaimDropDown
                    {
                        Name = viewModel.ClaimType,
                        Value = viewModel.ClaimValue
                    };

                    _context.ClaimDropDowns.Add(Claim);
                    _context.SaveChanges();

                    model.status = CrudEnumStatus.success;

                }
                else
                {
                    model.ErrorDescription.Add("Claims not entered");
                    model.status = CrudEnumStatus.failure;
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex, "error in EditClaim");
            }
            
            return model;
        }
        public EditClaimViewModel GetClaimById(int? Id)
        {
            EditClaimViewModel model = new EditClaimViewModel();
            if(Id != null)
            {
                var claim = _context.ClaimDropDowns.Where(x => x.Id == Id).FirstOrDefault();
                if(claim != null)
                {
                    model.ClaimId = claim.Id;
                    model.ClaimType = claim.Name;
                    model.ClaimValue = claim.Value;

                    model.status = CrudEnumStatus.success;
                }
                else
                {
                    model.ErrorDescription.Add("No Claim is present in database");
                    model.status = CrudEnumStatus.failure;
                }
            }
            
            return model;
        }

        public UserInformationModel GetUserInformationDetail(string UserId)
        {
            UserInformationModel model = new UserInformationModel();

            var User = _context.Users.Where(u => u.Id == UserId).FirstOrDefault();
            if(User != null)
            {
                var ModelMap = mapper.Map<UserInformationModel>(User);

                if(ModelMap!=null)
                {
                    model = ModelMap;
                    model.status = CrudEnumStatus.success;
                }
                else
                {
                    model.status = CrudEnumStatus.failure;
                    model.ErrorDescription.Add("Mapping Failed");
                }
            }else
            {
                model.status = CrudEnumStatus.NotFound;
                model.ErrorDescription.Add("user not found in database");
            }
            return model;
        }

        public UserAddressPartialViewModel SaveUserAddress(UserAddressPartialViewModel viewModel)
        {
            var User = _context.Users.Include(x=>x.Address).Where(u => u.Id == viewModel.UserId).FirstOrDefault();
            viewModel.States = SelectListUtility.GetStateItems();
            viewModel.Cities = SelectListUtility.GetCityItems();

            //Add opertaion
            if (User!=null && viewModel.UserAddressId < 1)
            {
                if (User.Address == null)
                    User.Address = new List<UserAddress>();
                
                User.Address.Add(new UserAddress
                {
                   FullAddress = viewModel.FullAddress,
                   City = viewModel.City,
                   State = viewModel.State,
                   IsActive = viewModel.IsActive
                });

                //Find if there is any active address present
                if (User.Address.Where(x=>x.IsActive == true).Select(x=>x.Id).Count() > 1)
                {
                    viewModel.ErrorDescription.Add("User has more than two active address");
                    viewModel.status = CrudEnumStatus.failure;
                    return viewModel;
                }

                _context.SaveChanges();

                //Add new claim
                ManageUserAddressClaim(User);

                viewModel.status = CrudEnumStatus.success;

            }
            else if(viewModel.UserAddressId>0 && User!=null)
            {
                
                var UserAddress = _context.UserAddresses.Where(ua => ua.Id == viewModel.UserAddressId).FirstOrDefault();

                if(UserAddress != null)
                {
                    UserAddress.FullAddress = viewModel.FullAddress;
                    UserAddress.City = viewModel.City;
                    UserAddress.State = viewModel.State;
                    UserAddress.IsActive = viewModel.IsActive;

                    //Check if there are more than one active addresses
                    if (User.Address.Where(x => x.IsActive == true).Select(x => x.Id).Count() > 1)
                    {
                        viewModel.ErrorDescription.Add("User has more than two active address");
                        viewModel.status = CrudEnumStatus.failure;
                        return viewModel;
                    }
                    _context.SaveChanges();

                    //Edit new claim
                    ManageUserAddressClaim(User);

                    viewModel.status = CrudEnumStatus.success;
                }
                else
                {
                    viewModel.ErrorDescription.Add("UserAddress not found");
                    viewModel.status = CrudEnumStatus.NotFound;
                }
            }
            else
            {
                _logger.LogInformation("User not found");
                viewModel.ErrorDescription.Add("User not found");
                viewModel.status = CrudEnumStatus.NotFound;
            }
            return viewModel;
        }

        public UserAddressPartialViewModel GetUserAddressInformation(GetUserAddressModel request)
        {
            var UserAddress = _context.UserAddresses.Where(u => u.Id == request.UserAddressId).FirstOrDefault();
            return new UserAddressPartialViewModel
            {
                Cities = SelectListUtility.GetCityItems(),
                States = SelectListUtility.GetStateItems(),
                FullAddress = UserAddress.FullAddress,
                City = UserAddress.City,
                State = UserAddress.State,
                IsActive = UserAddress.IsActive
            };
        }

        public void ManageUserAddressClaim(ApplicationUser user)
        {
            _logger.LogInformation("ManageUserAddressClaim started");

            var userClaim = _userManager.GetClaimsAsync(user).GetAwaiter().GetResult();

            var GetUserAddress = userClaim.Where(x => x.Type == "address").FirstOrDefault();

            if(GetUserAddress != null)
            {
                _logger.LogInformation("User address Claim found");

                var deleteUserAddressClaim = _userManager.RemoveClaimsAsync(user, new List<Claim> { GetUserAddress }).GetAwaiter().GetResult();

                if(deleteUserAddressClaim.Succeeded)
                {
                    _logger.LogInformation("User address Claim succesfully deleted");

                    //add new address claim
                    var NewClaim = new Claim("address", JsonConvert.SerializeObject(user.Address));
                    
                    var newAddressClaim = _userManager.AddClaimAsync(user, NewClaim).GetAwaiter().GetResult();

                    if(newAddressClaim.Succeeded)
                    {
                        _logger.LogInformation("New User address Claim add succeed");
                    }
                }
            }
            else
            {
                _logger.LogInformation("Add new Address claim started");

                //add new address claim
                var NewClaim = new Claim("address", JsonConvert.SerializeObject(user.Address));

                var newAddressClaim = _userManager.AddClaimAsync(user, NewClaim).GetAwaiter().GetResult();

                if (newAddressClaim.Succeeded)
                {
                    _logger.LogInformation("Add new Address claim ended");
                }
            }

            _logger.LogInformation("ManageUserAddressClaim ended");
        }
    }
}
