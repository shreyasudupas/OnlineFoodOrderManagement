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
    //public class RegisterFeature : FakeDB
    //{
    //    Mock<IUtilsService> mockUtilService;
    //    RegisterQueryHandler handler;
    //    public RegisterFeature()
    //    {
    //        mockUtilService = new Mock<IUtilsService>();
    //        handler = new RegisterQueryHandler(mockUtilService.Object);
    //    }

    //    [Fact]
    //    public async Task RegisterQuery_SuccessBuildModel()
    //    {
    //        var actual = await handler.Handle(new RegisterQuery { ReturnUrl = "test.url"}, It.IsAny<CancellationToken>());
    //        actual.Should().NotBeNull();
    //        actual.ReturnUrl.Should().Be("test.url");
    //    }
    //}
}
