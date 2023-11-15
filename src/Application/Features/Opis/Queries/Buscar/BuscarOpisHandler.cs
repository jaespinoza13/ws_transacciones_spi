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


namespace Application.Features.Opis.Queries.Buscar;

public class BuscarOpisHandler : IRequestHandler<ReqBuscarOpis, ResBuscarOpis>
{
    private readonly IOpisDat _opisDat;
    private readonly IMapper _mapper;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<BuscarOpisHandler> _logger;


    public BuscarOpisHandler(IOpisDat opisDat, IMapper mapper, ILogs logs, ILogger<BuscarOpisHandler> logger)
    {
        _opisDat = opisDat;
        _mapper = mapper;
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
                var body = (ConjuntoDatos)respuestaTransaccion.cuerpo;
                var opis = Conversions.ConvertToList<BuscarOpis>( body ).ToList();
                respuesta.lst_opis = opis;
                respuesta.str_nombre_reporte = $"Reporte_{request.str_tipo_transf}_{DateTime.Now:dd/MM/yyyy}";

                switch (request.str_tipo_transf)
                {
                    case "-1":
                    case "spi":
                    case "banred":
                        var modelTransferencias = _mapper.Map<IReadOnlyList<TransferenciaReporteVm>>( opis );
                        respuesta.str_reporte_base64 = ReporteFile.GenerarReporteTransferencias( request, modelTransferencias );
                        break;
                    case "proveedores":
                        var modelProveedores = _mapper.Map<IReadOnlyList<ProveedorReporteVm>>( opis );
                        respuesta.str_reporte_base64 = ReporteFile.GenerarReporteProveedores( request, modelProveedores );
                        break;
                    default:
                        respuesta.str_reporte_base64 = null;
                        break;
                }
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