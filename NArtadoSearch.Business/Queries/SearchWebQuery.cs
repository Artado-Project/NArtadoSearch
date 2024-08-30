using MediatR;
using NArtadoSearch.Entities.Concrete;
using NArtadoSearch.Entities.Models;

namespace NArtadoSearch.Business.Queries;

public class SearchWebQuery : IRequest<WebSearchResults>
{
    public string SearchText { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}