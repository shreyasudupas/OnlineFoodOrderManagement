using FluentAssertions;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Features.Register;
using IdentityServer.Tests.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.Tests.UnitTests.Core.Register
{
    public class RegisterFeature : FakeDB
    {
        Mock<IUtilsService> mockUtilService;
        RegisterQueryHandler handler;
        public RegisterFeature()
        {
            mockUtilService = new Mock<IUtilsService>();

            var cities = new List<SelectListItem>
            {
                new SelectListItem { Text = "Bangalore" , Value = "Bangalore"},
                new SelectListItem { Text = "Mumbai" , Value = "Mumbai"}
            };
            mockUtilService.Setup(_ => _.GetAllCities())
                .Returns(cities);

            var states = new List<SelectListItem>
            {
                new SelectListItem { Text = "Karnataka" , Value = "Karnataka"},
                new SelectListItem { Text = "Maharastra" , Value = "Maharastra"}
            };
            mockUtilService.Setup(_ => _.GetAllStates())
                .Returns(states);

            handler = new RegisterQueryHandler(mockUtilService.Object);
        }

        [Fact]
        public async Task RegisterQuery_SuccessBuildModel()
        {
            var actual = await handler.Handle(new RegisterQuery(), It.IsAny<CancellationToken>());
            actual.Should().NotBeNull();
            actual.States.Should().HaveCount(2);
            actual.Cities.Should().HaveCount(2);
        }
    }
}
