using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.Parametros.Queries.GetParametros;

public class ReqGetParametros : Header, IRequest<ResGetParametros>
{
    public string str_valor_busqueda { get; set; } = null!;
    public string str_campo_busqueda { get; set; } = null!;
    public int int_vigencia { get; set; } = 1;
    public string? dtt_fecha_desde { get; set; } = null;
    public string? dtt_fecha_hasta { get; set; } = null;
}