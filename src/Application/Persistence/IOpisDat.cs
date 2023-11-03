using Application.Common.Models;
using Application.Features.Opis.Queries.Buscar;
using Application.Features.Opis.Queries.Cuadre;
using Application.Features.Opis.Queries.Detalle;
using Application.Features.Opis.Queries.Imprimir.OrdenPago;
using Application.Features.Opis.Queries.Imprimir.Transferencias;

namespace Application.Persistence;

public interface IOpisDat
{
    Task<RespuestaTransaccion> BuscarOpis(ReqBuscarOpis request);
    Task<RespuestaTransaccion> DetalleOpi(ReqDetalleOpi request);
    Task<RespuestaTransaccion> ImprimirOrden(ReqImprimirOrdenPago request);
    Task<RespuestaTransaccion> ImprimirTransferencia(ReqImprimirTransferencia request);
    Task<RespuestaTransaccion> CuadreOpis(ReqCuadreOpis request);
}