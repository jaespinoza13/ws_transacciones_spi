using System.Globalization;
using System.Reflection;
using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Opis.Queries.Buscar;
using Application.Features.Opis.Queries.Cuadre;
using Application.Features.Opis.Queries.Detalle;
using Application.Features.Opis.Queries.Imprimir.OrdenPago;
using Application.Features.Opis.Queries.Imprimir.Transferencias;
using Application.Persistence;
using Infrastructure.Common.Functions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.GrpcClients.Sybase.Opis;

public class OpisDat: IOpisDat
{
    private readonly ILogs _logService;
    private readonly DAL.DALClient _objClientDal;
    private readonly ApiConfig _apiConfig;
    private readonly string _clase;
    private readonly ILogger<OpisDat> _logger;
    
    public OpisDat(ILogger<OpisDat> logger, ILogs logService, DAL.DALClient objClientDal, IOptions<ApiConfig> apiConfig)
    {
        _logger = logger;
        _logService = logService;
        _objClientDal = objClientDal;
        _apiConfig = apiConfig.Value;
        _clase = GetType().FullName!;
    }
    public async Task<RespuestaTransaccion> BuscarOpis(ReqBuscarOpis request)
    {
        var respuesta = new RespuestaTransaccion();
        try
        {
            var ds = new DatosSolicitud();
            Funciones.LlenarDatosAuditoriaSalida( ds, request );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha_desde", TipoDato = TipoDato.DateTime, ObjValue = request.dtt_fecha_desde.ToString( CultureInfo.CurrentCulture ) } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha_hasta", TipoDato = TipoDato.DateTime, ObjValue = request.dtt_fecha_hasta.ToString( CultureInfo.CurrentCulture ) } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_tipo_transf", TipoDato = TipoDato.VarChar, ObjValue = request.str_tipo_transf } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_cta_ordenante", TipoDato = TipoDato.VarChar, ObjValue = request.str_cta_ordenante } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_ident_ordenante", TipoDato = TipoDato.VarChar, ObjValue = request.str_ident_ordenante } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_cta_beneficiario", TipoDato = TipoDato.VarChar, ObjValue = request.str_cta_beneficiario } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_ident_beneficiario", TipoDato = TipoDato.VarChar, ObjValue = request.str_ident_beneficiario } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_codigo_opi", TipoDato = TipoDato.Integer, ObjValue = request.int_codigo_opi.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_usuario", TipoDato = TipoDato.VarChar, ObjValue = request.str_usuario } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_monto", TipoDato = TipoDato.Decimal, ObjValue = request.dec_monto.ToString( CultureInfo.InvariantCulture ) } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_nivel_opi", TipoDato = TipoDato.Integer, ObjValue = request.int_nivel_opi.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_estado_bce", TipoDato = TipoDato.Integer, ObjValue = request.int_estado_bce.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_estado_banred", TipoDato = TipoDato.Integer, ObjValue = request.int_estado_banred.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_banco_destino", TipoDato = TipoDato.VarChar, ObjValue = request.str_banco_destino } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_ruc_proveedor", TipoDato = TipoDato.VarChar, ObjValue = request.str_ruc_proveedor } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_comprobante_venta", TipoDato = TipoDato.VarChar, ObjValue = request.str_comprobante_venta } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_comprobante_cont", TipoDato = TipoDato.VarChar, ObjValue = request.str_comprobante_cont } );

            ds.NombreSP = "get_opis_all_spi";
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
            _logger.LogError( e, "Ocurrió un error en stored procedure get_search_transferencias" );
            _ = _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, e, _clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }

    public async Task<RespuestaTransaccion> DetalleOpi(ReqDetalleOpi request)
    {
        var respuesta = new RespuestaTransaccion();
        try
        {
            var ds = new DatosSolicitud();
            Funciones.LlenarDatosAuditoriaSalida( ds, request );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_codigo_opi", TipoDato = TipoDato.Integer, ObjValue = request.int_codigo_opi.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_tipo_ordenante", TipoDato = TipoDato.VarChar, ObjValue = request.str_tipo_ordenante } );

            ds.NombreSP = "get_detalle_opi_spi";
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
            _logger.LogError( e, "Ocurrió un error en stored procedure get_detalle_opi_spi" );
            _ = _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, e, _clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }

    public async Task<RespuestaTransaccion> ImprimirOrden(ReqImprimirOrdenPago request)
    {
        var respuesta = new RespuestaTransaccion();
        try
        {
            var ds = new DatosSolicitud();
            Funciones.LlenarDatosAuditoriaSalida( ds, request );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_codigo_opi", TipoDato = TipoDato.Integer, ObjValue = request.int_codigo_opi.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_tipo_ordenante", TipoDato = TipoDato.VarChar, ObjValue = request.str_tipo_ordenante } );

            ds.NombreSP = "get_detalle_opi_spi";
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
            _logger.LogError( e, "Ocurrió un error en stored procedure get_detalle_opi_spi" );
            _ = _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, e, _clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }

    public async Task<RespuestaTransaccion> ImprimirTransferencia(ReqImprimirTransferencia request)
    {
        var respuesta = new RespuestaTransaccion();
        var storedProcedure = request.str_tipo_persona == "J" ? "get_autorizacion_transf_ext_pj" : "get_autorizacion_transf_ext_pn";
        try
        {
            var ds = new DatosSolicitud();
            Funciones.LlenarDatosAuditoriaSalida( ds, request );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_codigo_opi", TipoDato = TipoDato.Integer, ObjValue = request.int_codigo_opi.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_tipo_ordenante", TipoDato = TipoDato.VarChar, ObjValue = request.str_tipo_ordenante } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_val_comision", TipoDato = TipoDato.Decimal, ObjValue = request.dec_valor_comision.ToString( CultureInfo.InvariantCulture ) } );


            ds.NombreSP = storedProcedure;
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
            _logger.LogError( e, "Ocurrió un error en stored procedure {StoredProcedure}", storedProcedure );
            _ = _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, e, _clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }

    public async Task<RespuestaTransaccion> CuadreOpis(ReqCuadreOpis request)
    {
        var respuesta = new RespuestaTransaccion();
        try
        {
            var ds = new DatosSolicitud();
            Funciones.LlenarDatosAuditoriaSalida( ds, request );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha", TipoDato = TipoDato.DateTime, ObjValue = request.dtt_fecha.ToString( CultureInfo.InvariantCulture ) } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_tipo_ordenante", TipoDato = TipoDato.Integer, ObjValue = request.int_tipo_ordenante.ToString() } );

            ds.NombreSP = "get_ordenes_pago";
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
            _logger.LogError( e, "Ocurrió un error en stored procedure get_ordenes_pago" );
            _ = _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, e, _clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }
    
}