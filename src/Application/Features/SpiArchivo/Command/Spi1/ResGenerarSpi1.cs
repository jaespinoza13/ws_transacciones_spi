using Application.Common.ISO20022.Models;

namespace Application.Features.SpiArchivo.Command.Spi1;

public class ResGenerarSpi1 : ResComun
{
    public string spi1_txt { get; set; } = string.Empty;
    public string spi1_md5 { get; set; } = string.Empty;
    public string str_nombre_archivo { get; set; } = string.Empty;
    public int int_cantidad_registros { get; set; }
    public int int_numero_corte { get; set; }
}