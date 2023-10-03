using System.Text.Json;
using AccesoDatosGrpcMongo.Neg;
using Application.Common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static AccesoDatosGrpcMongo.Neg.DALMongo;

namespace Infrastructure.GrpcClients.Mongo;

public class LogsMongoDat : IMongoDat
{
    private readonly ApiConfig _apiConfig;
    private readonly DALMongoClient _dalMongoClient;
    private readonly ILogger<LogsMongoDat> _logger;

    public LogsMongoDat(IOptionsMonitor<ApiConfig> settings, DALMongoClient dalMongoClient,
        ILogger<LogsMongoDat> logger)
    {
        _apiConfig = settings.CurrentValue;
        _dalMongoClient = dalMongoClient;
        _logger = logger;
    }

    public Task GuardarCabeceraMongo(dynamic request)
    {
        var ds = new DatosSolicitud();
        try
        {
            string solRequest = JsonSerializer.Serialize(request);
            ds.StrNameBD = _apiConfig.nombre_base_mongo;
            ds.NombreColeccion = _apiConfig.coll_peticiones;
            ds.Filter = string.Empty;
            ds.SolTran = solRequest;

            _ = _dalMongoClient.insertar_documentoAsync(ds);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error Guardar Cabecera Mongo");
        }

        return Task.CompletedTask;
    }


    public Task GuardarRespuestaMongo(dynamic request)
    {
        var ds = new DatosSolicitud();
        try
        {
            string solRequest = JsonSerializer.Serialize(request);
            ds.StrNameBD = _apiConfig.nombre_base_mongo;
            ds.NombreColeccion = _apiConfig.coll_respuesta;
            ds.Filter = string.Empty;
            ds.SolTran = solRequest;
            _ = _dalMongoClient.insertar_documentoAsync(ds);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error Guardar Respuesta Mongo");
        }

        return Task.CompletedTask;
    }

    public Task GuardarExcepcionesMongo(dynamic result, object exception)
    {
        var datosSolicitud = new DatosSolicitud();
        try
        {
            var body = new
            {
                idHeader = result.str_id_transaccion,
                result.str_id_servicio,
                result.str_nemonico_canal,
                result.dt_fecha_operacion,
                result.str_ip_dispositivo,
                result.str_login,
                result.str_id_oficina,
                rsc_res_info_adicional = result.str_res_info_adicional,
                error = exception.ToString()
            };

            var solCabecera = JsonSerializer.Serialize(body);
            datosSolicitud.StrNameBD = _apiConfig.nombre_base_mongo;
            datosSolicitud.NombreColeccion = _apiConfig.coll_errores;
            datosSolicitud.Filter = string.Empty;
            datosSolicitud.SolTran = solCabecera;

            _ = _dalMongoClient.insertar_documentoAsync(datosSolicitud);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error Guardar Exceptions Mongo");
        }

        return Task.CompletedTask;
    }

    public Task GuardarExcepcionesDataBase(dynamic result, object exception)
    {
        var datosSolicitud = new DatosSolicitud();
        try
        {
            var body = new
            {
                idHeader = result.str_id_transaccion,
                result.str_id_servicio,
                result.str_nemonico_canal,
                result.dt_fecha_operacion,
                result.str_ip_dispositivo,
                result.str_login,
                result.str_id_oficina,
                error = exception.ToString()
            };

            var solCabecera = JsonSerializer.Serialize(body);
            datosSolicitud.StrNameBD = _apiConfig.nombre_base_mongo;
            datosSolicitud.NombreColeccion = _apiConfig.coll_errores_db;
            datosSolicitud.Filter = string.Empty;
            datosSolicitud.SolTran = solCabecera;

            _ = _dalMongoClient.insertar_documentoAsync(datosSolicitud);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error Guardar Excepciones DataBase");
        }

        return Task.CompletedTask;
    }

    public Task GuardarErroresHttp(object request, object exception, string strIdTransaccion)
    {
        var datosSolicitud = new DatosSolicitud();
        try
        {
            var bjson = new
            {
                str_id_transaccion = strIdTransaccion,
                fecha = DateTime.Now,
                objeto = request,
                error = exception.ToString(),
            };

            var solCabecera = JsonSerializer.Serialize(bjson);
            datosSolicitud.StrNameBD = _apiConfig.nombre_base_mongo;
            datosSolicitud.NombreColeccion = _apiConfig.coll_errores_http;
            datosSolicitud.Filter = string.Empty;
            datosSolicitud.SolTran = solCabecera;

            _ = _dalMongoClient.insertar_documentoAsync(datosSolicitud);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error Guardar Errores Http");
        }

        return Task.CompletedTask;
    }

    public Task GuardarAmenazasMongo(ValidacionInyeccion request)
    {
        try
        {
            var datosSolicitud = new DatosSolicitud();
            var serCabecera = JsonSerializer.Serialize(request);
            datosSolicitud.StrNameBD = _apiConfig.nombre_base_mongo;
            datosSolicitud.NombreColeccion = _apiConfig.coll_amenazas;
            datosSolicitud.Filter = string.Empty;
            datosSolicitud.SolTran = serCabecera;

            _ = _dalMongoClient.insertar_documentoAsync(datosSolicitud);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error GuardarAmenazasMongo");
        }

        return Task.CompletedTask;
    }

    public RespuestaTransaccion buscar_peticiones_diarias(string filtro)
    {
        var respuesta = new RespuestaTransaccion();
        var ds = new DatosSolicitud();
        try
        {
            ds.StrNameBD = _apiConfig.nombre_base_mongo;
            ds.NombreColeccion = _apiConfig.coll_peticiones_diarias;
            ds.Filter = filtro;
            ds.SolTran = string.Empty;

            var res = _dalMongoClient.buscar_documentos(ds);

            respuesta.codigo = "000";
            respuesta.cuerpo = res.Mensaje;
        }
        catch (Exception e)

        {
            respuesta.codigo = "001";
            respuesta.diccionario.Add("str_error", e.ToString());
            _logger.LogError(e, "Error buscar_peticiones_diarias");
        }

        return respuesta;
    }

    public void actualizar_peticion_diaria(string filtro, string peticion)
    {
        var ds = new DatosSolicitud();

        try
        {
            ds.StrNameBD = _apiConfig.nombre_base_mongo;
            ds.NombreColeccion = _apiConfig.coll_peticiones_diarias;
            ds.Filter = filtro;
            ds.SolTran = peticion;
            _dalMongoClient.actualizar_documento_avanzado(ds);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error actualizar_peticion_diaria");
        }
    }

    public void guardar_promedio_peticion_diaria(string strOperacion, string strFecha)
    {
        var datosSolicitud = new DatosSolicitud();
        try
        {
            var strFiltro = "{'str_operacion':'" + strOperacion + "'}";
            datosSolicitud.StrNameBD = _apiConfig.nombre_base_mongo;
            datosSolicitud.NombreColeccion = _apiConfig.coll_promedio_peticiones_diarias;
            datosSolicitud.Filter = strFiltro;
            datosSolicitud.SolTran = string.Empty;
            var res = _dalMongoClient.buscar_documentos(datosSolicitud);
            var resultMongo = res.Mensaje;
            var promedio = calcular_promedio(strOperacion);
            if (resultMongo != null && resultMongo != "[]")
            {
                var strDatosUpdate = "{$set:{'dbl_promedio_peticion':" + promedio +
                                     ",'str_fecha_actualizacion':'" + strFecha + "'}}";

                datosSolicitud.Filter = strFiltro;
                datosSolicitud.SolTran = strDatosUpdate;

                _dalMongoClient.actualizar_documento(datosSolicitud);
            }
            else
            {
                object solicitud = new
                {
                    dbl_promedio_peticion = promedio,
                    str_operacion = strOperacion, str_fecha_actualizacion = strFecha
                };
                datosSolicitud.Filter = string.Empty;
                datosSolicitud.SolTran = JsonSerializer.Serialize(solicitud);
                _dalMongoClient.insertar_documento(datosSolicitud);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error guardar_promedio_peticion_diaria");
        }
    }

    public int calcular_promedio(string strOperacion)
    {
        var strFiltro = "[{ $match: { str_operacion:'" + strOperacion + "'} }," +
                        "{$group:{_id: '$str_operacion',dbl_promedio_peticion: { $avg: '$int_num_peticion'}}}]";
        var intRespuesta = 0;
        var ds = new DatosSolicitud();
        try
        {
            ds.StrNameBD = _apiConfig.nombre_base_mongo;
            ds.NombreColeccion = _apiConfig.coll_peticiones_diarias;
            ds.Filter = strFiltro;
            ds.SolTran = string.Empty;

            var res = _dalMongoClient.buscar_documentos_avanzado(ds);

            var resDatosMongo = res.Mensaje;
            if (resDatosMongo != null && resDatosMongo != "[]")
            {
                resDatosMongo = resDatosMongo.Replace("[", "").Replace("]", "");
                var peticionDiaria = JsonSerializer.Deserialize<PromedioPeticionDiaria>(resDatosMongo);
                intRespuesta = Convert.ToInt32(peticionDiaria!.dbl_promedio_peticion);
            }
        }
        catch (Exception e)

        {
            intRespuesta = 0;
            _logger.LogError(e, "Error calcular_promedio");
        }

        return intRespuesta;
    }

    public int obtener_promedio(string strOperacion)
    {
        var strFiltro = "{'str_operacion':'" + strOperacion + "'}";
        var intRespuesta = 0;
        var ds = new DatosSolicitud();
        try
        {
            ds.StrNameBD = _apiConfig.nombre_base_mongo;
            ds.NombreColeccion = _apiConfig.coll_promedio_peticiones_diarias;
            ds.Filter = strFiltro;
            ds.SolTran = string.Empty;

            var res = _dalMongoClient.buscar_documentos(ds);

            var resDatosMongo = res.Mensaje;
            if (resDatosMongo != null && resDatosMongo != "[]")
            {
                resDatosMongo = resDatosMongo.Replace("ObjectId(", " ").Replace(")", " ");

                resDatosMongo = resDatosMongo.Replace("[", "").Replace("]", "");
                var peticionDiaria = JsonSerializer.Deserialize<PromedioPeticionDiaria>(resDatosMongo);
                intRespuesta = Convert.ToInt32(peticionDiaria!.dbl_promedio_peticion);
            }
        }
        catch (Exception e)

        {
            intRespuesta = 0;
            
            _logger.LogError(e, "Error obtener_promedio");
        }

        return intRespuesta;
    }
}