namespace Application.Common.Models;
public class InfoLog
{
    /// <summary>
    /// Fecha de transacción
    /// </summary>
    public DateTime str_fecha { get; set; }
    /// <summary>
    /// Id de la transacción
    /// </summary>
    public string str_id_transaccion { get; set; } = string.Empty;
    /// <summary>
    /// Tipo de log. 
    /// </summary>
    public string str_tipo { get; set; } = string.Empty;
    /// <summary>
    /// Web service desde el cual se está realizando la transacción
    /// </summary>
    public string str_webservice { get; set; } = string.Empty;
    /// <summary>
    /// Clase del Web Service desde el cual se está realizando la transacción
    /// </summary>
    public string? str_clase { get; set; }
    /// <summary>
    /// Metodo de la clase en la cual se está realizando la transacción
    /// </summary>
    public string str_metodo { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de la operación o proceso que se esté realizando 
    /// </summary>
    public string str_operacion { get; set; } = string.Empty;
    /// <summary>
    /// Objeto de transacción (iso20022 o genérico)
    /// </summary>
    public object str_objeto { get; set; } = new ();
}