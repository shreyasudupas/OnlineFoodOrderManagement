using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class UploadUserProfilePictureCommand: IRequest<UserProfile>
    {
        public string ImageName { get; set; }
        public string UserId { get; set; }
    }

    public class UploadUserProfilePictureCommandHandler : IRequestHandler<UploadUserProfilePictureCommand, UserProfile>
    {
        private readonly IUtilsService _utilsService;
        private readonly ILogger _logger;
        public UploadUserProfilePictureCommandHandler(IUtilsService utilsService
            , ILogger<UploadUserProfilePictureCommandHandler> logger)
        {
            _utilsService = utilsService;
            _logger = logger;
        }
        public async Task<UserProfile> Handle(UploadUserProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var users = await _utilsService.GetUserProfile(request.UserId);
            if(users!= null)
            {
                //update image path
                var oldImagePath = await _utilsService.UpdateUserProfileImage(users.Id,request.ImageName);

                //if old path is present then send it back so that it can be deleted and replaced
                return new UserProfile
                {
                    Id = users.Id,
                    ImagePath = (string.IsNullOrEmpty(oldImagePath)?"":oldImagePath)
                };
            }
            else
            {
                _logger.LogError($"No users with this userId {request.UserId} exists");
                return null;
            }
        }
    }
}
