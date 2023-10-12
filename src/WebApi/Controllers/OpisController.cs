﻿using System.Net;
using Application.Common.ISO20022.Models;
using Application.Features.Opis.Queries.Buscar;
using Application.Features.Opis.Queries.Detalle;
using Application.Features.Opis.Queries.Imprimir.OrdenPago;
using Application.Features.Opis.Queries.Imprimir.Transferencias;
using Domain.Types;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers;

[Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Rol.SocioInvitadoInterno )]
[ServiceFilter( typeof( CryptographyAesFilter ) )]
[ServiceFilter( typeof( ClaimControlFilter ) )]
[ServiceFilter( typeof( SessionControlFilter ) )]
[ProducesResponseType(typeof(ResBadRequestException), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(ResException), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ResException), (int)HttpStatusCode.InternalServerError)]
public class OpisController : BaseController
{
    private readonly IMediator _mediator;

    public OpisController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("GET_BUSCAR_OPIS")]
    [ProducesResponseType(typeof(ResBuscarOpis), (int)HttpStatusCode.OK)]
    public async Task<ResBuscarOpis> BuscarOpis(ReqBuscarOpis request) => await _mediator.Send(request);

    [HttpPost("GET_DETALLE_OPI")]
    [ProducesResponseType(typeof(ResDetalleOpi), (int)HttpStatusCode.OK)]
    public async Task<ResDetalleOpi> DetalleOpi(ReqDetalleOpi request) => await _mediator.Send(request);

    [HttpPost("GET_IMPRIMIR_ORDEN")]
    [ProducesResponseType(typeof(ResImprimirOrdenPago), (int)HttpStatusCode.OK)]
    public async Task<ResImprimirOrdenPago> ImprimirOrden(ReqImprimirOrdenPago request) => await _mediator.Send(request);

    [HttpPost("GET_IMPRIMIR_TRANSFERENCIA")]
    [ProducesResponseType(typeof(ResImprimirTransferencia), (int)HttpStatusCode.OK)]
    public async Task<ResImprimirTransferencia> ImprimirTransferencia(ReqImprimirTransferencia request) => await _mediator.Send(request);
}