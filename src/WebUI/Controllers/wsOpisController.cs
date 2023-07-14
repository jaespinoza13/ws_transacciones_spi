using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Application.Common.ISO20022.Models;
using Application.Features.Opis.Queries.Buscar;
using Application.Features.Opis.Queries.Detalle;


namespace WebUI.Controllers;

[Route( "api/wsOpis" )]
[Produces( "application/json" )]
[ApiController]
public class WsOpisController : ControllerBase
{
    private readonly IMediator _mediator;

    public WsOpisController(IMediator mediator) => _mediator = mediator;


    [HttpPost( "GET_BUSCAR_OPIS" )]
    [ProducesResponseType( typeof(ResBuscarOpis), (int)HttpStatusCode.OK )]
    [ProducesResponseType( typeof(ResBadRequestException), (int)HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof(ResException), (int)HttpStatusCode.Unauthorized )]
    [ProducesResponseType( typeof(ResException), (int)HttpStatusCode.InternalServerError )]
    public async Task<ResBuscarOpis> BuscarOpis(ReqBuscarOpis request) => await _mediator.Send( request );

    [HttpPost( "GET_DETALLE_OPI" )]
    [ProducesResponseType( typeof(ResDetalleOpi), (int)HttpStatusCode.OK )]
    [ProducesResponseType( typeof(ResBadRequestException), (int)HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof(ResException), (int)HttpStatusCode.Unauthorized )]
    [ProducesResponseType( typeof(ResException), (int)HttpStatusCode.InternalServerError )]
    public async Task<ResDetalleOpi> DetalleOpi(ReqDetalleOpi request) => await _mediator.Send( request );
}