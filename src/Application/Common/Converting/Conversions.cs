using Application.Common.Models;

namespace Application.Common.Converting;

public static class Conversions
{
    #region Métodos "Conversión de Conjunto de Datos a un objeto/Lista de una Clase"

    /// <summary>
    /// Convierte un Conjunto de datos a un objeto de una Clase específica se puede enviar la tabla(0,1..) a convertir
    /// </summary>
    /// <param name="conjuntoDatos"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    public static T ConvertToClass<T>(ConjuntoDatos conjuntoDatos, int table = 0)
    {
        var obj = default(T);
        foreach (var item in conjuntoDatos.LstTablas[table].LstFilas)
        {
            obj = (T)Converting.MapDictToObj( item.NombreValor, typeof(T) );
        }

        return obj!;
    }

    /// <summary>
    /// Convierte un Conjunto de datos a una lista de una Clase específica se puede enviar la tabla(0,1..) a convertir
    /// </summary>
    /// <param name="conjuntoDatos"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    public static IEnumerable<T> ConvertToList<T>(ConjuntoDatos conjuntoDatos, int table = 0) =>
        conjuntoDatos.LstTablas[table].LstFilas
            .Select( item => (T)Converting.MapDictToObj( item.NombreValor, typeof(T) ) ).ToList();

    #endregion
}