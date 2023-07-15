using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.Opis.Queries.Imprimir.OrdenPago;

public class ReqImprimirOrdenPago : Header, IRequest<ResImprimirOrdenPago>
{
    public int int_codigo_opi { get; set; }
    public string str_tipo_ordenante { get; set; } = string.Empty;
}