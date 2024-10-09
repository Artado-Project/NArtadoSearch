using MediatR;
using NArtadoSearch.Entities.Models;

namespace NArtadoSearch.Business.Queries;

public class AutoCompleteQuery : IRequest<AutoCompleteResults>
{
    public string Query { get; set; } = string.Empty;
}