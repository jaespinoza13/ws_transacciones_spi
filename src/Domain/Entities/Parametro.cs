namespace Domain.Entities;

public class Parametro
{
    public int int_id_parametro { get; set; }
    public int int_id_sistema { get; set; }
    public string str_nombre { get; set; } = string.Empty;
    public string str_nemonico { get; set; } = string.Empty;
    public string str_valor_ini { get; set; } = string.Empty;
    public string str_valor_fin { get; set; } = string.Empty;
    public string dtt_fecha_desde { get; set; } = string.Empty;
    public string? dtt_fecha_hasta { get; set; } = null;
    public string str_descripcion { get; set; } = string.Empty;
    public bool bit_vigencia { get; set; }
}