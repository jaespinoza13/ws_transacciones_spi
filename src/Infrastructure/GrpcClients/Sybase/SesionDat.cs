using System.Reflection;
using Microsoft.Extensions.Options;
using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Common.Functions;
using static AccesoDatosGrpcAse.Neg.DAL;

namespace Infrastructure.GrpcClients.Sybase;

public interface ISessionDat
{
    RespuestaTransaccion ControlSession(ValidaSesion validaSession);
}

public class SessionDat : ISessionDat
{
    private readonly ApiConfig _configApi;
    private readonly DALClient _dalClient;
    private readonly ILogs _logsService;
    private readonly string _strClass;

    public SessionDat(IOptionsMonitor<ApiConfig> options, ILogs logsService, DALClient dalClient)
    {
        _configApi = options.CurrentValue;
        _logsService = logsService;
        _dalClient = dalClient;
        _strClass = GetType().FullName!;
    }
    public RespuestaTransaccion ControlSession(ValidaSesion validaSession)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();
            Funciones.LlenarDatosAuditoriaSalida(ds, validaSession);

            ds.ListaPEntrada.Add(new ParametroEntrada { StrNameParameter = "@int_id_login", TipoDato = TipoDato.Integer, ObjValue = validaSession.str_id_usuario });
            ds.ListaPEntrada.Add(new ParametroEntrada { StrNameParameter = "@int_estado", TipoDato = TipoDato.Integer, ObjValue = validaSession.int_estado.ToString() });


            ds.NombreSP = "get_validar_estado_sesion";
            ds.NombreBD = _configApi.db_meg_servicios;

            var resultado = _dalClient.ExecuteDataSet(ds);
            var lstValores = resultado.ListaPSalidaValores.ToList();

            var strCodigo = lstValores.Find(x => x.StrNameParameter == "@int_o_error_cod")!.ObjValue;
            var strError = lstValores.Find(x => x.StrNameParameter == "@str_o_error")!.ObjValue.Trim();

            respuesta.codigo = strCodigo.Trim().PadLeft(3, '0');
            respuesta.cuerpo = Funciones.ObtenerDatos(resultado);
            respuesta.diccionario.Add("str_error", strError);
        }
        catch (Exception exception)
        {
            respuesta.codigo = "001";
            respuesta.diccionario.Add("str_error", exception.ToString());
            _logsService.SaveExcepcionDataBaseSybase(
                validaSession,
                MethodBase.GetCurrentMethod()!.Name,
                exception,
                _strClass
            );
            throw new ArgumentException(validaSession.str_id_transaccion);
        }

        return respuesta;
    }
}