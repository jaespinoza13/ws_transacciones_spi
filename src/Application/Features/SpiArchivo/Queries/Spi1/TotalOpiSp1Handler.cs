using System.Globalization;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities.SpiArchivo;
using Newtonsoft.Json;


namespace Application.Features.SpiArchivo.Queries.Spi1;

public class TotalOpiSp1Handler(ISpiArchivoDat archivoDat, ILogs logs, ILogger<TotalOpiSp1Handler> logger) : IRequestHandler<ReqTotalOpiSp1, ResTotalOpiSpi1>
{

    public async Task<ResTotalOpiSpi1> Handle(ReqTotalOpiSp1 request, CancellationToken cancellationToken)
    {
        var respuesta = new ResTotalOpiSpi1();

        const string strOperacion = "GET_TOTAL_OPI_SPI1";

        try
        {
            respuesta.LlenarResHeader( request );

            logger.LogInformation( "GET_TOTAL_OPI_SPI1.REQUEST: {request}", JsonConvert.SerializeObject( request ) );
            _ = logs.SaveHeaderLogs( request, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name );

            var responseDat = await archivoDat.GetTotalOpiCorte( request );

            if (responseDat.codigo.Equals( "000" ))
            {
                var body = responseDat.cuerpo;
                var opis = Conversions.ConvertToList<TotalOpiCorte>( (ConjuntoDatos)body ).ToList();
                respuesta.lst_total_opi_corte = opis;
                respuesta.dec_total_monto = opis.Sum( x => Convert.ToDecimal( x.dec_total_monto ) ).ToString( CultureInfo.InvariantCulture );
                respuesta.int_total_opis = opis.Sum( x => Convert.ToInt32( x.int_total_opis ) );
            }

            respuesta.str_res_codigo = responseDat.codigo;
            respuesta.str_res_info_adicional = responseDat.diccionario["str_error"];

            logger.LogInformation( "GET_TOTAL_OPI_SPI1.RESPONSE: {respuesta}", JsonConvert.SerializeObject( respuesta ) );
            _ = logs.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name );

        }
        catch (Exception e)
        {
            respuesta.str_res_codigo = "003";
            respuesta.str_res_info_adicional = e.Message;

            logger.LogError( "GET_TOTAL_OPI_SPI1.EXCEPTION: {e}", e );
            _ = logs.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, e );

            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }
}