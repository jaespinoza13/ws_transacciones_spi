using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.ISO20022.Models;
using Application.Features.SpiArchivo.Command.Spi1;
using Domain.Types;
using WebApi.Filters;

namespace WebApi.Controllers;

// [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Rol.SocioInvitadoInterno )]
// [ServiceFilter( typeof(CryptographyAesFilter) )]
// [ServiceFilter( typeof(ClaimControlFilter) )]
// [ServiceFilter( typeof(SessionControlFilter) )]
[ProducesResponseType( typeof(ResBadRequestException), (int)HttpStatusCode.BadRequest )]
[ProducesResponseType( typeof(ResException), (int)HttpStatusCode.Unauthorized )]
[ProducesResponseType( typeof(ResException), (int)HttpStatusCode.InternalServerError )]
public class SpiArchivo : BaseController
{
    private readonly IMediator _mediator;

    public SpiArchivo(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost( "SET_GENERAR_SPI1" )]
    [ProducesResponseType( typeof(ResGenerarSpi1), (int)HttpStatusCode.OK )]
    public async Task<ResGenerarSpi1> SetGenerarSpi1(ReqGenerarSpi1 request) => await _mediator.Send( request );
}