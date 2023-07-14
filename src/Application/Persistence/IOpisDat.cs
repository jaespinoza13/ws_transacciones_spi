using Application.Common.Models;
using Application.Features.Opis.Queries.Buscar;
using Application.Features.Opis.Queries.Detalle;

namespace Application.Persistence;

public interface IOpisDat
{
    Task<RespuestaTransaccion> BuscarOpis(ReqBuscarOpis request);
    Task<RespuestaTransaccion> DetalleOpi(ReqDetalleOpi request);
}