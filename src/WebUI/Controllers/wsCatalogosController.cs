using System.Net;
using Microsoft.AspNetCore.Mvc;
using MediatR;

using Application.Common.ISO20022.Models;
using Application.Features.Catalogos.Queries.Consultar;

namespace WebUI.Controllers;

[Route("api/wsCatalogos")]
[Produces( "application/json" )]
[ApiController]
[ProducesResponseType( typeof(ResBadRequestException), (int)HttpStatusCode.BadRequest )]
[ProducesResponseType( typeof(ResException), (int)HttpStatusCode.Unauthorized )]
[ProducesResponseType( typeof(ResException), (int)HttpStatusCode.InternalServerError )]
public class WsCatalogosController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public WsCatalogosController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("GET_CATALOGOS")]
    [ProducesResponseType( typeof(ResGetCatalogos), (int)HttpStatusCode.OK )]
    public async Task<ResGetCatalogos> ConsultarCuentas(ReqGetCatalogos request) => await _mediator.Send( request );
    
    
}