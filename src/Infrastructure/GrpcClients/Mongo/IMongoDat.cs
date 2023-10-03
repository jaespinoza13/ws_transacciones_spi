using Application.Common.Models;

namespace Infrastructure.GrpcClients.Mongo;

public interface IMongoDat
{
    Task GuardarCabeceraMongo(dynamic cabecera);
    Task GuardarRespuestaMongo(dynamic request);
    Task GuardarExcepcionesMongo(dynamic objRespuesta, object exception);
    Task GuardarExcepcionesDataBase(dynamic objRespuesta, object exception);
    Task GuardarAmenazasMongo(ValidacionInyeccion objRespuesta);
    Task GuardarErroresHttp(object any, object exception, string strIdTransaccion);
    RespuestaTransaccion buscar_peticiones_diarias(string filtro);
    void actualizar_peticion_diaria(string filtro, string peticion);
    void guardar_promedio_peticion_diaria(string strOperacion, string strFecha);
    int calcular_promedio(string strOperacion);
    int obtener_promedio(string strOperacion);
}