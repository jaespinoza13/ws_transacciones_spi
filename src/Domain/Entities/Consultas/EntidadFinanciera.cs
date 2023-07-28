namespace Domain.Entities.Consultas;

public class EntidadFinanciera
{
    public string str_codigo_ifi { get; set; } = string.Empty;
    public string str_nombre_ifi { get; set; } = string.Empty;
    public string str_tipo { get; set; } = string.Empty;
    public string str_url_logo_ifi { get; set; } = string.Empty;
    public int int_cod_ifi_autorizador { get; set; }
    public string str_bin_receptor { get; set; } = string.Empty;
    public string str_cuenta_bce { get; set; } = string.Empty;
}