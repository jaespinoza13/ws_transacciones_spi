using Application.Common.Models;
using Application.Features.Spi1.Command.GenerarSpi1;

namespace Application.Persistence;

public interface ISpi1Dat
{
    Task<RespuestaTransaccion> GenerarSpi1( ReqGenerarSpi1 request );
}