using Application.Common.Models;
using Application.Features.SpiArchivo.Command.Spi1;

namespace Application.Persistence;

public interface ISpiArchivoDat
{
    Task<RespuestaTransaccion> GenerarSpi1(ReqGenerarSpi1 request);
}