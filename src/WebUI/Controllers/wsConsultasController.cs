using System.Net;
using Microsoft.AspNetCore.Mvc;
using MediatR;

using Application.Common.ISO20022.Models;
using Application.Features.Consultas.Queries.Cuentas;
using Application.Features.Consultas.Queries.Entidades;

namespace WebUI.Controllers;

[Route("api/wsConsultas")]
[Produces( "application/json" )]
[ApiController]
[ProducesResponseType( typeof(ResBadRequestException), (int)HttpStatusCode.BadRequest )]
[ProducesResponseType( typeof(ResException), (int)HttpStatusCode.Unauthorized )]
[ProducesResponseType( typeof(ResException), (int)HttpStatusCode.InternalServerError )]
public class WsConsultasController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public WsConsultasController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("GET_CUENTAS_SOCIO")]
    [ProducesResponseType( typeof(ResBuscarCuentas), (int)HttpStatusCode.OK )]
    public async Task<ResBuscarCuentas> ConsultarCuentas(ReqBuscarCuentas request) => await _mediator.Send( request );
    
    [HttpPost("GET_ENTIDADES_FINANCIERAS")]
    [ProducesResponseType( typeof(ResEntidades), (int)HttpStatusCode.OK )]
    public async Task<ResEntidades> ConsultarEntidadesFinancieras(ReqEntidades request) => await _mediator.Send( request );
    
}