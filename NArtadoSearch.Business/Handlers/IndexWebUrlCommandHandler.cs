using MediatR;
using NArtadoSearch.Business.Commands;
using NArtadoSearch.Core.Utilities.EventBus.Abstractions;

namespace NArtadoSearch.Business.Handlers;

public class IndexWebUrlCommandHandler(IEventBus eventBus) : IRequestHandler<IndexWebUrlCommand>
{
    public async Task Handle(IndexWebUrlCommand request, CancellationToken cancellationToken)
    {
        await eventBus.SendEventAsync(request);
    }
}