using FluentAssertions;
using MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorMenuColumnDetails;
using MenuManagement.Core.Test.Base;
using MenuManagment.Domain.Mongo.Entities;
using MenuManagment.Domain.Mongo.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MenuManagement.Core.Test.Services.VendorDetailTests.Query.GetVendorMenuColumnDetailTests
{
    public class GetVendorMenuColumnDetialTests : BaseMockSetup
    {
        GetVendorMenuColumnDetailsQueryHandler handler;
        Mock<IMenuRepository> mockMenuRepo;
        
        public GetVendorMenuColumnDetialTests()
        {
            mockMenuRepo = new Mock<IMenuRepository>();

            handler = new GetVendorMenuColumnDetailsQueryHandler(mockMenuRepo.Object,_mapper);

            var result = new List<VendorColumnDetailEntity>
            {
                new VendorColumnDetailEntity{ ColumnName="id",DisplayName="ID",DisplayOnScreen="none"},
                new VendorColumnDetailEntity{ ColumnName="menu name",DisplayName="Menu Name",DisplayOnScreen=""},
                new VendorColumnDetailEntity{ ColumnName="menu type",DisplayName="Menu Type",DisplayOnScreen=""},
                new VendorColumnDetailEntity{ ColumnName="price",DisplayName="Price",DisplayOnScreen=""}
            };
            mockMenuRepo.Setup(_ => _.ListVendorMenuColumnDetails("vendorid"))
                .ReturnsAsync(result);
        }

        [Fact]
        public async Task GetVendorMenuColumnDetailsQuery_Must_Return_ListofValues()
        {
            var actual = await handler.Handle(new GetVendorMenuColumnDetailsQuery { VendorId = "vendorid" }, 
                It.IsAny<CancellationToken>());

            actual.Should().NotBeNullOrEmpty();
            actual.Should().HaveCount(4);
        }

        [Fact]
        public async Task GetVendorMenuColumnDetailsQuery_Must_Return_NullWithIncorrect_VendorID()
        {
            var actual = await handler.Handle(new GetVendorMenuColumnDetailsQuery { VendorId = "vendoridddds" },
                It.IsAny<CancellationToken>());

            actual.Should().BeNullOrEmpty();
            
        }
    }
}
