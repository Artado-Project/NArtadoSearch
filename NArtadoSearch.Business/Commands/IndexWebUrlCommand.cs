using MediatR;
using NArtadoSearch.Entities.Dto;

namespace NArtadoSearch.Business.Commands;

public class IndexWebUrlCommand : IRequest
{
    public IndexWebUrlDto Data { get; set; }
}