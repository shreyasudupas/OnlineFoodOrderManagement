using System;

namespace MenuManagment.Microservice.Core.Dtos
{
    public class NotificationDto
    {
        public string Id { get; set; }

        public string Priority { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string FromUserId { get; set; }
        public string ToUserId { get; set; }

        public string Role { get; set; }

        public NotificationDataDto Data { get; set; }

        public bool SendAll { get; set; }

        public bool Read { get; set; }

        public string CreatedDate { get; set; } = DateTime.Now.ToLocalTime().ToString();

    }

    public class NotificationDataDto
    {
        public string Uri { get; set; } = null;

        public string RequestType { get; set; } = null;

        public string Body { get; set; } = null;
    }
}
