using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities.SpiArchivo;


namespace Application.Features.SpiArchivo.Queries.Spi1;

public class TotalOpiSp1Handler: IRequestHandler<ReqTotalOpiSp1, ResTotalOpiSpi1>
{
    private readonly ISpiArchivoDat _spiArchivoDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<TotalOpiSp1Handler> _logger;

    public TotalOpiSp1Handler(ISpiArchivoDat archivoDat, ILogs logs, ILogger<TotalOpiSp1Handler> logger)
    {
        _spiArchivoDat = archivoDat;
        _logs = logs;
        _clase = GetType().Name;
        _logger = logger;
    }
    
    public async Task<ResTotalOpiSpi1> Handle(ReqTotalOpiSp1 request, CancellationToken cancellationToken)
    {
        var respuesta = new ResTotalOpiSpi1();
        
        const string strOperacion = "GET_TOTAL_OPI_SPI1";
        
        try
        {
            respuesta.LlenarResHeader(request);
            
            _ = _logs.SaveHeaderLogs( request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );

            var responseDat = await _spiArchivoDat.GetTotalOpiCorte( request );
            
            if (responseDat.codigo.Equals("000"))
            {
                var body = responseDat.cuerpo;
                var opis = Conversions.ConvertToList<TotalOpiCorte>((ConjuntoDatos)body).ToList();
                respuesta.lst_total_opi_corte = opis;
                respuesta.dec_total_monto = opis.Sum( x => Convert.ToDecimal( x.dec_total_monto ) );
                respuesta.int_total_opis = opis.Sum( x => Convert.ToInt32( x.int_total_opis ) );
            }

            respuesta.str_res_codigo = responseDat.codigo;
            respuesta.str_res_info_adicional = responseDat.diccionario["str_error"];
            _ = _logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);
        }
        catch (Exception e)
        {
            _ = _logs.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e );
            _logger.LogError( e, "Ocurrió un error en TotalOpiSp1Handler" );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }
        return respuesta;
    }
}