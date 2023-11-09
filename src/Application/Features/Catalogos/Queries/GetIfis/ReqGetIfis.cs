using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.Catalogos.Queries.GetIfis;

public class ReqGetIfis: Header, IRequest<ResGetIfis>
{
    public string str_filtro { get; set; } = "-1";
}