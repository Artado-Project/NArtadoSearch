namespace NArtadoSearch.Core.Utilities.EventBus.Abstractions;

public interface IEventBus
{
    void SendEvent<T>(T eventToSend) where T : class;
    Task SendEventAsync<T>(T eventToSend) where T : class;
    Task ConsumeAsync<T>(Action<T> onDataAvailable, CancellationToken cancellationToken) where T : class;
}