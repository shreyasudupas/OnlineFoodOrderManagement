using MenuDatabase.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.BuisnessLayer.DBModels
{
    public class tblCity
    {
        public int CityId { get; set; }
        public string CityNames { get; set; }
        public int StateId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public tblState State { get; set; }
        public ICollection<tblUser> Users { get; set; }
    }
}
