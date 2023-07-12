using Application.Common.Models;
using Application.Features.Opis.Queries.Buscar;

namespace Application.Persistence;

public interface IOpisDat
{
    Task<RespuestaTransaccion> BuscarOpis( ReqBuscarOpis request );
}