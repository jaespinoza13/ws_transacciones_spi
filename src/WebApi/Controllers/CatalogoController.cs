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
public class CatalogoController(IMediator mediator) : BaseController
{

    [HttpPost( "GET_CATALOGOS_SPI" )]
    [ProducesResponseType( typeof(ResGetCatalogo), StatusCodes.Status200OK )]
    public async Task<ResGetCatalogo> GetCatalogos(ReqGetCatalogo request) => await mediator.Send( request );

    [HttpPost( "GET_IFIS_SPI" )]
    [ProducesResponseType( typeof(ResGetIfis), StatusCodes.Status200OK )]
    public async Task<ResGetIfis> GetIfisSpi(ReqGetIfis request) => await mediator.Send( request );
}