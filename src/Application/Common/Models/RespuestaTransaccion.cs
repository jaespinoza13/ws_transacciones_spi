namespace Application.Common.Models;

public class RespuestaTransaccion
{
    public object cuerpo { get; set; } = new();

    public string codigo { get; set; } = string.Empty;

    public Dictionary<string, string> diccionario { get; set; } = new();
}