// @author Andrew Zmushko (andrewzmushko@gmail.com)

namespace Banks.Entities
{
    public class Message
    {
        public Message(string messageText)
        {
            MessageText = messageText;
        }

        public string MessageText
        {
            get;
        }
    }
}