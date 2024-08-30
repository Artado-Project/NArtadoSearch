using MediatR;
using Microsoft.AspNetCore.Mvc;
using NArtadoSearch.Business.Queries;

namespace NArtadoSearch.WebAPI.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class SearchController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("web")]
    public async Task<IActionResult> Web([FromQuery] string q)
    {
        return Ok(await _mediator.Send(new SearchWebQuery() { SearchText = q }));
    }
}