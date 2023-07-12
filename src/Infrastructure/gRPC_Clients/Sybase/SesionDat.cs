using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Common.Funciones;
using Microsoft.Extensions.Options;
using System.Reflection;
using static AccesoDatosGrpcAse.Neg.DAL;

namespace Infrastructure.gRPC_Clients.Sybase;

public interface ISesionDat
{
    RespuestaTransaccion ControlSesion(ValidaSesion validaSesion);
}

public class SesionDat : ISesionDat
{
    private readonly ApiSettings _settings;
    private readonly DALClient _objClienteDal;
    private readonly ILogs _logsService;
    private readonly string _strClase;

    public SesionDat(IOptionsMonitor<ApiSettings> options, ILogs logsService, DALClient objClienteDal)
    {
        _settings = options.CurrentValue;
        _logsService = logsService;
        _objClienteDal = objClienteDal;
        _strClase = GetType().FullName!;
    }


    public RespuestaTransaccion ControlSesion(ValidaSesion validaSesion)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();
            Funciones.LlenarDatosAuditoriaSalida( ds, validaSesion );

            ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@int_id_login", TipoDato = TipoDato.Integer,ObjValue = validaSesion.str_id_usuario } );
            ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@int_estado", TipoDato = TipoDato.Integer,ObjValue = validaSesion.int_estado.ToString()} );


            ds.NombreSP = "get_validar_estado_sesion";
            ds.NombreBD = _settings.DB_meg_servicios;

            var resultado = _objClienteDal.ExecuteDataSet( ds );
            var lstValores = resultado.ListaPSalidaValores.ToList();

            var strCodigo = lstValores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
            var strError = lstValores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();

            respuesta.codigo = strCodigo.Trim().PadLeft( 3, '0' );
            respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
            respuesta.diccionario.Add( "str_error", strError );
        }
        catch (Exception exception)
        {
            respuesta.codigo = "001";
            respuesta.diccionario.Add( "str_error", exception.ToString() );
            _logsService.SaveExcepcionDataBaseSybase(
                validaSesion,
                MethodBase.GetCurrentMethod()!.Name,
                exception,
                _strClase
            );
            throw new ArgumentException( validaSesion.str_id_transaccion );
        }

        return respuesta;
    }
}