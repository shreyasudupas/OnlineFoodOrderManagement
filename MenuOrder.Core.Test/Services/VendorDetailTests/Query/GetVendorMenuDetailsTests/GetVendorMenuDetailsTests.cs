using FluentAssertions;
using MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorMenuDetails;
using MenuManagement.Core.Test.Base;
using MenuManagment.Domain.Mongo.Entities;
using MenuManagment.Domain.Mongo.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MenuManagement.Core.Test.Services.VendorDetailTests.Query.GetVendorMenuDetailsTests
{
    public class GetVendorMenuDetailsTests : BaseMockSetup
    {
        Mock<IMenuRepository> mockMenuRepo;
        GetVendorMenuDetailsQueryHandler handler;
        VendorMenuDetail vendorMenuResult;

        private void Setup()
        {
            mockMenuRepo = new Mock<IMenuRepository>();
            handler = new GetVendorMenuDetailsQueryHandler(mockMenuRepo.Object, _mapper);

            vendorMenuResult = new VendorMenuDetail
            {
                ColumnDetail = new List<MenuColumnDetail>
                {
                    new MenuColumnDetail
                    {
                        Field = "id",
                        Header = "ID",
                        DisplayOnScreen = "none"
                    },
                    new MenuColumnDetail
                    {
                        Field = "menu name",
                        Header = "Menu",
                        DisplayOnScreen = "none"
                    },
                    new MenuColumnDetail
                    {
                        Field = "menu type",
                        Header = "Menu Type",
                        DisplayOnScreen = "none"
                    },
                    new MenuColumnDetail
                    {
                        Field = "Price",
                        Header = "Price",
                        DisplayOnScreen = "none"
                    }
                },
                Data = new List<object>
                {
                    new
                    {
                        id = "fb5ee2b1-b64f-40a2-8171-61e5b479b940",
                        menu_name = "idly",
                        menu_type = "breakfast",
                        Price = 20
                    }
                }
            };
            mockMenuRepo.Setup(_ => _.ListVendorMenuDetails("vendorid", "testlocation"))
                .ReturnsAsync(vendorMenuResult);
        }

        [Fact]
        public async Task GetMenuDetailsBasedOnVendorid_Success()
        {
            Setup();


            var actual = await handler.Handle(new GetVendorMenuDetailsQuery { Location = "testlocation", VendorId = "vendorid" }
            ,It.IsAny<CancellationToken>() );

            actual.Should().NotBeNull();
            actual.MenuColumnDetail.Should().SatisfyRespectively(first => {
                first.Field.Should().Be("id");
                first.Header.Should().Be("ID");
                first.Display.Should().Be("none");
            },
            second=> {
                second.Field.Should().Be("menu name");
                second.Header.Should().Be("Menu");
                second.Display.Should().Be("none");
            },
            third =>{
                third.Field.Should().Be("menu type");
                third.Header.Should().Be("Menu Type");
                third.Display.Should().Be("none");
            },
            fourth=> {
                fourth.Field.Should().Be("Price");
                fourth.Header.Should().Be("Price");
                fourth.Display.Should().Be("none");
            });
        }

        [Theory]
        [InlineData("vendor1","location1")]
        [InlineData("vendor2", "location2")]
        public async Task GetMenuDetailsBasedOnVendorid_MustSendEmptyIfInvalidLocationOrVendorId(string vendorId,string Location)
        {
            Setup();

            var actual = await handler.Handle(new GetVendorMenuDetailsQuery { Location = Location, VendorId = vendorId }
            , It.IsAny<CancellationToken>());

            actual.Should().BeNull();
        }
    }
}
