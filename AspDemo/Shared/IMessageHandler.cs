namespace Shared
{
    public interface IRevocationMessageHandler
    {
        void HandleMessage(dynamic message);
    }
}