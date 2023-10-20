using System.Reflection;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities.Opis;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Opis.Queries.Buscar;

public class BuscarOpisHandler : IRequestHandler<ReqBuscarOpis, ResBuscarOpis>
{
    private readonly IOpisDat _opisDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<BuscarOpisHandler> _logger;

    public BuscarOpisHandler(IOpisDat opisDat, ILogs logs, ILogger<BuscarOpisHandler> logger)
    {
        _opisDat = opisDat;
        _logs = logs;
        _clase = GetType().Name;
        _logger = logger;
    }

    public async Task<ResBuscarOpis> Handle(ReqBuscarOpis request, CancellationToken cancellationToken)
    {
        var respuesta = new ResBuscarOpis();

        const string strOperacion = "GET_BUSCAR_OPIS";

        try
        {
            respuesta.LlenarResHeader(request);

            _ = _logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);

            var respuestaTransaccion = await _opisDat.BuscarOpis(request);

            if (respuestaTransaccion.codigo.Equals("000"))
            {
                var opis = Conversions.ConvertToList<BuscarOpis>((ConjuntoDatos)respuestaTransaccion.cuerpo);

                respuesta.lst_opis = (List<BuscarOpis>)opis;
            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["str_error"];
            _ = _logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);
        }
        catch (Exception e)
        {
            _ = _logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e);
            _logger.LogError(e, "Ocurrió un error en BuscarOpisHandler");
            throw new ArgumentException(respuesta.str_id_transaccion);
        }

        return respuesta;
    }
}