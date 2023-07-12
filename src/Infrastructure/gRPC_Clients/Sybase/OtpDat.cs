using Microsoft.Extensions.Options;
using System.Reflection;

using AccesoDatosGrpcAse.Neg;
using static AccesoDatosGrpcAse.Neg.DAL;

using Application.Common.Interfaces;
using Application.Common.Models;

using Infrastructure.Common.Funciones;
using Infrastructure.Common.Interfaces;

namespace Infrastructure.gRPC_Clients.Sybase;

public class OtpDat : IOtpDat
{
    private readonly ApiSettings _settings;
    private readonly DALClient _objClienteDal;
    private readonly ILogs _logsService;
    private readonly string _strClase;

    public OtpDat(IOptionsMonitor<ApiSettings> options, ILogs logsService, DALClient objClienteDal)
    {
        _settings = options.CurrentValue;
        _logsService = logsService;

        _strClase = GetType().FullName!;

        _objClienteDal = objClienteDal;
    }


    public async Task<RespuestaTransaccion> GetDatosOtpDat(dynamic request)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            DatosSolicitud ds = new();
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_ente", TipoDato = TipoDato.Integer, ObjValue = request.str_ente } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_sistema", TipoDato = TipoDato.Integer, ObjValue = request.str_id_sistema } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_canal", TipoDato = TipoDato.VarChar, ObjValue = request.str_nemonico_canal } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_login", TipoDato = TipoDato.VarChar, ObjValue = request.str_login } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_equipo", TipoDato = TipoDato.VarChar, ObjValue = request.str_ip_dispositivo } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_sesion", TipoDato = TipoDato.VarChar, ObjValue = request.str_sesion } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_mac", TipoDato = TipoDato.VarChar, ObjValue = request.str_mac_dispositivo } );

            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@o_error_cod", TipoDato = TipoDato.Integer } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@o_error", TipoDato = TipoDato.VarChar } );

            ds.NombreSP = "get_datos_otp_gen";
            ds.NombreBD = _settings.DB_meg_servicios;

            var resultado = await _objClienteDal.ExecuteDataSetAsync( ds );
            var lstValores = resultado.ListaPSalidaValores.ToList();

            var strCodigo = lstValores.Find( x => x.StrNameParameter == "@o_error_cod" )!.ObjValue;
            var strError = lstValores.Find( x => x.StrNameParameter == "@o_error" )!.ObjValue.Trim();

            respuesta.codigo = strCodigo.Trim().PadLeft( 3, '0' );
            respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
            respuesta.diccionario.Add( "str_error", strError );
        }
        catch (Exception exception)
        {
            respuesta.codigo = "003";
            respuesta.diccionario.Add( "str_error", exception.ToString() );
            await _logsService.SaveExcepcionDataBaseSybase(
                request,
                MethodBase.GetCurrentMethod()!.Name,
                exception,
                _strClase
            );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }
}