using Application.Common.Models;

namespace Infrastructure.gRPC_Clients.Mongo;

public interface IMongoDat
{
    Task GuardarCabeceraMongo(dynamic cabecera);
    Task GuardarRespuestaMongo(dynamic objRespPeticion);
    Task GuardarExcepcionesMongo(dynamic objRespuesta, object excepcion);
    Task GuardarExcepcionesDataBase(dynamic objRespuesta, object excepcion);
    Task GuardarAmenazasMongo(ValidacionInyeccion objRespuesta);
    Task GuardaErroresHttp(object any, object excepcion, string strIdTransaccion);
    RespuestaTransaccion buscar_peticiones_diarias(string filtro);
    void actualizar_peticion_diaria(string filtro, string peticion);
    void guardar_promedio_peticion_diaria(string strOperacion, string strFecha);
    int calcular_promedio(string strOperacion);
    int obtener_promedio(string strOperacion);
}