using System.Reflection;
using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Catalogos.Queries.Consultar;
using Application.Persistence;
using Grpc.Net.Client;
using Infrastructure.Common.Funciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.gRPC_Clients.Sybase.Catalogos;

public class CatalogosDat : ICatalogosDat
{
    private readonly ILogs _logService;
    private readonly DAL.DALClient _objClienteDal;
    private readonly string _clase;
    private readonly ApiSettings _settings;
    private readonly ILogger<CatalogosDat> _logger;

    public CatalogosDat(IOptionsMonitor<ApiSettings> options, ILogs logService, ILogger<CatalogosDat> logger)
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


    public async Task<RespuestaTransaccion> GetCatalogos(ReqGetCatalogos request)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();
            Funciones.LlenarDatosAuditoriaSalida( ds, request );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_nombre_catalogo", TipoDato = TipoDato.VarChar, ObjValue = request.str_nombre_catalogo } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_filtro", TipoDato = TipoDato.VarChar, ObjValue = request.str_filtro } );
            ds.NombreSP = "get_catalogos_spi";
            ds.NombreBD = _settings.DB_meg_bce;

            var resultado = await _objClienteDal.ExecuteDataSetAsync( ds );
            var lstValores = resultado.ListaPSalidaValores.ToList();

            var strCodigo = lstValores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
            var strError = lstValores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();

            respuesta.codigo = strCodigo.Trim().PadLeft( 3, '0' );
            respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
            respuesta.diccionario.Add( "Error", strError );
            
        }
        catch (Exception e)
        {
            respuesta.codigo = "001";
            respuesta.diccionario.Add( "Error", e.InnerException?.Message ?? e.Message );
            _logger.LogError( e, "Error en stored procedure get_catalogos_spi" );
            _ = _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, e, _clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }
}