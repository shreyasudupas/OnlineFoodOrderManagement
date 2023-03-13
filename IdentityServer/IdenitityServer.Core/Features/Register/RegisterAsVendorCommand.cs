using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using MediatR;
using MenuManagement.HttpClient.Domain.Interface;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Register
{
    public class RegisterAsVendorCommand : IRequest<VendorRegister>
    {
        public VendorRegister vendorRegister { get; set; }
    }

    public class RegisterAsVendorCommandHandler : IRequestHandler<RegisterAsVendorCommand, VendorRegister>
    {
        private readonly IAuthService _authService;
        private readonly IIdsHttpClientWrapper _idsHttpClientWrapper;

        public RegisterAsVendorCommandHandler(IIdsHttpClientWrapper idsHttpClientWrapper,
            IAuthService authService)
        {
            _idsHttpClientWrapper = idsHttpClientWrapper;
            _authService = authService;
        }

        public async Task<VendorRegister> Handle(RegisterAsVendorCommand request, CancellationToken cancellationToken)
        {
            var registerVendor = await _authService.RegisterAsVendor(request.vendorRegister);
            if (!registerVendor.Item1.Errors.Any() && !string.IsNullOrEmpty(registerVendor.Item2))
            {
                var token = await GetAccessToken();
                if(!string.IsNullOrEmpty(token))
                {
                    var vendorResult = await AddVendorDetailsToInventoryMicroservice(request.vendorRegister, token);
                    var notificationResult = await AddNewNotificationToNotificationMicroservice(registerVendor.Item2, registerVendor.Item1, token);
                }
            }

            return request.vendorRegister;
        }

        public async Task<string> GetAccessToken()
        {
            var result = await _idsHttpClientWrapper.GetApiAccessToken();
            return result;
        }

        public async Task<NotificationModel> AddNewNotificationToNotificationMicroservice(string userId,VendorRegister vendorRegister,string token)
        {
            var model = new NotificationModel
            {
                Title = "Welcome to Menu Vendor",
                Description="Welcome to Menu Vendor Application",
                UserId = userId,
                Read= false,
                Role="vendror",
                SendAll=false,
            };

            var result = await _idsHttpClientWrapper.AddApiCallAsync("", model, token, "IDSClient");

            return model;
        }

        public async Task<VendorModel> AddVendorDetailsToInventoryMicroservice(VendorRegister vendorRegister,string token)
        {
            var model = new VendorModel
            {
                VendorName = vendorRegister.VendorName,
                VendorDescription = vendorRegister.VendorDescription,
                Rating = 0,
                Active=  true,
                State = vendorRegister.States.Where(x=>x.Value == vendorRegister.StateId).Select(x=>x.Text).FirstOrDefault(),
                City = vendorRegister.Cities.Where(x=>x.Value == vendorRegister.CityId).Select(x=>x.Text).FirstOrDefault(),
                Area = vendorRegister.Areas.Where(x => x.Value == vendorRegister.AreaId).Select(x => x.Text).FirstOrDefault(),
                AddressLine1 = vendorRegister.Address,
            };

            var result = await _idsHttpClientWrapper.AddApiCallAsync("", model, token, "VendorClient");

            return model;
        }
    }
}
