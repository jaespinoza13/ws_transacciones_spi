using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Common.Tramas;
using Infrastructure.gRPC_Clients.Mongo;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class LogsService : ILogs
{
    private readonly InfoLog _infoLog = new();
    private readonly ApiSettings _settings;
    private readonly IMongoDat _mongoDat;

    public LogsService(IOptionsMonitor<ApiSettings> options, IMongoDat mongoDat)
    {
        _settings = options.CurrentValue;
        _mongoDat = mongoDat;
    }

    /// <summary>
    /// Guarda en mongodb la cabecera de la petición
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="strOperacion"></param>
    /// <param name="strMetodo"></param>
    /// <param name="strClase"></param>
    /// <returns></returns>
    ///
    public async Task SaveHeaderLogs(dynamic transaction, string strOperacion, string strMetodo, string strClase)
    {
        _infoLog.str_id_transaccion = transaction.str_id_transaccion;
        _infoLog.str_clase = strClase;
        _infoLog.str_operacion = strOperacion;
        _infoLog.str_objeto = transaction;
        _infoLog.str_metodo = strMetodo;
        _infoLog.str_fecha = transaction.dt_fecha_operacion;
        _infoLog.str_tipo = "s:<";

        // REGISTRA LOGS DE TEXTO
        TextFiles.RegistrarTramas( _infoLog.str_tipo, _infoLog, _settings.logs_path_peticiones );

        // REGISTRA LOGS DE MONGO
        await _mongoDat.GuardarCabeceraMongo( transaction );
    }

    /// <summary>
    /// Guarda en mongodb la respuesta de la petición
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="strOperacion"></param>
    /// <param name="strMetodo"></param>
    /// <param name="strClase"></param>
    /// <returns></returns>
    ///
    public async Task SaveResponseLogs(dynamic transaction, string strOperacion, string strMetodo, string strClase)
    {
        _infoLog.str_id_transaccion = transaction.str_id_transaccion;
        _infoLog.str_clase = strClase;
        _infoLog.str_operacion = strOperacion;
        _infoLog.str_objeto = transaction;
        _infoLog.str_metodo = strMetodo;
        _infoLog.str_fecha = transaction.dt_fecha_operacion;
        _infoLog.str_tipo = "r:>";

        // REGISTRA LOGS DE TEXTO
        TextFiles.RegistrarTramas( _infoLog.str_tipo, _infoLog, _settings.logs_path_peticiones );

        // REGISTRA LOGS DE MONGO
        await _mongoDat.GuardarRespuestaMongo( transaction );
    }

    /// <summary>
    /// Guarda exepciones de codigo
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="strOperacion"></param>
    /// <param name="strMetodo"></param>
    /// <param name="strClase"></param>
    /// <param name="objError"></param>
    /// <returns></returns>
    ///
    public async Task SaveExceptionLogs(dynamic transaction, string strOperacion, string strMetodo, string strClase,object objError)
    {
        var objSave = new { peticion = transaction, error = objError };
        _infoLog.str_id_transaccion = transaction.str_id_transaccion;
        _infoLog.str_clase = strClase;
        _infoLog.str_operacion = strOperacion;
        _infoLog.str_objeto = objSave.ToString()!;
        _infoLog.str_metodo = strMetodo;
        _infoLog.str_fecha = transaction.dt_fecha_operacion;
        _infoLog.str_tipo = "e:<";

        // REGISTRA LOGS DE TEXTO
        TextFiles.RegistrarTramas( _infoLog.str_tipo, _infoLog, _settings.logs_path_errores );

        //REGISTRA LOGS DE MONGO
        await _mongoDat.GuardarExcepcionesMongo( transaction, objError );
    }

    public async Task SaveAmenazasLogs(ValidacionInyeccion validacion, string strOperacion, string strMetodo,string strClase)
    {
        _infoLog.str_clase = strClase;
        _infoLog.str_operacion = strOperacion;
        _infoLog.str_objeto = validacion;
        _infoLog.str_metodo = strMetodo;
        _infoLog.str_fecha = validacion.dtt_fecha;
        _infoLog.str_id_transaccion = validacion.idHeader;
        _infoLog.str_tipo = "s:<";

        // REGISTRA LOGS DE TEXTO
        TextFiles.RegistrarTramas( _infoLog.str_tipo, _infoLog, _settings.logs_path_amenazas );

        //REGISTRA LOGS DE MONGO
        await _mongoDat.GuardarAmenazasMongo( validacion );
    }

    /// <summary>
    /// Guarda cualquier error
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="strMetodo"></param>
    /// <param name="strClase"></param>
    /// <param name="objError"></param>
    /// <param name="strIdTransaccion"></param>
    /// <returns></returns>
    ///
    public async Task SaveHttpErrorLogs(dynamic transaction, string strMetodo, string strClase, dynamic objError,
        string strIdTransaccion)
    {
        var objSave = new { peticion = transaction, error = objError };

        _infoLog.str_id_transaccion = strIdTransaccion;
        _infoLog.str_clase = strClase;
        _infoLog.str_objeto = objSave.ToString()!;
        _infoLog.str_metodo = strMetodo;
        _infoLog.str_fecha = DateTime.Now;
        _infoLog.str_tipo = "e:<";

        //REGISTRA LOGS DE TEXTO
        TextFiles.RegistrarTramas( _infoLog.str_tipo, _infoLog, _settings.logs_path_errores_http );

        //REGISTRA LOGS DE MONGO
        await _mongoDat.GuardaErroresHttp( transaction, objError, strIdTransaccion );
    }

    public async Task SaveExcepcionDataBaseSybase(dynamic transaction, string strMetodo, Exception excepcion,
        string strClase)
    {
        _infoLog.str_id_transaccion = transaction.str_id_transaccion;
        _infoLog.str_clase = strClase;
        _infoLog.str_operacion = transaction.str_id_servicio;
        _infoLog.str_objeto = excepcion.ToString();
        _infoLog.str_metodo = strMetodo;
        _infoLog.str_fecha = transaction.dt_fecha_operacion;
        _infoLog.str_tipo = "e:<";

        //REGISTRA LOGS DE TEXTO
        TextFiles.RegistrarTramas( _infoLog.str_tipo, _infoLog, _settings.logs_path_errores_db );

        //REGISTRA LOGS DE MONGO
        await _mongoDat.GuardarExcepcionesDataBase( transaction, excepcion );
    }
}