﻿using System.Globalization;
using System.Reflection;
using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Opis.Queries.Buscar;
using Application.Persistence;
using Grpc.Net.Client;
using Infrastructure.Common.Funciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.gRPC_Clients.Sybase.Opis;

public class OpisDat : IOpisDat
{
    private readonly ILogs _logService;
    private readonly DAL.DALClient _objClienteDal;
    private readonly string _clase;
    private readonly ApiSettings _settings;
    private readonly ILogger<OpisDat> _logger;

    public OpisDat(IOptionsMonitor<ApiSettings> options, ILogs logService, ILogger<OpisDat> logger)
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
        var canal = GrpcChannel.ForAddress( _settings.client_grpc_sybase!, new GrpcChannelOptions { HttpHandler = handler } );
        _objClienteDal = new DAL.DALClient( canal );
    }

    public async Task<RespuestaTransaccion> BuscarOpis(ReqBuscarOpis request)
    {
        var respuesta = new RespuestaTransaccion();
        try
        {
            var ds = new DatosSolicitud();
            Funciones.LlenarDatosAuditoriaSalida( ds, request );
            ds.ListaPEntrada.Add( new ParametroEntrada{StrNameParameter = "@dtt_fecha_desde", TipoDato = TipoDato.DateTime, ObjValue = request.dtt_fecha_desde.ToString( CultureInfo.CurrentCulture )} );
            ds.ListaPEntrada.Add( new ParametroEntrada{StrNameParameter = "@dtt_fecha_hasta", TipoDato = TipoDato.DateTime, ObjValue = request.dtt_fecha_hasta.ToString( CultureInfo.CurrentCulture )} );
            ds.ListaPEntrada.Add( new ParametroEntrada{StrNameParameter = "@str_tipo_transf", TipoDato = TipoDato.VarChar, ObjValue = request.str_tipo_transf} );
            ds.ListaPEntrada.Add( new ParametroEntrada{StrNameParameter = "@str_cta_ordenante", TipoDato = TipoDato.VarChar, ObjValue = request.str_cta_ordenante} );
            ds.ListaPEntrada.Add( new ParametroEntrada{StrNameParameter = "@str_ident_ordenante", TipoDato = TipoDato.VarChar, ObjValue = request.str_ident_ordenante} );
            ds.ListaPEntrada.Add( new ParametroEntrada{StrNameParameter = "@str_cta_beneficiario", TipoDato = TipoDato.VarChar, ObjValue = request.str_cta_beneficiario} );
            ds.ListaPEntrada.Add( new ParametroEntrada{StrNameParameter = "@str_ident_beneficiario", TipoDato = TipoDato.VarChar, ObjValue = request.str_ident_beneficiario} );
            ds.ListaPEntrada.Add( new ParametroEntrada{StrNameParameter = "@int_codigo_opi", TipoDato = TipoDato.Integer, ObjValue = request.int_codigo_opi.ToString()});
            ds.ListaPEntrada.Add( new ParametroEntrada{StrNameParameter = "@str_ruc_proveedor", TipoDato = TipoDato.VarChar, ObjValue = request.str_ruc_proveedor} );
            ds.ListaPEntrada.Add( new ParametroEntrada{StrNameParameter = "@str_comprobante_venta", TipoDato = TipoDato.VarChar, ObjValue = request.str_comprobante_venta} );
            ds.ListaPEntrada.Add( new ParametroEntrada{StrNameParameter = "@str_comprobante_cont", TipoDato = TipoDato.VarChar, ObjValue = request.str_comprobante_cont} );
            
            ds.NombreSP = "get_opis_all_spi";
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
            _logger.LogError( e, "Error en stored procedure get_search_transferencias" );
            _ = _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, e, _clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }
}