using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Common.ISO20022.Models;
using Application.Features.Catalogos.Queries.GetCatalogos;
using Application.Features.Catalogos.Queries.GetIfis;
using Domain.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WebApi.Filters;

namespace WebApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Rol.SocioInvitadoInterno)]
[ServiceFilter(typeof(CryptographyAesFilter))]
[ServiceFilter(typeof(ClaimControlFilter))]
[ServiceFilter(typeof(SessionControlFilter))]
[ProducesResponseType(typeof(ResBadRequestException), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(ResException), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ResException), (int)HttpStatusCode.InternalServerError)]
public class CatalogoController : BaseController
{
    private readonly IMediator _mediator;

    public CatalogoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost( "GET_CATALOGOS_SPI" )]
    [ProducesResponseType( typeof(ResGetCatalogo), StatusCodes.Status200OK )]
    public async Task<ResGetCatalogo> GetCatalogos(ReqGetCatalogo request) => await _mediator.Send( request );

    [HttpPost( "GET_IFIS_SPI" )]
    [ProducesResponseType( typeof(ResGetIfis), StatusCodes.Status200OK )]
    public async Task<ResGetIfis> GetIfisSpi(ReqGetIfis request) => await _mediator.Send( request );
}