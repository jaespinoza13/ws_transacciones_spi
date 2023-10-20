using System.Globalization;
using System.Reflection;
using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.SpiArchivo.Command.Spi1;
using Application.Persistence;
using Infrastructure.Common.Functions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.GrpcClients.Sybase.SpiArchivo;

public class SpiArchivoDat : ISpiArchivoDat
{
    private readonly ILogs _logService;
    private readonly DAL.DALClient _objClientDal;
    private readonly ApiConfig _apiConfig;
    private readonly string _clase;
    private readonly ILogger<SpiArchivoDat> _logger;
    
    public SpiArchivoDat(ILogger<SpiArchivoDat> logger, ILogs logService, DAL.DALClient objClientDal, IOptionsMonitor<ApiConfig> apiConfig)
    {
        _logger = logger;
        _logService = logService;
        _objClientDal = objClientDal;
        _apiConfig = apiConfig.CurrentValue;
        _clase = GetType().FullName!;
    }
    
    public async Task<RespuestaTransaccion> GenerarSpi1(ReqGenerarSpi1 request)
    {
        var respuesta = new RespuestaTransaccion();
        try
        {
            var ds = new DatosSolicitud();
            Funciones.LlenarDatosAuditoriaSalida( ds, request );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha_corte", TipoDato = TipoDato.DateTime, ObjValue = request.dtt_fecha_corte.ToString( CultureInfo.InvariantCulture ) } );

            ds.NombreSP = "set_generar_archivo_spi1";
            ds.NombreBD = _apiConfig.db_meg_bce;

            var resultado = await _objClientDal.ExecuteDataSetAsync( ds );
            var lstValores = resultado.ListaPSalidaValores.ToList();

            var strCodigo = lstValores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
            var strError = lstValores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();

            respuesta.codigo = strCodigo.Trim().PadLeft( 3, '0' );
            respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
            respuesta.diccionario.Add( "str_error", strError );
        }
        catch (Exception e)
        {
            respuesta.codigo = "001";
            respuesta.diccionario.Add( "str_error", e.InnerException?.Message ?? e.Message );
            _logger.LogError( e, "Ocurrió un error en stored procedure set_generar_archivo_spi1" );
            _ = _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, e, _clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }
}