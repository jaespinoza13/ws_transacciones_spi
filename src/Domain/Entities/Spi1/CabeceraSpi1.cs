namespace Domain.Entities.Spi1;

public class CabeceraSpi1
{
    public int int_codigo_spi1 { get; set; }
    public DateTime dtt_fecha_envio { get; set; } = DateTime.Now;
    public int int_num_total_opi { get; set; }
    public decimal dec_monto_total_opi { get; set; }
    public int int_num_control { get; set; }
    public string str_num_cuenta_io { get; set; } = string.Empty;
    public string str_nom_archivo_spi1 { get; set; } = string.Empty;
    public DateTime dtt_hora_genera_spi1 { get; set; } = DateTime.Now;
    public string str_usuario_genera_spi1 { get; set; } = string.Empty;
    
}