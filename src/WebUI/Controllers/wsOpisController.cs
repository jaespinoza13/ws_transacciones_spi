using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MediatR;

using WebUI.Filters;
using Domain.Types;
using Application.Common.ISO20022.Models;
using Application.Features.Opis.Queries.Buscar;
using Application.Features.Opis.Queries.Detalle;
using Application.Features.Opis.Queries.Imprimir.OrdenPago;
using Application.Features.Opis.Queries.Imprimir.Transferencias;


namespace WebUI.Controllers;

[Route( "api/wsOpis" )]
[Produces( "application/json" )]
[ApiController]
//[Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Rol.SocioInvitadoInterno )]
//[ServiceFilter( typeof( CryptographyAesFilter ) )]
//[ServiceFilter( typeof( ClaimControlFilter ) )]
//[ServiceFilter( typeof( SessionControlFilter ) )]
[ProducesResponseType( typeof(ResBadRequestException), (int)HttpStatusCode.BadRequest )]
[ProducesResponseType( typeof(ResException), (int)HttpStatusCode.Unauthorized )]
[ProducesResponseType( typeof(ResException), (int)HttpStatusCode.InternalServerError )]
public class WsOpisController : ControllerBase
{
    private readonly IMediator _mediator;

    public WsOpisController(IMediator mediator) => _mediator = mediator;


    [HttpPost( "GET_BUSCAR_OPIS" )]
    [ProducesResponseType( typeof(ResBuscarOpis), (int)HttpStatusCode.OK )]

    public async Task<ResBuscarOpis> BuscarOpis(ReqBuscarOpis request) => await _mediator.Send( request );

    [HttpPost( "GET_DETALLE_OPI" )]
    [ProducesResponseType( typeof(ResDetalleOpi), (int)HttpStatusCode.OK )]
    public async Task<ResDetalleOpi> DetalleOpi(ReqDetalleOpi request) => await _mediator.Send( request );

    [HttpPost( "GET_IMPRIMIR_ORDEN" )]
    [ProducesResponseType( typeof(ResImprimirOrdenPago), (int)HttpStatusCode.OK )]
    public async Task<ResImprimirOrdenPago> ImprimirOrden(ReqImprimirOrdenPago request) => await _mediator.Send( request );

    [HttpPost( "GET_IMPRIMIR_TRANSFERENCIA" )]
    [ProducesResponseType( typeof(ResImprimirTransferencia), (int)HttpStatusCode.OK )]
    public async Task<ResImprimirTransferencia> ImprimirTransferencia(ReqImprimirTransferencia request) => await _mediator.Send( request );
}