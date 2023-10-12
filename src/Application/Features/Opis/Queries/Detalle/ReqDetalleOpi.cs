using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.Opis.Queries.Detalle;

public class ReqDetalleOpi : Header, IRequest<ResDetalleOpi>
{
    public int int_codigo_opi { get; set; }
    public string str_tipo_ordenante { get; set; } = string.Empty;
}