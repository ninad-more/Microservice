namespace Restaurant.MessageBus
{
    public class BaseMessage
    {
        public int Id { get; set; }
        public DateTime MsgTime { get; set; }
    }
}
