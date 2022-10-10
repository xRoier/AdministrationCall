namespace AdministrationCall.Interfaces
{
    public class Message
    {
        public Message(string content)
        {
            this.content = content;
        }
        
        public string content { get; }
    }
}