using System.Net.Http.Json;
using NArtadoSearch.Entities.Models;
using NArtadoSearch.WebApplication.Services.Abstractions;

namespace NArtadoSearch.WebApplication.Services;

public class SearchService(HttpClient client) : ISearchService
{
    public async Task<WebSearchResults> SearchAsync(string query, int page = 1, int pageSize = 20)
    {
        var response = await client.GetFromJsonAsync<WebSearchResults>($"v1/search/web?query={query}&page={page}&pageSize={pageSize}");

        if (response == null)
        {
            throw new Exception("Unable to connect to Artado servers. Please try again later.");
        }
        
        return response;
    }
}