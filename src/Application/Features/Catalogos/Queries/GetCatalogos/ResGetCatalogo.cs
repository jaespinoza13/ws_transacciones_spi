using Application.Common.ISO20022.Models;
using Domain.Entities;

namespace Application.Features.Catalogos.Queries.GetCatalogos;

public class ResGetCatalogo: ResComun
{
    public IReadOnlyList<Catalogo> lst_catalogos { get; set; } = new List<Catalogo>();
}