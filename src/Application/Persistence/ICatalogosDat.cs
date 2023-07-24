using Application.Common.Models;
using Application.Features.Catalogos.Queries.Consultar;

namespace Application.Persistence;

public interface ICatalogosDat
{
    Task<RespuestaTransaccion> GetCatalogos(ReqGetCatalogos request);
}