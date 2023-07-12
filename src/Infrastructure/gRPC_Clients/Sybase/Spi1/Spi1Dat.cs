using System.Globalization;
using System.Reflection;
using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Spi1.Command.GenerarSpi1;
using Application.Persistence;
using Grpc.Net.Client;
using Infrastructure.Common.Funciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.gRPC_Clients.Sybase.Spi1;

public class Spi1Dat : ISpi1Dat
{
    private readonly ILogs _logService;
    private readonly DAL.DALClient _objClienteDal;
    private readonly string _clase;
    private readonly ApiSettings _settings;
    private readonly ILogger<Spi1Dat> _logger;

    public Spi1Dat(IOptionsMonitor<ApiSettings> options, ILogs logService, ILogger<Spi1Dat> logger)
    {
        _logService = logService;
        _settings = options.CurrentValue;
        _clase = GetType().FullName!;
        _logger = logger;

        var handler = new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
            KeepAlivePingDelay = TimeSpan.FromSeconds( _settings.delayOutGrpcSybase ),
            KeepAlivePingTimeout = TimeSpan.FromSeconds( _settings.timeoutGrpcSybase ),
            EnableMultipleHttp2Connections = true
        };
        var canal = GrpcChannel.ForAddress( _settings.client_grpc_sybase!,
            new GrpcChannelOptions { HttpHandler = handler } );
        _objClienteDal = new DAL.DALClient( canal );
    }

    public async Task<RespuestaTransaccion> GenerarSpi1(ReqGenerarSpi1 request)
    {
        var respuesta = new RespuestaTransaccion();
        try
        {
            var ds = new DatosSolicitud();
            Funciones.LlenarDatosAuditoriaSalida( ds, request );
            ds.ListaPEntrada.Add( new ParametroEntrada{StrNameParameter = "@dtt_fecha_corte", TipoDato = TipoDato.DateTime, ObjValue = request.dtt_fecha_corte.ToString( CultureInfo.CurrentCulture )} );
            
            ds.NombreSP = "set_generar_archivo_spi1";
            ds.NombreBD = _settings.DB_meg_bce;
            
            var resultado = await _objClienteDal.ExecuteDataSetAsync( ds );
            var lstValores = resultado.ListaPSalidaValores.ToList();

            var strCodigo = lstValores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
            var strError = lstValores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();

            respuesta.codigo = strCodigo.Trim().PadLeft( 3, '0' );
            respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
            respuesta.diccionario.Add( "str_error", strError );

        }
        catch (Exception e)
        {
            respuesta.codigo = "100";
            respuesta.diccionario.Add( "str_error", e.InnerException?.Message ?? e.Message );
            _logger.LogError( e, "Error en stored procedure set_generar_archivo_spi1" );
            _ = _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, e, _clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }
}