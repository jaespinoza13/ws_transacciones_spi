using Application.Common.ISO20022.Models;
using Domain.Entities.Opis;

namespace Application.Features.Opis.Queries.Buscar;

public class ResBuscarOpis: ResComun
{
    public List<BuscarOpis> lst_opis { get; set; } = new();
}