using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Opis.Queries.Buscar.Common;
using Application.Features.Opis.Vm;
using Application.Persistence;
using AutoMapper;
using Domain.Entities.Opis;
using Newtonsoft.Json;


namespace Application.Features.Opis.Queries.Buscar;

public class BuscarOpisHandler(IOpisDat opisDat, IMapper mapper, ILogs logs, ILogger<BuscarOpisHandler> logger) : IRequestHandler<ReqBuscarOpis, ResBuscarOpis>
{
    public async Task<ResBuscarOpis> Handle(ReqBuscarOpis request, CancellationToken cancellationToken)
    {
        var respuesta = new ResBuscarOpis();

        const string strOperacion = "GET_BUSCAR_OPIS";

        try
        {
            respuesta.LlenarResHeader(request);

            logger.LogInformation("GET_BUSCAR_OPIS.REQUEST: {request}", JsonConvert.SerializeObject(request));
            _ = logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);

            var respuestaTransaccion = await opisDat.BuscarOpis(request);
            
            if (respuestaTransaccion.codigo.Equals("000"))
            {
                var body = (ConjuntoDatos)respuestaTransaccion.cuerpo;
                var opis = Conversions.ConvertToList<BuscarOpis>( body ).ToList();
                respuesta.lst_opis = opis;
                respuesta.str_nombre_reporte = $"Reporte_{request.str_tipo_transf}_{DateTime.Now:dd/MM/yyyy}";

                switch (request.str_tipo_transf)
                {
                    case "-1":
                    case "spi":
                    case "banred":
                        var modelTransferencias = mapper.Map<IReadOnlyList<TransferenciaReporteVm>>( opis );
                        respuesta.str_reporte_base64 = ReporteFile.GenerarReporteTransferencias( request, modelTransferencias );
                        break;
                    case "proveedores":
                        var modelProveedores = mapper.Map<IReadOnlyList<ProveedorReporteVm>>( opis );
                        respuesta.str_reporte_base64 = ReporteFile.GenerarReporteProveedores( request, modelProveedores );
                        break;
                    default:
                        respuesta.str_reporte_base64 = null;
                        break;
                }
            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["str_error"];
            
            logger.LogInformation("GET_BUSCAR_OPIS.RESPONSE: {respuesta}", JsonConvert.SerializeObject(respuesta));
            _ = logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
            
        }
        catch (Exception e)
        {
            logger.LogError("GET_BUSCAR_OPIS.EXCEPTION: {e}", e);
            _ = logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, e);

            throw new ArgumentException(respuesta.str_id_transaccion);
        }

        return respuesta;
    }
}