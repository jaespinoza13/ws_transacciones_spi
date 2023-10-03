using System.Reflection;
using AccesoDatosGrpcAse.Neg;
using Application.Common.Cryptography;
using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Common.Functions;
using Microsoft.Extensions.Options;
using static AccesoDatosGrpcAse.Neg.DAL;

namespace Infrastructure.GrpcClients.Sybase;

public class KeysDat : IKeysDat
{
    private readonly ApiConfig _settings;
    private readonly DALClient _objClientDal;
    private readonly ILogs _logsService;
    private readonly string _strClase;

    public KeysDat(IOptionsMonitor<ApiConfig> options, ILogs logsService, DALClient objClientDal)
    {
        _settings = options.CurrentValue;
        _logsService = logsService;

        _strClase = GetType().FullName!;
        _objClientDal = objClientDal;
    }

    public RespuestaTransaccion AddKeys(ReqAddKeys reqAddKeys)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            DatosSolicitud ds = new();

            Funciones.LlenarDatosAuditoriaSalida(ds, reqAddKeys);

            ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@int_ente", TipoDato = TipoDato.Integer, ObjValue = reqAddKeys.str_ente } );
            ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_modulo", TipoDato = TipoDato.VarChar, ObjValue = reqAddKeys.str_modulo} );
            ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_exponente", TipoDato = TipoDato.VarChar,ObjValue = reqAddKeys.str_exponente} );
            ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_llave_privada", TipoDato = TipoDato.Text,ObjValue = reqAddKeys.str_llave_privada} );
            ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_llave_simetrica", TipoDato = TipoDato.VarChar,ObjValue = reqAddKeys.str_llave_simetrica} );
            ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_llave_secreta", TipoDato = TipoDato.VarChar,ObjValue = reqAddKeys.str_clave_secreta } );


            ds.NombreSP = "add_llaves_cifrado";
            ds.NombreBD = _settings.db_meg_servicios;

            var resultado = _objClientDal.ExecuteDataSet(ds);
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
                reqAddKeys,
                MethodBase.GetCurrentMethod()!.Name,
                exception,
                _strClase
            );
            throw new ArgumentException(reqAddKeys.str_id_transaccion);
        }

        return respuesta;
    }


    public RespuestaTransaccion GetKeys(ReqGetKeys reqGetKeys)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            DatosSolicitud ds = new();
            
            Funciones.LlenarDatosAuditoriaSalida( ds, reqGetKeys );
            ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_llave_secreta", TipoDato = TipoDato.VarChar,ObjValue = reqGetKeys.str_clave_secreta} );
            
            ds.NombreSP = "get_llaves_cifrado";
            ds.NombreBD = _settings.db_meg_servicios;

            var resultado = _objClientDal.ExecuteDataSet(ds);
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
                reqGetKeys,
                MethodBase.GetCurrentMethod()!.Name,
                exception,
                _strClase
            );
            throw new ArgumentException(reqGetKeys.str_id_transaccion);
        }

        return respuesta;
    }
}