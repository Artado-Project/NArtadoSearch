using MediatR;
using NArtadoSearch.Business.Queries;
using NArtadoSearch.Entities.Models;

namespace NArtadoSearch.Business.Handlers;

public class AutoCompleteQueryHandler : IRequestHandler<AutoCompleteQuery, AutoCompleteResults>
{
    public async Task<AutoCompleteResults> Handle(AutoCompleteQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}