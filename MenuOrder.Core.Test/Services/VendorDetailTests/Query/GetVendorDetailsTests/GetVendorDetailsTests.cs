using FluentAssertions;
using MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorDetails;
using MenuManagement.Core.Test.Base;
using MenuManagment.Domain.Mongo.Entities;
using MenuManagment.Domain.Mongo.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MenuManagement.Core.Test.Services.VendorDetailTests.Query.GetVendorDetailsTests
{
    public class GetVendorDetailsTests : BaseMockSetup
    {
        Mock<IMenuRepository> mockMenuRepo;
        GetVendorDetailsQueryHandler handler;

        private void Setup()
        {
            mockMenuRepo = new Mock<IMenuRepository>();
            handler = new GetVendorDetailsQueryHandler(mockMenuRepo.Object, _mapper);

            var result = new List<VendorDetail>
            {
                new VendorDetail{ VendorId="vendorId",VendorName="test",Description="test ses", Location="Bangalore",Rating=6}
            };
            mockMenuRepo.Setup(_ => _.ListAllVendorDetails("kathreguppe"))
                .ReturnsAsync(result);
        }

        [Fact]
        public async Task GetVendorDetails_Success()
        {
            Setup();

            var actual = await handler.Handle(new GetVendorDetailsQuery { Locality = "kathreguppe" }
            ,It.IsAny<CancellationToken>() );

            actual.Should().NotBeEmpty();
            actual.Should().HaveCount(1);
        }
    }
}
