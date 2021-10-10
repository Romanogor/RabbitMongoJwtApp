namespace RabbitMongoJwt.BL
{
    public interface IMessageService
    {
        void SendMessageToQueue(string userName);
    }
}