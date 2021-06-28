using OrderAPI.BuisnessLayer.DBModels;
using System;
using System.Collections.Generic;

namespace MenuDatabase.Data.Database
{
    public class tblUser
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        //foriegn key
        public int RoleId { get; set; }
        public string FullName { get; set; }

        public string? Address { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public string PictureLocation { get; set; }
        public long Points { get; set; }
        public double CartAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        

        public tblRole tblRole { get; set; }
        public ICollection<tblUserOrder> tblUserOrders { get; set; }
        public tblCity City { get; set; }
        public tblState State { get; set; }
    }
}