using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Parametros.Queries.GetParametros;
using Application.Persistence;
using Infrastructure.Common.Functions;


namespace Infrastructure.GrpcClients.Sybase.Parametros;

public class ParametrosDat : IParametroDat
{
    private readonly ILogs _logService;
    private readonly DAL.DALClient _objClientDal;
    private readonly ApiConfig _apiConfig;
    private readonly string _clase;
    private readonly ILogger<ParametrosDat> _logger;

    public ParametrosDat(ILogger<ParametrosDat> logger, ILogs logService, DAL.DALClient objClientDal, IOptions<ApiConfig> apiConfig)
    {
        _logger = logger;
        _logService = logService;
        _objClientDal = objClientDal;
        _apiConfig = apiConfig.Value;
        _clase = GetType().FullName!;
    }

    public async Task<RespuestaTransaccion> GetParametros(ReqGetParametros request)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();
            
            Funciones.LlenarDatosAuditoriaSalida( ds, request );
            
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_valor_busqueda", TipoDato = TipoDato.VarChar, ObjValue = request.str_valor_busqueda } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_campo_busqueda", TipoDato = TipoDato.VarChar, ObjValue = request.str_campo_busqueda } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_vigencia", TipoDato = TipoDato.Integer, ObjValue = request.int_vigencia.ToString() } );
            if (request.dtt_fecha_desde != null)
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha_desde", TipoDato = TipoDato.VarChar, ObjValue = request.dtt_fecha_desde } );
            if (request.dtt_fecha_hasta != null)
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha_hasta", TipoDato = TipoDato.VarChar, ObjValue = request.dtt_fecha_hasta } );
            
            ds.NombreSP = "get_parametros_v2";
            ds.NombreBD = _apiConfig.db_meg_sistemas;

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
            respuesta.codigo = "003";
            respuesta.diccionario.Add( "str_error", e.InnerException?.Message ?? e.Message );
            _logger.LogError( e, "Error en stored procedure get_parametros_v2" );
            _ = _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, e, _clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }
}