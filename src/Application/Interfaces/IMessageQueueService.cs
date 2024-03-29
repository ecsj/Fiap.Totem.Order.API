namespace Application.Interfaces;

public interface IMessageQueueService
{
    void PublishMessage(string queue, string message);
    Task ConsumeMessages(string queue, Func<string, Task> callback);
    void CloseConnection();
}