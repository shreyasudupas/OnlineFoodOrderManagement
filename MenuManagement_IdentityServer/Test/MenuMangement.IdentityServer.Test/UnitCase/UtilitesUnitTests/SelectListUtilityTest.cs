using MenuManagement_IdentityServer.Utilities.DropdownItems;
using Xunit;

namespace MenuMangement.IdentityServer.Test.UnitCase.UtilitesUnitTests
{
    public class SelectListUtilityTest
    {
        [Fact]
        public void Test_GetCityItems_Function_ShouldReturn_ListOfCities()
        {
            //Arrange 
            var Cities = SelectListUtility.GetCityItems();

            //Assert
            Assert.True(Cities.Count > 0);
        }

        [Fact]
        public void Test_GetStateItems_Function_ShouldReturn_ListOfStates()
        {
            //Arrange 
            var States = SelectListUtility.GetStateItems();

            //Assert
            Assert.True(States.Count>0);
        }
    }
}
