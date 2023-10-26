using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.SpiArchivo.Command.Spi1;

public class ReqGenerarSpi1 : Header, IRequest<ResGenerarSpi1>
{
    public DateTime dtt_fecha_corte { get; set; } = DateTime.UtcNow;
    public string str_email { get; set; } = null!;
}