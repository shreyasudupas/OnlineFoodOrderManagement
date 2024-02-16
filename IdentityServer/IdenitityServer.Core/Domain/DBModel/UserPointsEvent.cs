using System;
using System.ComponentModel.DataAnnotations;

namespace IdenitityServer.Core.Domain.DBModel
{
    public class UserPointsEvent
    {
        [Key]
        public long EventId { get; set; }

        [StringLength(maximumLength: 40, ErrorMessage = "UserId is more than 30 character")]
        public string UserId { get; set; }

        public double PointsInHand { get; set; }
        public double PointsAdjusted { get; set; }

        [StringLength(maximumLength:40,ErrorMessage = "EventOperation name length is more than 40 character")]
        public string EventOperation { get; set; }

        [StringLength(maximumLength: 40, ErrorMessage = "EventAdjustedUserId is more than 30 character")]
        public string AddOrAdjustedUserId { get; set; }
        public DateTime EventCreatedDate { get; set; }
    }
}
