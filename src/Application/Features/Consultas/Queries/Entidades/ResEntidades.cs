using Application.Common.ISO20022.Models;
using Domain.Entities.Consultas;

namespace Application.Features.Consultas.Queries.Entidades;

public class ResEntidades: ResComun
{
    public List<EntidadFinanciera> lst_entidades { get; set; } = new();
}