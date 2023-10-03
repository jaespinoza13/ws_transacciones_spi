using System.Net;
using Application.Common.ISO20022.Models;
using Application.Features.Parametros.Queries.GetParametros;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

//[Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Rol.SocioInvitadoInterno )]
//[ServiceFilter( typeof( CryptographyAesFilter ) )]
//[ServiceFilter( typeof( ClaimControlFilter ) )]
//[ServiceFilter( typeof( SessionControlFilter ) )]
[ProducesResponseType(typeof(ResBadRequestException), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(ResException), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ResException), (int)HttpStatusCode.InternalServerError)]
public class ParametrosController : BaseController
{
    private readonly IMediator _mediator;

    public ParametrosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("GET_PARAMETROS")]
    [ProducesResponseType(typeof(ResGetParametros), StatusCodes.Status200OK)]
    public async Task<ResGetParametros> GetParametros(ReqGetParametros request) => await _mediator.Send(request);
}