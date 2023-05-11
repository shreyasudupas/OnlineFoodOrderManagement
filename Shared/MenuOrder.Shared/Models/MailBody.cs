using MenuOrder.Shared.Enum;

namespace MenuOrder.Shared.Models
{
    public class MailBody
    {
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailTypeEnum TemplateType { get; set; }
    }
}
