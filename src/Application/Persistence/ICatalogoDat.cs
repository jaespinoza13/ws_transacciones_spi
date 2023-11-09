using Application.Common.Models;
using Application.Features.Catalogos.Queries.GetCatalogos;
using Application.Features.Catalogos.Queries.GetIfis;

namespace Application.Persistence;

public interface ICatalogoDat
{
    Task<RespuestaTransaccion> GetCatalogos(ReqGetCatalogo request);
    Task<RespuestaTransaccion> GetIfis(ReqGetIfis request);
}