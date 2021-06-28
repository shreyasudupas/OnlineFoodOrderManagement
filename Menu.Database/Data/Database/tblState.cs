using MenuDatabase.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.BuisnessLayer.DBModels
{
    public class tblState
    {
        public int StateId { get; set; }
        public string StateNames { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<tblCity> Cities { get; set; }
        public ICollection<tblUser> Users { get; set; }
    }
}
