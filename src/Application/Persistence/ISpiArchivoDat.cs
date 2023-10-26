using Application.Common.Models;
using Application.Features.SpiArchivo.Command.Spi1;
using Application.Features.SpiArchivo.Queries.Spi1;

namespace Application.Persistence;

public interface ISpiArchivoDat
{
    Task<RespuestaTransaccion> GetTotalOpiCorte(ReqTotalOpiSp1 request);
    Task<RespuestaTransaccion> GenerarSpi1(ReqGenerarSpi1 request);
}