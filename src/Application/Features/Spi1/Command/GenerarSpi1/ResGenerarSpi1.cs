using Application.Common.ISO20022.Models;

namespace Application.Features.Spi1.Command.GenerarSpi1;

public class ResGenerarSpi1: ResComun
{
    public string spi1_txt { get; set; } = string.Empty;
    public string spi1_md5 { get; set; } = string.Empty;
    public string spi1_zip { get; set; } = string.Empty;
    
}