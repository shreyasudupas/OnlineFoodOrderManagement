using MenuManagement.Core.Common.Models.BasketService;
using MenuManagement.Core.Common.Models.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace MenuManagement.Core.Services.BasketService.Common
{
    public static class BasketCommon
    {
        public static void CreateCartMenuObject(JObject CartInfoFromUI, out string cartMenuId, out Dictionary<string, object> menuObject, out VendorDetail vendorDetail)
        {
            //Get the menu id from the selected object of the user
            cartMenuId = (string)CartInfoFromUI["Data"]["id"];
            //convert the item selected by user to object
            var cartObject = CartInfoFromUI["Data"].ToObject<Dictionary<string, object>>();
            menuObject = new Dictionary<string, object>();
            var ColumnsList = (from columns in CartInfoFromUI["ColumnData"]
                               select columns).ToList();

            //prepare the object with column names and add the values of user selected item and add = to add quantity as well which is hardcoded in frontend
            for (int i = 0; i <= ColumnsList.Count(); i++)
            {
                menuObject[cartObject.Keys.ElementAt(i).ToString()] = cartObject.Values.ElementAt(i);
            }
            //for vendor object
            //menuObject[cartObject.Keys.ElementAt(ColumnsList.Count()).ToString()] = cartObject.Values.ElementAt(ColumnsList.Count());
            vendorDetail = CartInfoFromUI["vendor details"].ToObject<VendorDetail>();
        }

        public static void UpdateUserCacheCartQuantity(Dictionary<string, object> menuObject, UserCartInformation UserInfoInCache, JObject ItemExistsInCache)
        {
            UserInfoInCache.Items.Remove(ItemExistsInCache);

            JObject newCartItem = JObject.Parse(JsonConvert.SerializeObject(menuObject));
            //add item in Item back with updated value
            UserInfoInCache.Items.Add(newCartItem);
        }
    }
}
