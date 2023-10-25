using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities.Opis;

namespace Application.Features.Opis.Queries.Detalle;

public class DetalleOpiHandler : IRequestHandler<ReqDetalleOpi, ResDetalleOpi>
{
    private readonly IOpisDat _opisDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<DetalleOpiHandler> _logger;

    public DetalleOpiHandler(IOpisDat opisDat, ILogs logs, ILogger<DetalleOpiHandler> logger)
    {
        _opisDat = opisDat;
        _logs = logs;
        _clase = GetType().Name;
        _logger = logger;
    }

    public async Task<ResDetalleOpi> Handle(ReqDetalleOpi request, CancellationToken cancellationToken)
    {
        var respuesta = new ResDetalleOpi();

        const string strOperacion = "GET_DETALLE_OPI";
        try
        {
            respuesta.LlenarResHeader(request);

            _ = _logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);

            var respuestaTransaccion = await _opisDat.DetalleOpi(request);

            if (respuestaTransaccion.codigo.Equals("000"))
            {
                var detalleOpi = Conversions.ConvertToClass<DetalleOpi>((ConjuntoDatos)respuestaTransaccion.cuerpo);

                if (detalleOpi.str_tipo_ordenante.Equals("CLIENTE"))
                {
                    var condiciones = Conversions.ConvertToList<FirmanteCuenta>((ConjuntoDatos)respuestaTransaccion.cuerpo, 1);
                    respuesta.lst_condiciones = (List<FirmanteCuenta>)condiciones;
                }

                respuesta.detalle_opi = detalleOpi;
            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["str_error"];
            _ = _logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);
        }
        catch (Exception e)
        {
            _ = _logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e);
            _logger.LogError(e, "Ocurrió un error en DetalleOpiHandler");
            throw new ArgumentException(respuesta.str_id_transaccion);
        }

        return respuesta;
    }
}