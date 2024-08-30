using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NArtadoSearch.Business.Commands;
using NArtadoSearch.Business.Services.Abstractions;
using NArtadoSearch.Core.Utilities.EventBus.Abstractions;
using NArtadoSearch.Entities.Concrete;
using RabbitMQ.Client;

namespace NArtadoSearch.Business.Services.Hosted;

public class BackgroundIndexerService(IServiceProvider serviceProvider) : IHostedService
{
    private IEventBus _eventBus;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using (var scope = serviceProvider.CreateAsyncScope())
        {
            _eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
            await _eventBus.ConsumeAsync<IndexWebUrlCommand>(async command =>
            {
                await using var innerScope = serviceProvider.CreateAsyncScope();
                var service = innerScope.ServiceProvider.GetRequiredService<IWebIndexService>();

                await service.AddAsync(command.Data);
            }, cancellationToken);
        }
    }

    private async Task SyncDatabase()
    {
        using var scope = serviceProvider.CreateScope();
        var webIndexService = scope.ServiceProvider.GetRequiredService<IWebIndexService>();
        await webIndexService.SynchronizeAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}