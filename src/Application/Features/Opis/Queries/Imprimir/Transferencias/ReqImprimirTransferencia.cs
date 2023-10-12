using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.Opis.Queries.Imprimir.Transferencias;

public class ReqImprimirTransferencia : Header, IRequest<ResImprimirTransferencia>
{
    public int int_codigo_opi { get; set; }
    public string str_tipo_ordenante { get; set; } = string.Empty;
    public string str_tipo_persona { get; set; } = string.Empty;
    public decimal dec_valor_comision { get; set; }
}