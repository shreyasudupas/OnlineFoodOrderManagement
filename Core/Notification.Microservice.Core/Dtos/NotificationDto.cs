using System;
using System.Collections.Generic;

namespace MenuManagment.Microservice.Core.Dtos
{
    public class NotificationDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string FromUserId { get; set; }
        public string ToUserId { get; set; }

        public string Role { get; set; }

        public DateTime RecordedTimeStamp { get; set; }

        public string Link { get; set; }

        public bool SendAll { get; set; }

        public bool Read { get; set; }

    }
}
