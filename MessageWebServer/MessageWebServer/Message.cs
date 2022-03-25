namespace MessageWebServer
{
    public class Message
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
    }
}
