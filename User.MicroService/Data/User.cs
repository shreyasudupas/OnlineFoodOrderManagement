using System;

namespace Identity.MicroService.Data
{
    public class User : IEntityBase
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        //foriegn key
        public long UserRoleId { get; set; }
        public string FullName { get; set; }

        public string PictureLocation { get; set; }
        public int Points { get; set; }
        public decimal CartAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public UserRole UserRoleEntityFK { get; set; }
    }
}
