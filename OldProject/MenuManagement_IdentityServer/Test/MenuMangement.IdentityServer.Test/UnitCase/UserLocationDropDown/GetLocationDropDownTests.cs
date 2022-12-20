using AutoMapper;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Service;
using MenuMangement.IdentityServer.Test.Helper.FakeDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace MenuMangement.IdentityServer.Test.UnitCase.UserLocationDropDown
{
    public class GetLocationDropDownTests : BaseFakeClientDBContext
    {
        private void Setup()
        {
            appContext.Users.Add(new ApplicationUser
            {
                Id = "12sks",
                Address = new List<UserAddress>
                {
                    new UserAddress { Id = 1, City="Bangalore",State= "Karnataka",IsActive = true},
                    new UserAddress { Id = 2, City="Bangalore",State= "Karnataka",IsActive = false}
                }
            });

            appContext.States.Add(new State
            {
                Name = "Karnataka",
                Cities = new List<City>
                {
                    new City
                    {
                        Name="Bengaluru",
                        Areas = new List<LocationArea>
                        {
                            new LocationArea { AreaName="Kathreguppe" },
                            new LocationArea { AreaName="JP Nagar" },
                            new LocationArea { AreaName="Jayanagar" },
                            new LocationArea { AreaName="Uttrahalli" },
                            new LocationArea { AreaName="Banashankari 2nd Stage" }
                        }
                    }
                }
            });

            appContext.SaveChanges();
        }

        [Fact]
        public void GetLocationDropDownForUi_Success()
        {
            Setup();

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();
            var mockMapper = new Mock<IMapper>();

            var sut = new UserAdministrationManager(mockUserManager, mockRoleManager, appContext, mockMapper.Object, mockWebHostEnv.Object,
                logMock.Object);

            var actual = sut.GetLocationDropDownForUi("12sks");

            Assert.NotNull(actual);
            Assert.Equal(actual[0].Label, "Bengaluru");
        }
    }
}
