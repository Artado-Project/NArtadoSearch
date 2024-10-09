using MediatR;
using Microsoft.AspNetCore.Mvc;
using NArtadoSearch.Business.Queries;

namespace NArtadoSearch.WebAPI.Controllers;

[ApiController]
[Route("v1/search")]
public class SearchController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("web")]
    public async Task<IActionResult> Web([FromQuery] string query, [FromQuery] int page = 1,
        [FromQuery] int perPage = 20)
    {
        return Ok(
            await _mediator.Send(new SearchWebQuery
            {
                SearchText = query,
                Page = page,
                PageSize = perPage
            })
        );
    }

    [HttpGet("auto-complete")]
    public async Task<IActionResult> AutoComplete([FromQuery] string query)
    {
        return Ok(
            await _mediator.Send(new { })
        );
    }
}