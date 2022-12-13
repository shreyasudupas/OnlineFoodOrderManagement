using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.Domain.Request;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Features.Utility;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.API.APIControllers
{
    [Authorize(LocalApi.PolicyName)]
    public class UtilitiesController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UtilitiesController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("/api/utility/getAllLocations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegisteredLocationReponse>))]
        public async Task<List<RegisteredLocationReponse>> GetAllLocation()
        {
            return await Mediator.Send(new GetAllLocationQuery());
        }

        [HttpGet("/api/utility/getAllCities")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AddressDropdownModel>))]
        public async Task<List<AddressDropdownModel>> GetAllCities()
        {
            return await Mediator.Send(new GetAllCitiesQuery());
        }

        [HttpGet("/api/utility/getCityByStateId")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<DropdownModel>))]
        public async Task<List<DropdownModel>> GetCityById(int StateId)
        {
            return await Mediator.Send(new GetCityByStateIdQuery { StateId = StateId });
        }

        [HttpGet("/api/utility/getAreaByCityId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DropdownModel>))]
        public async Task<List<DropdownModel>> GetAreaById(int CityId)
        {
            return await Mediator.Send(new GetAreaByCityIdQuery { CityId = CityId });
        }

        [HttpGet("/api/utility/getAllowedScopes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DropdownModel>))]
        public async Task<List<DropdownModel>> GetAllowedScopes()
        {
            return await Mediator.Send(new GetAllowedScopeListQuery ());
        }

        [AllowAnonymous]
        [HttpPost("/api/utility/UploadPhoto")]
        public async Task<UserProfile> UploadPhoto([FromForm] UserProfilePicture userProfilePicture)
        {
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            string uniqueImageName = Guid.NewGuid().ToString() + " " + userProfilePicture.UserId + ".png"; ;

            var userResult = await Mediator.Send(new UploadUserProfilePictureCommand {
                UserId = userProfilePicture.UserId,
                ImageName = uniqueImageName
            });

            if (userResult != null)
            {
                if (!string.IsNullOrEmpty(userResult.ImagePath))
                {
                    //delete the existing photo
                    if (System.IO.File.Exists(Path.Combine(uploadFolder, userResult.ImagePath)))
                    {
                        //delete the file from location
                        System.IO.File.Delete(Path.Combine(uploadFolder, userResult.ImagePath));

                        uniqueImageName = Guid.NewGuid().ToString() + " " + userResult.Id +".png";
                    }
                }
                string uploadFolderPath = Path.Combine(uploadFolder, uniqueImageName);

                userProfilePicture.Image.CopyTo(new FileStream(uploadFolderPath, FileMode.Create));

                return userResult;
            }
            else
            {
                return null;
            }
        }
    }
}
