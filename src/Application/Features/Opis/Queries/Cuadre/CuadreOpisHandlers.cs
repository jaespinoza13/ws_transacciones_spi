using System.Reflection;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Opis.Queries.Cuadre.Common;
using Application.Persistence;
using Domain.Entities.Opis;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Features.Opis.Queries.Cuadre;

public class CuadreOpisHandlers : IRequestHandler<ReqCuadreOpis, ResCuadreOpis>
{
    private readonly IOpisDat _opisDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<CuadreOpisHandlers> _logger;
    private readonly ApiConfig _apiConfig;
    
    public CuadreOpisHandlers(IOpisDat opisDat, ILogs logs, ILogger<CuadreOpisHandlers> logger, IOptionsMonitor<ApiConfig> apiConfig)
    {
        _opisDat = opisDat;
        _logs = logs;
        _clase = GetType().Name;
        _logger = logger;
        _apiConfig = apiConfig.CurrentValue;
    }
    
    public async Task<ResCuadreOpis> Handle(ReqCuadreOpis request, CancellationToken cancellationToken)
    {
        var respuesta = new ResCuadreOpis();

        const string strOperacion = "GET_CUADRE_OPIS";

        try
        {
            respuesta.LlenarResHeader(request);

            _ = _logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);

            var respuestaTransaccion = await _opisDat.CuadreOpis(request);
            
            if (respuestaTransaccion.codigo.Equals("000"))
            {
                var body = (ConjuntoDatos)respuestaTransaccion.cuerpo;
                var cuadreOpis = Conversions.ConvertToList<OrdenPago>( body ).ToList();
                respuesta.file_bytes = CuadreOpis.GenerarCuadreOpis(request, cuadreOpis, _apiConfig );
                respuesta.str_doc_extencion = "application/pdf";
               
            }
            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["str_error"];
            _ = _logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);
        }
        catch (Exception e)
        {
            _ = _logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e);
            _logger.LogError(e, "Ocurrió un error en CuadreOpisHandlers");
            throw new ArgumentException(respuesta.str_id_transaccion);
        }

        return respuesta;
    }
}