namespace Domain.Entities.SpiArchivo;

public class DetalleSpi1Consolidado: DetalleSpi1
{
    public string str_usuario { get; set; } = string.Empty;
    public string str_banco { get; set; } = string.Empty;
    public int int_tipo_ordenante { get; set; }
}