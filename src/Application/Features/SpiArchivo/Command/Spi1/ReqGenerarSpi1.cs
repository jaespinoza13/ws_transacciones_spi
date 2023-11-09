using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.SpiArchivo.Command.Spi1;

public class ReqGenerarSpi1 : Header, IRequest<ResGenerarSpi1>
{
    public string str_email { get; set; } = null!;
}