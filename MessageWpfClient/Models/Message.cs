using System;

namespace MessageWpfClient.Models
{
    internal class Message
    {        
        public string MessageId { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
    }
}
