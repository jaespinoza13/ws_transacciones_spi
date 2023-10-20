namespace Domain.Entities.SpiArchivo;

public class CabeceraSpi1
{
    public int int_codigo_spi1 { get; set; }
    public DateTime dtt_fecha_envio { get; set; } = DateTime.UtcNow;
    public int int_num_total_opi { get; set; }
    public string dec_monto_total_opi { get; set; } = string.Empty;
    public string int_num_control { get; set; } = string.Empty;
    public string str_num_cuenta_io { get; set; } = string.Empty;
    public string str_nom_archivo_spi1 { get; set; } = string.Empty;
    public DateTime dtt_hora_genera_spi1 { get; set; } = DateTime.UtcNow;
    public string str_usuario_genera_spi1 { get; set; } = string.Empty;
    public int int_numero_envio { get; set; }
}