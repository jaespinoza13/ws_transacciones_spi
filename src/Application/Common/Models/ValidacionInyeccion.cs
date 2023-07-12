namespace Application.Common.Models;

public class ValidacionInyeccion
{
    public string idHeader { get; set; } = string.Empty;
    public string str_servicio { get; set; } = string.Empty;
    public string str_valor { get; set; } = string.Empty;
    public string str_campo { get; set; } = string.Empty;
    public string str_mensaje { get; set; } = string.Empty;
    public string str_informacion_adicional { get; set; } = string.Empty;
    public string str_tipo { get; set; } = string.Empty;
    public string str_ip { get; set; } = string.Empty;
    public DateTime dtt_fecha { get; set; }
}