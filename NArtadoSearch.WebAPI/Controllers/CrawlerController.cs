using MediatR;
using Microsoft.AspNetCore.Mvc;
using NArtadoSearch.Business.Commands;
using NArtadoSearch.Entities.Dto;

namespace NArtadoSearch.WebAPI.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class CrawlerController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("add/web")]
    public async Task<IActionResult> AddIndex([FromBody] IndexWebUrlDto dto)
    {
        await _mediator.Send(new IndexWebUrlCommand() { Data = dto });
        return Ok(new
        {
            Success = true,
            Message = "Index request added to queue. Please don't send same request again."
        });
    }
}