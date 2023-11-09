namespace Domain.Entities;

public class Ifi
{
    public string str_codigo_ifi { get; set; } = string.Empty;
    public string str_nombre_ifi { get; set; } = string.Empty;
    public string str_cuenta_bce { get; set; } = string.Empty;
    public string str_bin_ordenante { get; set; } = string.Empty;
    public string str_bin_receptor { get; set; } = string.Empty;
    public int int_fi_adquiriente { get; set; }
    public int int_fi_autorizador { get; set; }
    public string str_logo_ifi { get; set; } = string.Empty;
    public string str_tipo { get; set; } = string.Empty;
}