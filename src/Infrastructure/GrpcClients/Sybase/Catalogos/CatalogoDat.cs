using System.Reflection;
using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Catalogos.Queries.GetCatalogos;
using Application.Features.Catalogos.Queries.GetIfis;
using Application.Persistence;
using Infrastructure.Common.Functions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.GrpcClients.Sybase.Catalogos;

public class CatalogoDat: ICatalogoDat
{
    private readonly ILogs _logService;
    private readonly DAL.DALClient _objClientDal;
    private readonly ApiConfig _apiConfig;
    private readonly string _clase;
    private readonly ILogger<CatalogoDat> _logger;
    
    public CatalogoDat(ILogger<CatalogoDat> logger, ILogs logService, DAL.DALClient objClientDal, IOptions<ApiConfig> apiConfig)
    {
        _logger = logger;
        _logService = logService;
        _objClientDal = objClientDal;
        _apiConfig = apiConfig.Value;
        _clase = GetType().FullName!;
    }
    
    public async Task<RespuestaTransaccion> GetCatalogos(ReqGetCatalogo request)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();
            
            Funciones.LlenarDatosAuditoriaSalida( ds, request );
            
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_catalogo", TipoDato = TipoDato.VarChar, ObjValue = request.str_catalogo } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_filtro", TipoDato = TipoDato.VarChar, ObjValue = request.str_filtro } );

            ds.NombreSP = "get_catalogos_spi";
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
            respuesta.codigo = "003";
            respuesta.diccionario.Add( "str_error", e.InnerException?.Message ?? e.Message );
            _logger.LogError( e, "Ocurrió un error en stored procedure get_catalogos_spi" );
            _ = _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, e, _clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }

    public async Task<RespuestaTransaccion> GetIfis(ReqGetIfis request)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();
            
            Funciones.LlenarDatosAuditoriaSalida( ds, request );
            
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_filtro", TipoDato = TipoDato.VarChar, ObjValue = request.str_filtro } );

            ds.NombreSP = "get_ifis_spi";
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
            respuesta.codigo = "003";
            respuesta.diccionario.Add( "str_error", e.InnerException?.Message ?? e.Message );
            _logger.LogError( e, "Ocurrió un error en stored procedure get_ifis_spi" );
            _ = _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, e, _clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }
}