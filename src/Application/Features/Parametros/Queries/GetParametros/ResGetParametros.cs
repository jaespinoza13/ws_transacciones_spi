using Application.Common.ISO20022.Models;
using Domain.Entities;

namespace Application.Features.Parametros.Queries.GetParametros;

public class ResGetParametros : ResComun
{
    public List<Parametro> lst_parametros { get; set; } = new();
}