using MediatR;
using Application.Common.ISO20022.Models;

namespace Application.Features.Spi1.Command.GenerarSpi1;

public class ReqGenerarSpi1 : Header, IRequest<ResGenerarSpi1>
{
    public DateTime dtt_fecha_corte { get; set; } = DateTime.Now;
}