using MediatR;
using NArtadoSearch.Entities.Concrete;
using NArtadoSearch.Entities.Models;

namespace NArtadoSearch.Business.Queries;

public class SearchWebQuery : IRequest<WebSearchResults>
{
    public string SearchText { get; set; } = string.Empty;
}