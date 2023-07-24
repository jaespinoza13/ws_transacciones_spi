using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.Consultas.Queries.Entidades;

public class ReqEntidades: Header, IRequest<ResEntidades>
{
    public string str_filtro { get; set; } = "-1";
}