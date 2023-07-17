using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MediatR;

using Application.Common.ISO20022.Models;
using Domain.Types;
using WebUI.Filters;

using Application.Features.Spi1.Command.GenerarSpi1;

namespace WebUI.Controllers;

[Route("api/wsSpi1")]
[Produces( "application/json" )]
[ApiController]
[Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Rol.SocioInvitadoInterno )]
[ServiceFilter( typeof( CryptographyAesFilter ) )]
[ServiceFilter( typeof( ClaimControlFilter ) )]
[ServiceFilter( typeof( SessionControlFilter ) )]
[ProducesResponseType( typeof(ResBadRequestException), (int)HttpStatusCode.BadRequest )]
[ProducesResponseType( typeof(ResException), (int)HttpStatusCode.Unauthorized )]
[ProducesResponseType( typeof(ResException), (int)HttpStatusCode.InternalServerError )]
public class WsSpi1Controller : ControllerBase
{
    private readonly IMediator _mediator;
    
    public WsSpi1Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("GET_ARCHIVO_SPI1")]
    public async Task<ResGenerarSpi1> ConsultaTransferenciasInternasIngresadas(ReqGenerarSpi1 request) => await _mediator.Send( request );
    
}