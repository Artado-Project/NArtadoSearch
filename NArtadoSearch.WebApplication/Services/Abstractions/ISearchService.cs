using NArtadoSearch.Entities.Models;

namespace NArtadoSearch.WebApplication.Services.Abstractions;

public interface ISearchService
{
    Task<WebSearchResults> SearchAsync(string query, int page, int pageSize);
}