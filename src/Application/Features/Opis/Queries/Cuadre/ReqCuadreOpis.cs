using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.Opis.Queries.Cuadre;

public class ReqCuadreOpis : Header, IRequest<ResCuadreOpis>
{
    public DateTime dtt_fecha { get; set; }
    public int int_tipo_ordenante { get; set; }
}