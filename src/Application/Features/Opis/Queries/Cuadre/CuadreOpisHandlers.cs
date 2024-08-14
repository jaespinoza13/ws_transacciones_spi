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
using Newtonsoft.Json;

namespace Application.Features.Opis.Queries.Cuadre;

public class CuadreOpisHandlers(IOpisDat opisDat, ILogs logs, ILogger<CuadreOpisHandlers> logger, IOptionsMonitor<ApiConfig> apiConfig) : IRequestHandler<ReqCuadreOpis, ResCuadreOpis>
{
    private readonly ApiConfig _apiConfig = apiConfig.CurrentValue;

    public async Task<ResCuadreOpis> Handle(ReqCuadreOpis request, CancellationToken cancellationToken)
    {
        var respuesta = new ResCuadreOpis();

        const string strOperacion = "GET_CUADRE_OPIS";

        try
        {
            respuesta.LlenarResHeader( request );

            logger.LogInformation("GET_CUADRE_OPIS.REQUEST: {request}", JsonConvert.SerializeObject(request));
            _ = logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);


            var respuestaTransaccion = await opisDat.CuadreOpis( request );

            if (respuestaTransaccion.codigo.Equals( "000" ))
            {
                var body = (ConjuntoDatos)respuestaTransaccion.cuerpo;
                var cuadreOpis = Conversions.ConvertToList<OrdenPago>( body ).ToList();
                respuesta.file_bytes = CuadreOpis.GenerarCuadreOpis( request, cuadreOpis, _apiConfig );
                respuesta.str_doc_extencion = "application/pdf";

            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["str_error"];
            
            logger.LogInformation("GET_CUADRE_OPIS.RESPONSE: {respuesta}", JsonConvert.SerializeObject(respuesta));
            _ = logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
            
        }
        catch (Exception e)
        {
            respuesta.str_res_codigo = "003";
            respuesta.str_res_info_adicional = e.Message;
            
            logger.LogError("GET_CUADRE_OPIS.EXCEPTION: {e}", e);
            _ = logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, e);

            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }
}